using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BP_OnlineDOD.Client.Models;
using BP_OnlineDOD.Shared.Models;

namespace BP_OnlineDOD.Client.Services
{ 
    public interface IAccountService
    {
        Task<bool> LoginAsync(LoginModel model);
        Task<bool> LogoutAsync();
    }
}
