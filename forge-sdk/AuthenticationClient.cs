using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Autodesk.Forge
{
    public record Authentication(string token_type, string access_token, int expires_in);

    public class AuthenticationClient : BaseClient
    {
        public string ClientID { get; init; }
        public string ClientSecret { get; init; }

        public async Task<Authentication> Authenticate(params string[] scopes)
        {
            return await Authenticate((IEnumerable<string>)scopes);
        }

        public async Task<Authentication> Authenticate(IEnumerable<string> scopes)
        {
            var payload = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "client_id", ClientID },
                { "client_secret", ClientSecret },
                { "grant_type", "client_credentials" },
                { "scope", string.Join(" ", scopes) }
            });
            var response = await Post("authentication/v1/authenticate", payload);
            // if (!response.IsSuccessStatusCode)
            // {
            //     System.Console.WriteLine(response.RequestMessage.ToString());
            //     var error = await response.Content.ReadAsStringAsync();
            //     throw new HttpRequestException(error);
            // }
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<Authentication>();
        }
    }
}