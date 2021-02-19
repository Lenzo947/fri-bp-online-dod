using BP_OnlineDOD.Server.Logic;
using BP_OnlineDOD.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BP_OnlineDOD.Server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountLogic _accountLogic;

        public AccountController(IAccountLogic accountLogic)
        {
            _accountLogic = accountLogic;
        }

        [HttpPost]
        [Route("login-token")]
        public IActionResult GetLoginToken(LoginModel model)
        {
            var tokenModel = _accountLogic.GetAuthenticationToken(model);

            if (tokenModel == null)
            {
                return NotFound();
            }
            return Ok(tokenModel);
        }

        [HttpPost]
        [Route("activate-token-by-refreshtoken")]
        public IActionResult ActivateAccessTokenByRefresh(TokenModel refreshToken)
        {
            var resultTokenModel = _accountLogic.ActivateTokenUsingRefreshToke(refreshToken);
            if (refreshToken == null)
            {
                return NotFound();
            }
            return Ok(resultTokenModel);
        }
    }
}
