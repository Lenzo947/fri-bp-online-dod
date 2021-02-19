using BP_OnlineDOD.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Server.Logic
{
    public interface IAccountLogic
    {
        TokenModel GetAuthenticationToken(LoginModel loginModel);
        TokenModel ActivateTokenUsingRefreshToke(TokenModel tokenModel);
    }
}
