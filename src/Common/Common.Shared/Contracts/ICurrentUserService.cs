namespace Dyvenix.App1.Common.Shared.Contracts;

public interface ICurrentUserService
{
	bool IsAuthenticated { get; }
	Guid? UserId { get; }
}
