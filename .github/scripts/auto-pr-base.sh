#!/usr/bin/env bash
set -euo pipefail

# This script attempts to set a PR's base branch to the branch the PR's head was branched from.
# It runs in pull_request_target context so it has permission to update the PR using GITHUB_TOKEN.

PR_NUMBER=${PR_NUMBER:-$(jq -r .pull_request.number < "$GITHUB_EVENT_PATH")}
REPO_FULL=${GITHUB_REPOSITORY}
API_URL="https://api.github.com/repos/${REPO_FULL}"

# Get PR details
pr_json=$(curl -sSf -H "Authorization: token ${GITHUB_TOKEN}" "${API_URL}/pulls/${PR_NUMBER}")
head_ref=$(echo "$pr_json" | jq -r .head.ref)
base_ref=$(echo "$pr_json" | jq -r .base.ref)
head_sha=$(echo "$pr_json" | jq -r .head.sha)

# Find recent branches that contain the immediate parent commit of the head
# We'll look at the commit parents and see which branch contains the first parent
parents=$(curl -sSf -H "Authorization: token ${GITHUB_TOKEN}" "${API_URL}/commits/${head_sha}")
first_parent_sha=$(echo "$parents" | jq -r '.parents[0].sha // empty')

candidate_branches=()
if [ -n "$first_parent_sha" ]; then
  branches_json=$(curl -sSf -H "Authorization: token ${GITHUB_TOKEN}" "${API_URL}/commits/${first_parent_sha}/branches-where-head")
  # branches-where-head returns an array of branch names
  mapfile -t candidate_branches < <(echo "$branches_json" | jq -r '.[]')
fi

# Fallback: look at recent branches from the repo (most recently updated)
if [ ${#candidate_branches[@]} -eq 0 ]; then
  recent_branches=$(curl -sSf -H "Authorization: token ${GITHUB_TOKEN}" "${API_URL}/branches?per_page=50")
  # sort by commit date is not trivial here; just take branches excluding main / master
  mapfile -t candidate_branches < <(echo "$recent_branches" | jq -r '.[] | .name' | grep -v -E '^(main|master)$' | head -n 10)
fi

# Decide the best candidate - prefer a branch that is not the current base and not main/master
selected=""
for b in "${candidate_branches[@]}"; do
  if [ "$b" != "$base_ref" ] && [[ ! "$b" =~ ^(main|master)$ ]]; then
    selected="$b"
    break
  fi
done

# If nothing selected, skip
if [ -z "$selected" ]; then
  echo "No suitable base branch discovered. Leaving as $base_ref"
  exit 0
fi

# Update the PR base
echo "Setting PR #${PR_NUMBER} base to ${selected} (was ${base_ref})"
patch=$(jq -n --arg b "$selected" '{base: $b}')
curl -sSf -X PATCH -H "Authorization: token ${GITHUB_TOKEN}" -H "Content-Type: application/json" -d "$patch" "${API_URL}/pulls/${PR_NUMBER}" >/dev/null

echo "Done"
