using System.Text.Json.Serialization;

namespace App1.App1.Functions.DTOs.EntraId;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserType
{
    Member,
    Guest
}
