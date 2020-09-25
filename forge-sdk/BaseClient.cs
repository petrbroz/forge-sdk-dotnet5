using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Autodesk.Forge
{
    public class BaseClient
    {
        protected static readonly HttpClient Client = new HttpClient();
        public Uri BaseAddress { get; set; } = new Uri("https://developer.api.autodesk.com/");

        public async Task<HttpResponseMessage> Get(string endpoint)
        {
            return await Client.GetAsync(new Uri(BaseAddress, endpoint));
        }

        public async Task<HttpResponseMessage> Post(string endpoint, HttpContent content)
        {
            return await Client.PostAsync(new Uri(BaseAddress, endpoint), content);
        }

        public async Task<HttpResponseMessage> Put(string endpoint, HttpContent content)
        {
            return await Client.PutAsync(new Uri(BaseAddress, endpoint), content);
        }

        public async Task<HttpResponseMessage> Patch(string endpoint, HttpContent content)
        {
            return await Client.PatchAsync(new Uri(BaseAddress, endpoint), content);
        }

        public async Task<HttpResponseMessage> Delete(string endpoint)
        {
            return await Client.DeleteAsync(new Uri(BaseAddress, endpoint));
        }
    }
}