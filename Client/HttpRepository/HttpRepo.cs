﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Client.HttpRepository
{
    public class HttpRepo : IHttpRepo
    {
        private readonly HttpClient _client;

        public HttpRepo(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> UploadProductImage(MultipartFormDataContent content)
        {
            var postResult = await _client.PostAsync("api/upload", content);
            var postContent = await postResult.Content.ReadAsStringAsync();

            if (!postResult.IsSuccessStatusCode)
            {
                throw new ApplicationException(postContent);
            }
            else
            {
                var imgUrl = Path.Combine(_client.BaseAddress.ToString(), postContent);
                return imgUrl;
            }
        }
    }
}
