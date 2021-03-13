using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Client.Repository
{
    public interface IHttpRepo
    {
        Task<string> UploadFile(MultipartFormDataContent content);
    }
}
