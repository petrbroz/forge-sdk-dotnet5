using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Autodesk.Forge
{
    public class DataManagementClient : AuthorizedClient
    {
        public record Bucket(string bucketKey, long createdDate, string policyKey);
        public record Object(string bucketKey, string objectKey, string objectId, string sha1, long size, string location);
        protected record PaginatedResponse<T>(T[] items, string next);

        static readonly string ListBucketsEndpoint = "oss/v2/buckets";
        static readonly string ListObjectsEndpoint = "oss/v2/buckets/{0}/objects";

        public DataManagementClient(string accessToken) : base(accessToken)
        {
        }

        public DataManagementClient(string clientId, string clientSecret) : base(clientId, clientSecret)
        {
        }

        public async IAsyncEnumerable<Bucket> EnumerateBuckets()
        {
            var uri = ListBucketsEndpoint;
            do
            {
                var response = await GetJson<PaginatedResponse<Bucket>>(uri);
                foreach (var bucket in response.items)
                {
                    yield return bucket;
                }
                uri = response.next;
            }
            while (!string.IsNullOrEmpty(uri));
        }

        // public async Task<List<Bucket>> ListBuckets()
        // {
        //     var buckets = new List<Bucket>();
        //     var uri = ListBucketsEndpoint;
        //     while (!string.IsNullOrEmpty(uri))
        //     {
        //         var response = await GetFromJson<ListBucketsResponse>(uri);
        //         buckets.AddRange(response.items);
        //         uri = response.next;
        //     }
        //     return buckets;
        // }

        public async Task<List<Bucket>> ListBuckets()
        {
            return await EnumerateBuckets().ToListAsync();
        }

        public async IAsyncEnumerable<Object> EnumerateObjects(string bucketKey)
        {
            var uri = string.Format(ListObjectsEndpoint, HttpUtility.UrlEncode(bucketKey));
            while (!string.IsNullOrEmpty(uri))
            {
                var response = await GetJson<PaginatedResponse<Object>>(uri);
                foreach (var obj in response.items)
                {
                    yield return obj;
                }
                uri = response.next;
            }
        }

        // public async Task<List<Object>> ListObjects(string bucketKey)
        // {
        //     var objects = new List<Object>();
        //     var uri = string.Format(ListObjectsEndpoint, HttpUtility.UrlEncode(bucketKey));
        //     while (!string.IsNullOrEmpty(uri))
        //     {
        //         var response = await GetFromJson<ListObjectsResponse>(uri);
        //         objects.AddRange(response.items);
        //         uri = response.next;
        //     }
        //     return objects;
        // }

        public async Task<List<Object>> ListObjects(string bucketKey)
        {
            return await EnumerateObjects(bucketKey).ToListAsync();
        }

        // public async Task<List<string>> LargeObjects(string bucketKey)
        // {
        //     var query =
        //         from obj in EnumerateObjects(bucketKey)
        //         where obj.size > 1000
        //         select obj.objectKey;
        //     return await query.ToListAsync();
        // }
    }
}