//------------------------------------------------------------------------------------------------------------
// This file was auto-generated on 2/10/2026 4:33 PM. Any changes made to it will be lost.
//------------------------------------------------------------------------------------------------------------

namespace Dyvenix.App1.Common.Data.Shared.Entities;

public partial class Practice
{
	// PK
	public Guid Id { get; set; }

	// Properties
	public string Name { get; set; } = null!;

	// Navigation Properties
	public List<Patient> Patients { get; set; } = new();

	#region PropNames

	public static class PropNames
	{
		public const string Id = "Id";
		public const string Name = "Name";
		public const string Patients = "Patients";
	}

	#endregion
}
