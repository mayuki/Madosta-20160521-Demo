using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication6.ViewModels.Authentication
{
    public class AuthenticationIndexViewModel
    {
        public bool AuthenticationFailed { get; set; }
        public bool RegistrationSucceeded { get; set; }
        public bool RegistrationFailed { get; set; }
        public string UserName { get; set; }
    }
}