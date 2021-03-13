using BP_OnlineDOD.Client.Helpers;
using BP_OnlineDOD.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Client.Repository
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly IHttpService _httpService;

        public AccountsRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<UserToken> Register(UserInfo userInfo)
        {
            var httpResponse = await _httpService.Post<UserInfo, UserToken>("api/accounts/create", userInfo);

            if (!httpResponse.Success)
            {
                throw new ApplicationException(await httpResponse.GetBody());
            }

            return httpResponse.Response;
        }

        public async Task<UserToken> Login(UserInfo userInfo)
        {
            var httpResponse = await _httpService.Post<UserInfo, UserToken>("api/accounts/login", userInfo);

            if (!httpResponse.Success)
            {
                throw new ApplicationException(await httpResponse.GetBody());
            }

            return httpResponse.Response;
        }
    }
}
