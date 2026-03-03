using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Dyvenix.App1.Auth.Api.Context;

public class AuthIdentityDb : IdentityDbContext<IdentityUser, IdentityRole, string>
{
	public AuthIdentityDb(DbContextOptions<AuthIdentityDb> options) : base(options)
	{
	}
}
