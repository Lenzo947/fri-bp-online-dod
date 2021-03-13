using System;
using System.Collections.Generic;
using System.Text;

namespace BP_OnlineDOD.Shared.Models
{
    public class UserToken
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
