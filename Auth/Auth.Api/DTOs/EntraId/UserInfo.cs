using System;

namespace App1.Auth.Api.DTOs.EntraId;

public class UserInfo
{
	public DateTime CreatedDateTime { get; set; }
	public string DisplayName { get; set; }
	public string Id { get; set; }
	public string UserPrincipalName { get; set; }
	public UserType UserType { get; set; }
}
