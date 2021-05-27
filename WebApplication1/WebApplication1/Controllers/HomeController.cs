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
            return View(new TestModel()
            {
                guid = _member.guid.ToString()
            });
        }

        public IActionResult LogIn(TestModel testModel)
        {
            var account =_member.GetAccount(testModel.account);
            return RedirectToAction("Privacy", account);
        }

        public IActionResult Privacy(AccountVM accountVm)
        {
            return View(accountVm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
