using System.Text.Json.Serialization;

namespace Dyvenix.App1.AdAgent.Api.Config
{
    public class AdAgentConfig
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public AdAgentAuthMode AuthMode { get; set; }
        public string? DcHost { get; set; }
        public int LdapPort { get; set; } = 389;
        public string? BaseDn { get; set; }
        public string? ServiceUsername { get; set; }
        public string? ServicePassword { get; set; }
    }

    public enum AdAgentAuthMode
    {
        Ldap = 0,
        Kerberos = 1
    }
}
