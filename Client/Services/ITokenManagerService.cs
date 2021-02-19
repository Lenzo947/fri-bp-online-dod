using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Client.Services
{
    public interface ITokenManagerService
    {
        Task<string> GetTokenAsync();
    }
}
