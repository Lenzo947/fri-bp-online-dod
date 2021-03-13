using System.Net.Http;

namespace BP_OnlineDOD.Client.Helpers
{
    public class HttpClientWithToken
    {
        public HttpClientWithToken(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public HttpClient HttpClient { get; }
    }
}
