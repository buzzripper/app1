namespace Dyvenix.App1.Functions.DTOs.EntraId;

public class UserInfo
{
    public DateTime CreatedDateTime { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Id { get; set; } = string.Empty;
    public string UserPrincipalName { get; set; } = string.Empty;
    public UserType UserType { get; set; }
}
