using BP_OnlineDOD.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Client.Repository
{
    public interface IAccountsRepository
    {
        Task<UserToken> Login(UserInfo userInfo);
        Task<UserToken> Register(UserInfo userInfo);

    }
}
