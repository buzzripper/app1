//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 1/28/2026 3:17 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.Common.Shared.Entities;

public partial class Person
{
	// PK
	public Guid Id { get; set; }

	// Properties
	public string LastName { get; set; } = null!;
	public string FirstName { get; set; } = null!;
	public string Email { get; set; } = null!;
	public string NewProperty { get; set; } = null!;

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string FirstName = "FirstName";
		public const string LastName = "LastName";
		public const string Email = "Email";
		public const string NewProperty = "NewProperty";
	}

	#endregion
}
