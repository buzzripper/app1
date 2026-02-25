using Dyvenix.App1.AdAgent.Shared.Contracts.v1;
using Dyvenix.App1.AdAgent.Shared.DTOs;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Dyvenix.App1.AdAgent.Api.Services.v1;

public class AdService() : IAdService
{
    private string _dcHost = "adcorp1dc";
    private const int LdapPort = 389;
    private bool validated = false;
    private string _baseDn = "DC=adcorp1,DC=local";
    private string _serviceUsername = "AdAgentService";
    private string _servicePassword = "Toto;55";

    public async Task<AdAuthResult> AuthenticateUser(string userUpnOrDomainUser, string password, CancellationToken ct = default)
    {
        // First check the credentials
        try
        {
            if (!await ValidateUserCredentials(userUpnOrDomainUser, password, ct))
                return new AdAuthResult(AdAuthStatus.InvalidCredentials, "Invalid username or password");
        }
        catch (LdapException ex)
        {
            return MapLdapException(ex);
        }
        catch (Exception ex)
        {
            return new AdAuthResult(AdAuthStatus.UnknownError, ex.Message);
        }

        // Login succeeded, now get the unique identifier for the user - objectGuid
        try
        {
            var serviceCredentials = new NetworkCredential(_serviceUsername, _servicePassword);

            var objectGuid = await GetUserObjectGuidAsync(_baseDn, _serviceUsername, serviceCredentials);
            if (objectGuid == null)
                return new AdAuthResult(AdAuthStatus.UserNotFound, "User not found");

            // Success!
            return new AdAuthResult(AdAuthStatus.Success, "Success", objectGuid);
        }
        catch (LdapException ex)
        {
            return MapLdapException(ex);
        }
        catch (Exception ex)
        {
            return new AdAuthResult(AdAuthStatus.UnknownError, $"Credentials validated successfully, but error attempting to find user in directory: {ex.Message}");
        }
    }

    private async Task<bool> ValidateUserCredentials(string userUpnOrDomainUser, string password, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(userUpnOrDomainUser))
            throw new ArgumentException("Email/username required", nameof(userUpnOrDomainUser));

        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password required", nameof(password));

        return await Task.Run(() =>
        {
            var id = new LdapDirectoryIdentifier(_dcHost, LdapPort);
            using var conn = new LdapConnection(id) { AuthType = AuthType.Negotiate };
            conn.SessionOptions.ProtocolVersion = 3;

            conn.Bind(new NetworkCredential(userUpnOrDomainUser, password));
            return true;
        }, ct);
    }

    private AdAuthResult MapLdapException(LdapException ex)
    {
        // Most AD failures return LDAP error 49 with sub-error codes in the message.
        if (ex.ErrorCode == 49)
        {
            if (ex.ServerErrorMessage?.Contains("775") == true)
                return new AdAuthResult(AdAuthStatus.AccountLocked, "Account locked");

            if (ex.ServerErrorMessage?.Contains("532") == true)
                return new AdAuthResult(AdAuthStatus.PasswordExpired, "Password expired");

            if (ex.ServerErrorMessage?.Contains("525") == true)
                return new AdAuthResult(AdAuthStatus.UserNotFound, "User not found");

            return new AdAuthResult(AdAuthStatus.InvalidCredentials, "Invalid username or password");
        }

        if (ex.ErrorCode == 81) // LDAP_SERVER_DOWN
            return new AdAuthResult(AdAuthStatus.DomainUnavailable, "Domain controller unavailable");

        return new AdAuthResult(AdAuthStatus.UnknownError, ex.Message);
    }

    public async Task<Guid?> GetUserObjectGuidAsync(
        string baseDn,                 // e.g. "DC=ahs,DC=local"
        string samAccountName,          // e.g. "jdoe"
        NetworkCredential serviceCred,  // e.g. new("svc-ldap-reader","pwd","AHS")
        CancellationToken ct = default)
    {
        return await Task.Run(() =>
        {
            var id = new LdapDirectoryIdentifier(_dcHost, LdapPort);
            using var conn = new LdapConnection(id) { AuthType = AuthType.Negotiate };
            conn.SessionOptions.ProtocolVersion = 3;

            conn.Bind(serviceCred);

            var filter = $"(sAMAccountName={EscapeLdapFilterValue(samAccountName)})";
            var req = new SearchRequest(baseDn, filter, SearchScope.Subtree, "objectGUID");

            var resp = (SearchResponse)conn.SendRequest(req);

            if (resp.Entries.Count == 0) return (Guid?)null;

            var bytes = (byte[])resp.Entries[0].Attributes["objectGUID"][0];
            return new Guid(bytes);
        }, ct);
    }

    private static string EscapeLdapFilterValue(string value)
        => value.Replace(@"\", @"\5c")
                .Replace("*", @"\2a")
                .Replace("(", @"\28")
                .Replace(")", @"\29")
                .Replace("\0", @"\00");
}

