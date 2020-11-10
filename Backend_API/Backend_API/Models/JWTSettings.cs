using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_API.Models
{
    public class JWTSettings
    {
        public string Secret { get; set; }
        public string Issuer {get; set;}
        public string Audience { get;set; }
        public int ExpieryTimeInMins { get; set; }

        public const string AuthScheme = "Identity.Application" + "," + JwtBearerDefaults.AuthenticationScheme;
    }
}
