using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication6.ViewModels.Authentication;
using System.Security.Claims;
using WebApplication6.Infrastracture;
using WebApplication6.Models;

namespace WebApplication6.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult SignIn(AuthenticationIndexViewModel viewModel)
        {
            return View(viewModel);
        }

        [Authorize]
        public IActionResult RequireFidoRegistration()
        {
            return View();
        }

        [Authorize]
        public IActionResult RegisterFidoPubkey(string id, string publicKey)
        {
            SampleAuthentication.Users[((ClaimsIdentity)User.Identity).GetUserId()].PublicKey = publicKey;
            return RedirectToAction("Index", "Home", new { RegistrationSucceeded = true });
        }

        public async Task<IActionResult> SignOut()
        {
            var authentication = this.HttpContext.Authentication;
            await authentication.SignOutAsync(Application.AuthenticationTypeCookieAuthentication);
            return RedirectToAction("Index", "Home", new { SignOutCompleted = true });
        }

        public async Task<IActionResult> Authenticate(AuthenticateSignInInputViewModel inputModel)
        {
            var identity = inputModel.Authenticate();
            if (identity != null)
            {
                var authentication = this.HttpContext.Authentication;
                await authentication.SignInAsync(Application.AuthenticationTypeCookieAuthentication, new ClaimsPrincipal(identity));
                return RedirectToAction("Index", "Home", new { AuthenticationSucceeded = true });
            }
            else
            {
                return RedirectToAction("SignIn", new { AuthenticationFailed = true });
            }
        }

        public IActionResult SignUp(string userName, string password)
        {
            return SampleAuthentication.Register(userName, password)
                ? RedirectToAction("SignIn", new { RegistrationSucceeded = true, UserName = userName })
                : RedirectToAction("SignIn", new { RegistrationFailed = true });
        }
    }
}
