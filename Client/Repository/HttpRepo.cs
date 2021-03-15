using BP_OnlineDOD.Client.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Client.Repository
{
    public class HttpRepo : IHttpRepo
    {
        private readonly HttpClientWithoutToken _client;

        public HttpRepo(HttpClientWithoutToken client)
        {
            _client = client;
        }

        public async Task<string> UploadFile(MultipartFormDataContent content)
        {
            var postResult = await _client.HttpClient.PostAsync("api/upload", content);
            var postContent = await postResult.Content.ReadAsStringAsync();

            if (!postResult.IsSuccessStatusCode)
            {
                throw new ApplicationException(postContent);
            }
            else
            {
                var fileUrl = Path.Combine(_client.HttpClient.BaseAddress.ToString(), postContent);
                return fileUrl;
            }
        }
    }
}
