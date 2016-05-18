using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication6.Infrastracture;
using WebApplication6.Models;

namespace WebApplication6.ViewModels.Authentication
{
    public class AuthenticateSignInInputViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public bool UseFido { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        public string ClientData { get; set; }
        public string Signature { get; set; }
        public string AuthenticatorData { get; set; }
        public string Challenge { get; set; }

        public ClaimsIdentity Authenticate()
        {
            return SampleAuthentication.Authenticate(this.UseFido, this.UserName, this.Password, this.ClientData, this.AuthenticatorData, this.Signature, this.Challenge);
        }
    }
}
