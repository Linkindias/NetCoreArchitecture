using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BLL;
using DAL.TableModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebApplication1.Filter
{
    public class ExceptionAttribute : Attribute, IExceptionFilter
    {
        private ExceptionLogService exceptionLogService;

        /// <summary>
        /// 發生異常進入
        /// </summary>
        /// <param name="context"></param>
        public async void OnException(ExceptionContext context)
        {
            if (!context.ExceptionHandled)//如果異常沒有處理
            {
                this.exceptionLogService = context.HttpContext.RequestServices.GetService(typeof(ExceptionLogService)) as ExceptionLogService;

                string Method = context.HttpContext.Request.Method;
                string parameters = string.Empty;

                if (Method == "POST" || Method == "PUT" || Method == "DELETE")
                    parameters = string.Join(",",context.HttpContext.Request.Form
                                                                                    .Select(x => $"{x.Key}={x.Value.ToString()}")
                                                                                    .ToList());

                else
                    parameters = context.HttpContext.Request.QueryString.Value;

                string ExceptionMsg = string.Empty;

                if (context.Exception.GetBaseException() is ValidationException)
                    ExceptionMsg = string.Join(",", ((ValidationException)context.Exception.GetBaseException()).ValidationResult.ErrorMessage);

                else if (context.Exception.GetBaseException() is Exception)
                {
                    Exception exception = ((Exception)context.Exception.GetBaseException());
                    if (exception.Data.Count > 0)
                    {
                        List<string> fields = new List<string>();
                        PropertyInfo[] properties = exception.GetType().GetProperties();
                        foreach (PropertyInfo property in properties)
                            fields.Add($"{property.Name} : {property.GetValue(exception, null) ?? string.Empty}");

                        ExceptionMsg = string.Join("\n", fields.ToArray());
                    }
                    else
                        ExceptionMsg = exception.Message;
                }
               
                this.exceptionLogService.CreateExceptionLog(new ExceptionLog()
                {
                    UPD_DTM = DateTime.Now,
                    UPD_OPER = context.HttpContext.User.Identity.Name,
                    OPR_IP = context.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ROUTE = context.HttpContext.Request.GetDisplayUrl(),
                    METHOD = Method,
                    PARAMETER = parameters,
                    EXCEPTION = ExceptionMsg,
                    OS = "WebCore"
                });

                context.ExceptionHandled = true;//異常已處理
            }
        }
    }
}
