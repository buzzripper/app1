namespace Dyvenix.App1.Tests.Common.Core.Data;

public class DataSet
{
	#region Properties

	public DataSetType Type { get; set; }
	public List<AppUser> AppUser { get; set; } = new List<AppUser>();
	public List<AccessClaim> AccessClaim { get; set; } = new List<AccessClaim>();

	#endregion

	public AppUser CreateAppUser()
	{
		var appUser = new AppUser
		{
			Id = Guid.NewGuid()
		};
		this.AppUser.Add(appUser);
		return appUser;
	}

	public AccessClaim CreateAccessClaim()
	{
		var accessClaim = new AccessClaim
		{
			Id = Guid.NewGuid()
		};
		this.AccessClaim.Add(accessClaim);
		return accessClaim;
	}
}
