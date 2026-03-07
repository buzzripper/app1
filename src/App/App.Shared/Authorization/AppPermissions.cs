// src/App/App.Shared/Authorization/AppPermissions.cs
namespace Dyvenix.App1.App.Shared.Authorization;

public static class AppPermissions
{
    public const string Read = "app:read";
    public const string Write = "app:write";
    public const string Admin = "app:admin";

    public static readonly IReadOnlyDictionary<string, string[]> Hierarchy =
        new Dictionary<string, string[]>
        {
            [Admin] = [Write, Read],
            [Write] = [Read],
        };
}
