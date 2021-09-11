using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;

namespace WebApplication1.WebApi
{
	[Route("api/[controller]")]
	[ApiController]
	public class SecurityController : ControllerBase
	{
		private IAntiforgery antiforgery;

		public SecurityController(IAntiforgery antiforgery)
		{
			this.antiforgery = antiforgery;
		}

        [AllowAnonymous]
        [Route("LogIn")]
        [HttpPost]
        public IActionResult LogIn([FromBody] LogInInputModel login)
        {
            //throw new Exception("ddd"); test UseExceptionHandler
            
            return BadRequest(new ErrorVM(){ status = 999, msg = "test" });
        }

        [Route("xsrfToken")]
        [HttpGet("xsrf-token")]
        public ActionResult GetXsrfToken(string test)
        {
            //throw new Exception("test"); //test UseExceptionHandler

            var tokens = antiforgery.GetAndStoreTokens(HttpContext);
            // 向客戶端發送名稱為 XSRF-TOKEN 的 Cookie ， 客戶端必須將這個 Cookie 的值
            // 以 X-XSRF-TOKEN 為名稱的 Header 再發送回服務端， 才能完成 XSRF 認證。
            Response.Cookies.Append(
                "XSRF-TOKEN",
                tokens.RequestToken,
                new CookieOptions
                {
                    HttpOnly = false,
                    Path = "/",
                    IsEssential = true,
                    SameSite = SameSiteMode.Lax
                }
            );
            return Ok();
        }
    }
}
