using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.ViewModels.Home;

namespace WebApplication6.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(HomeIndexViewModel viewModel)
        {
            return View(viewModel);
        }

        public ActionResult Notification()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Beacon()
        {
            return View();
        }
    }
}
