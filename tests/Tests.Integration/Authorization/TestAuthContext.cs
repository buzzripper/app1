using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Dyvenix.App1.Tests.Integration.Authorization;

public sealed class TestAuthContext
{
	private IReadOnlyCollection<string> _permissions = [];

	public void SetPermissions(params string[] permissions)
	{
		_permissions = permissions ?? [];
	}

	public string CreateToken()
	{
		if (_permissions.Count == 0)
			return string.Empty;

		var claims = _permissions.Select(permission => new Claim("permissions", permission));
		var token = new JwtSecurityToken(claims: claims);

		return new JwtSecurityTokenHandler().WriteToken(token);
	}
}
