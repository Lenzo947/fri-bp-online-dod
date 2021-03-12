using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using BP_OnlineDOD.Server.Data.Entities;
using BP_OnlineDOD.Server.Shared;
using BP_OnlineDOD.Shared.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace BP_OnlineDOD.Server.Logic
{
    public class AccountLogic : IAccountLogic
    {
        private readonly TokenSettings _tokenSettings;
        public AccountLogic(IOptions<TokenSettings> tokenSettings)
        {
            _tokenSettings = tokenSettings.Value;
        }

        private List<User> Users = new List<User>
        {
            new User{
                Id = 1,
                FirstName = "Admin",
                LastName = "Konto",
                Email = Environment.GetEnvironmentVariable("WEB_LOGIN") ?? "admin",
                Password= Environment.GetEnvironmentVariable("WEB_PASSWORD") ?? "admin",
                PhoneNumber="8888899999"
            }
        };


        public TokenModel GetAuthenticationToken(LoginModel loginModel)
        {
			User currentUser = Users.Where(_ => _.Email.ToLower() == loginModel.Email.ToLower() &&
			_.Password == loginModel.Password).FirstOrDefault();

			if (currentUser != null)
			{
				var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));
				var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

				var userClaims = new List<Claim>{
					new Claim("email", currentUser.Email),
					new Claim("phone", currentUser.PhoneNumber),
				};

				return GetTokens(currentUser, userClaims);
			}

			return null;
		}

        private string GetRefreshToken()
        {
			var key = new byte[32];
			using (var refreshTokenGenerator = RandomNumberGenerator.Create())
			{
				refreshTokenGenerator.GetBytes(key);
				return Convert.ToBase64String(key);
			}
		}

		private TokenModel GetTokens(
			User currentUser,
			List<Claim> userClaims
		)
		{
			var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));
			var credentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			var newJwtToken = new JwtSecurityToken(
					issuer: _tokenSettings.Issuer,
					audience: _tokenSettings.Audience,
					expires: DateTime.Now.AddDays(7),
					signingCredentials: credentials,
					claims: userClaims
			);

			string token = new JwtSecurityTokenHandler().WriteToken(newJwtToken);
			string refreshToken = GetRefreshToken();

			currentUser.RefreshToken = refreshToken;

			return new TokenModel
			{
				Token = token,
				RefreshToken = refreshToken
			};
		}

		public TokenModel ActivateTokenUsingRefreshToke(TokenModel tokenModel)
		{
            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(tokenModel.Token,
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _tokenSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _tokenSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key)),
                ValidateLifetime = true
            }, out SecurityToken validatedToken);


            var jwtToken = validatedToken as JwtSecurityToken;

            if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return null;
            }

            var email = claimsPrincipal.Claims.Where(_ => _.Type == ClaimTypes.Email).Select(_ => _.Value).FirstOrDefault();
            if (string.IsNullOrEmpty(email))
            {
                return null;
            }

            var currentUser = Users.Where(_ => _.Email == email && _.RefreshToken == tokenModel.RefreshToken).FirstOrDefault();
            if (currentUser == null)
            {
                return null;
            }

            return GetTokens(currentUser, jwtToken.Claims.ToList());
        }
	}
}