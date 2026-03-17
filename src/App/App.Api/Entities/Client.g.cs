using Dyvenix.App1.App.Shared.Dtos;

namespace Dyvenix.App1.App.Api.Entities;

public partial class Client
{
	// PK
	public Guid Id { get; set; }

	// Rowversion
	public byte[] RowVersion { get; set; } = null!;

	// Properties
	public string Key { get; set; } = null!;
	public string Name { get; set; } = null!;
	public string? BaseUrl { get; set; }
	public DateTime CreatedUtc { get; set; }
	public Guid? CreatedByUserId { get; set; }
	public DateTime ModifiedUtc { get; set; }
	public Guid? ModifiedByUserId { get; set; }
	public DateTime? DeletedUtc { get; set; }
	public Guid? DeletedByUserId { get; set; }

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string Key = "Key";
		public const string Name = "Name";
		public const string BaseUrl = "BaseUrl";
		public const string RowVersion = "RowVersion";
		public const string CreatedUtc = "CreatedUtc";
		public const string CreatedByUserId = "CreatedByUserId";
		public const string ModifiedUtc = "ModifiedUtc";
		public const string ModifiedByUserId = "ModifiedByUserId";
		public const string DeletedUtc = "DeletedUtc";
		public const string DeletedByUserId = "DeletedByUserId";
	}

	#endregion
}
