using Base;
using BLL;
using DAL.Table;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Razor.Language;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using BLL.Model;
using IActionFilter = Microsoft.AspNetCore.Mvc.Filters.IActionFilter;

namespace OrganDonationFuneralSubsidy.Filter
{
    public class OperateLogAttribute : Attribute, IActionFilter
    {
        private OperateLogService OperateLogsService;
        private MemberService memberService;
        private readonly int OperateId;

        public OperateLogAttribute(Enums.OperateEvent OperateId)
        {
            this.OperateId = (int)OperateId;
        }

        /// <summary>
        /// TODO aysnc
        /// </summary>
        /// <param name="actionContext"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="continuation"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
            HttpResponseMessage response = await continuation();
            return response;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            ControllerActionDescriptor controllerDescriptor = ((ControllerActionDescriptor)context.ActionDescriptor);

            var httpRequestMessage = context.HttpContext.RequestServices;
            var httpContext = context.HttpContext;
            this.OperateLogsService = httpRequestMessage.GetService(typeof(OperateLogService)) as OperateLogService;
            this.memberService = httpRequestMessage.GetService(typeof(MemberService)) as MemberService;

            string PageCode = controllerDescriptor.ControllerName;
            string Parameter = string.Empty;
            string HttpMethod = controllerDescriptor.ActionName;
            Member member = null;

            switch (httpContext.Request.Method)
            {
                case "POST":
                    var httpObject = httpContext.Request.Form;

                    if (httpObject.Count > 0)
                    {
                        if (PageCode != "Home" && HttpMethod != "LogIn") //登入頁面
                            member = Common.Get<Member>(httpContext.Session, "member");

                        JObject job = new JObject();
                        foreach (var key in httpObject.Keys)
                        {
                            job.Add(key, (string)httpObject[key]);
                        }
                        Parameter = JsonConvert.SerializeObject(job, Formatting.None);
                    }
                    this.OperateLogsService.CreateOperateLog(new UserEventLog()
                    {
                        Machine = Environment.MachineName,
                        IP = httpContext.Connection.RemoteIpAddress.ToString(),
                        PageCode = PageCode,
                        Method = HttpMethod,
                        CreateDate = DateTime.Now,
                        CreateId = member == null ? 0 : member.Id,
                        Parameter = Parameter
                    });
                    break;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            ControllerActionDescriptor controllerDescriptor = ((ControllerActionDescriptor)context.ActionDescriptor);

            var httpRequestMessage = context.HttpContext.RequestServices;
            var httpContext = context.HttpContext;
            this.OperateLogsService = httpRequestMessage.GetService(typeof(OperateLogService)) as OperateLogService;
            this.memberService = httpRequestMessage.GetService(typeof(MemberService)) as MemberService;

            string PageCode = controllerDescriptor.ControllerName;
            string StrLog = string.Empty;
            string HttpMethod = controllerDescriptor.ActionName;
            Member member = null;

            switch (httpContext.Request.Method)
            {
                case "GET":

                    if (PageCode == "Home" && HttpMethod == "LogOut") //登出頁面
                        member = Common.Get<Member>(httpContext.Session, "member");

                    var httpObject = httpContext.Request.QueryString;
                    if (httpObject.HasValue) StrLog = httpObject.Value;

                    break;
            }

            if (PageCode == "Home" && HttpMethod == "LogOut")
                this.OperateLogsService.CreateOperateLog(new UserEventLog()
                {
                    Machine = Environment.MachineName,
                    IP = httpContext.Connection.RemoteIpAddress.ToString(),
                    PageCode = PageCode,
                    Method = HttpMethod,
                    CreateDate = DateTime.Now,
                    CreateId = member == null ? 0 : member.Id,
                });
        }
    }
}
