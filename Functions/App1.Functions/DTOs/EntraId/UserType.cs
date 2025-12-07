using System.Text.Json.Serialization;

namespace Dyvenix.App1.Functions.DTOs.EntraId;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum UserType
{
    Member,
    Guest
}
