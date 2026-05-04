
namespace Dyvenix.App1.Common.Shared.Contracts;

public interface IAuditable
{
    DateTime CreatedUtc { get; set; }
    Guid? CreatedByUserId { get; set; }
    DateTime ModifiedUtc { get; set; }
    Guid? ModifiedByUserId { get; set; }
}
