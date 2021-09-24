using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MemberService _member;

        public HomeController(ILogger<HomeController> logger, MemberService memberService)
        {
            _logger = logger;
            this._member = memberService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogIn(LogInInputModel loginModel)
        {
	        if (!ModelState.IsValid)
	        {
		        return View("Index", loginModel);
	        }
            var account =_member.GetAccount(loginModel.account);
            return RedirectToAction("Privacy", account);
        }

        public IActionResult Privacy(PersonInfoVM accountVm)
        {
            return View(accountVm);
        }

        public IActionResult Test()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
