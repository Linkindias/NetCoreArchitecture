using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using BLL.Model;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private MemberTestService _memberTest;

        public HomeController(ILogger<HomeController> logger, MemberTestService memberTestService)
        {
            _logger = logger;
            this._memberTest = memberTestService;
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
            //var account =_memberTest.GetAccount(loginModel.account);
            var account = _memberTest.TestLinqKitPredicate(new Member(0,string.Empty,0,string.Empty,0,string.Empty,null) { Account = loginModel.account });
            return RedirectToAction("Privacy", new PersonInfoVM()
            {
                oneAccount = account.Account1,
				name = account.Name, 
                email = account.Email,
                jobTitle = account.JobTitle,
            });
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
