using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApplication6.ViewModels.Home
{
    public class HomeIndexViewModel
    {
        public bool AuthenticationSucceeded { get; set; }
        public bool RegistrationSucceeded { get; set; }
        public bool SignOutCompleted { get; set; }
    }
}