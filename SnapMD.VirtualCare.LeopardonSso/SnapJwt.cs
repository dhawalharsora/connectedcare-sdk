﻿using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace SnapMD.VirtualCare.LeopardonSso
{
    public class SnapJwt : AbstractJwt
    {
        private const string Role = "patient";
        private readonly string _issuer;
        private readonly SecurityKey _securityKey;

        public SnapJwt(string issuer, SecurityKey securityKey)
        {
            _issuer = issuer;
            _securityKey = securityKey;
        }

        protected override string Audience => @"snapmd";

        protected override string Issuer => _issuer; 

        protected override SigningCredentials SigningCredentials =>
            new SigningCredentials(_securityKey,
                "http://www.w3.org/2001/04/xmldsig-more#rsa-sha512",
                "http://www.w3.org/2001/04/xmlenc#sha512");

        protected override TokenValidationParameters TokenValidationParameters
        {
            get
            {
                var parameters = base.TokenValidationParameters;
                parameters.IssuerSigningKey = _securityKey;
                return parameters;
            }
        }

        public string Write(string name, string email)
        {
            return CreateToken(CreateClaims(name, email), false);
        }

        public string Encode(string name, string email)
        {
            return CreateToken(CreateClaims(name, email));
        }

        protected virtual List<Claim> CreateClaims(string name, string email)
        {
            return new List<Claim>
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, Role)
            };
        }
    }
}