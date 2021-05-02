using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Bolt.Comments.Domain;
using Microsoft.AspNetCore.Http;

namespace Bolt.Comments
{
    public class Authorization
    {
        private readonly SettingsService _settings;

        public Authorization(SettingsService settings)
        {
            _settings = settings;
        }

        public class ClientPrincipal
        {
            public string IdentityProvider { get; set; } = "";
            public string UserId { get; set; } = "";
            public string UserDetails { get; set; } = "";
            public IEnumerable<string> UserRoles { get; set; } = Array.Empty<string>();
        }

        public static class Roles{
            public const string Anonymous = "anonymous";
            public const string Authenticated = "authenticated";
            public static string Approve = "approve";
            public static string AddComment = "add-comment";
            public static string Admin = "admin";
            public static string ImportComment = "import-comment";
            public static string ListComments = "list-comments";
        }

        public static ClaimsPrincipal Parse(HttpRequest req, ApiKey[] apiKeys)
        {
            var principal = new ClientPrincipal();

            if (req.Headers.TryGetValue("x-ms-client-principal", out var header))
            {
                var data = header[0];
                var decoded = Convert.FromBase64String(data);
                var json = Encoding.ASCII.GetString(decoded);
                principal = JsonSerializer.Deserialize<ClientPrincipal>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else if (TryGetApiKey(req, out var apiKeyValue))
            {
                var apiKey = apiKeys.FirstOrDefault( k => string.Equals(k.Key, apiKeyValue));
                
                if( apiKey != null )
                {
                    principal.IdentityProvider="API Key";
                    principal.UserDetails=apiKey.Name;
                    principal.UserId=apiKey.UserId;
                    principal.UserRoles=apiKey.Roles;
                }
            }

            principal.UserRoles = principal.UserRoles?.Except(new string[] { "anonymous" }, StringComparer.OrdinalIgnoreCase) ?? Array.Empty<string>();

            if (!principal.UserRoles?.Any() ?? true)
            {
                return new ClaimsPrincipal();
            }

            var identity = new ClaimsIdentity(principal.IdentityProvider);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, principal.UserId));
            identity.AddClaim(new Claim(ClaimTypes.Name, principal.UserDetails));
            identity.AddClaims(principal.UserRoles.Select(r => new Claim(ClaimTypes.Role, r)));

            return new ClaimsPrincipal(identity);
        }

        public async Task<bool> IsAuthorized(HttpRequest req, params string[] allowedRoles)
        {
            var principal = Parse(req, await _settings.GetApiKeys());

            return allowedRoles?.Any(r => principal.IsInRole(r)) ?? false;
        }
        
        private static bool TryGetApiKey(HttpRequest request, out string apiKey)
        {
            if( request.Headers.TryGetValue("x-bolt-api-key", out var apiKeyHeader) )
            {
                apiKey = apiKeyHeader.FirstOrDefault() ?? "";
                return true;
            }

            if (request.Query.TryGetValue("bolt-key", out var queryValue))
            {
                apiKey = queryValue.FirstOrDefault() ?? "";
                return true;
            }

            apiKey = "";
            return false;
        }
    }
}