using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Autodesk.Forge
{
    interface IAuthenticationProvider
    {
        Task<string> GetAccessToken();
    }

    class PassiveAuthenticationProvider : IAuthenticationProvider
    {
        private string _accessToken;
        public PassiveAuthenticationProvider(string accessToken) => _accessToken = accessToken;
        public Task<string> GetAccessToken() => Task.FromResult<string>(_accessToken);
    }

    class ActiveAuthenticationProvider : IAuthenticationProvider
    {
        protected static readonly string[] Scopes = { "bucket:read", "bucket:create", "data:read", "data:write", "data:create" };
        private AuthenticationClient _authenticationClient;
        private string _accessTokenCache;
        private DateTime _accessTokenExpires;
        public ActiveAuthenticationProvider(AuthenticationClient authenticationClient) => _authenticationClient = authenticationClient;
        public async Task<string> GetAccessToken()
        {
            if (string.IsNullOrEmpty(_accessTokenCache) || DateTime.Now > _accessTokenExpires)
            {
                var authentication = await _authenticationClient.Authenticate(Scopes);
                _accessTokenCache = authentication.access_token;
                _accessTokenExpires = DateTime.Now.AddSeconds(authentication.expires_in);
            }
            return _accessTokenCache;
        }
    }

    public abstract class AuthorizedClient : BaseClient
    {
        private IAuthenticationProvider AuthenticationProvider { get; set; }

        public AuthorizedClient(string accessToken) : base()
        {
            ResetAuthentication(accessToken);
        }

        public AuthorizedClient(string clientId, string clientSecret) : base()
        {
            ResetAuthentication(clientId, clientSecret);
        }

        public void ResetAuthentication(string accessToken)
        {
            AuthenticationProvider = new PassiveAuthenticationProvider(accessToken);
        }

        public void ResetAuthentication(string clientId, string clientSecret)
        {
            AuthenticationProvider = new ActiveAuthenticationProvider(new AuthenticationClient { ClientID = clientId, ClientSecret = clientSecret });
        }

        protected new async Task<HttpResponseMessage> Get(string endpoint)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, new Uri(BaseAddress, endpoint));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await AuthenticationProvider.GetAccessToken());
            var response = await Client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            return response;
        }

        protected async Task<T> GetJson<T>(string endpoint)
        {
            var response = await Get(endpoint);
            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}