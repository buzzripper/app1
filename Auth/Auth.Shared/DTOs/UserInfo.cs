namespace App1.Auth.Shared.DTOs;

/// <summary>
/// Represents the current user's authentication status and claims.
/// Used by the /api/auth/user endpoint to return user information to clients.
/// </summary>
public class UserInfo
{
	public static readonly UserInfo Anonymous = new();

	public bool IsAuthenticated { get; set; }

	public string NameClaimType { get; set; } = string.Empty;

	public string RoleClaimType { get; set; } = string.Empty;

	public ICollection<ClaimValue> Claims { get; set; } = [];
}
