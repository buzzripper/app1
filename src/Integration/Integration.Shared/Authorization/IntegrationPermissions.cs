namespace Dyvenix.App1.Integration.Shared.Authorization;

public static class IntegrationPermissions
{
    public const string Read = "integration:read";
    public const string Write = "integration:write";
    public const string Admin = "integration:admin";

    public static readonly IReadOnlyDictionary<string, string[]> Hierarchy =
        new Dictionary<string, string[]>
        {
            [Admin] = [Write, Read],
            [Write] = [Read],
        };
}
