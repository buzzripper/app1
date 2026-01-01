namespace App1.Data.Shared.Entities;

public class ClaimType
{
	public string Type { get; set; } = string.Empty;
	public string? DataType { get; set; } // "string", "bool", "int", etc.
	public string? DisplayName { get; set; }
	public string? Description { get; set; }
}

