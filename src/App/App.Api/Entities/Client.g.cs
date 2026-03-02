//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 3/1/2026 10:25 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------
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

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string Key = "Key";
		public const string Name = "Name";
		public const string BaseUrl = "BaseUrl";
		public const string RowVersion = "RowVersion";
	}

	#endregion
}
