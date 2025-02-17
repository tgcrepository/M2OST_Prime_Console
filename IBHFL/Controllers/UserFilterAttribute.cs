// Decompiled with JetBrains decompiler
// Type: IBHFL.Controllers.UserFilterAttribute
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using IBHFL.Models;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web;
using System;

namespace IBHFL.Controllers
{
    public class UserFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if ((UserSession)filterContext.HttpContext.Session.Contents["UserSession"] == null)
                filterContext.Result = (ActionResult)new RedirectToRouteResult(new RouteValueDictionary()
        {
          {
            "Controller",
            (object) "Account"
          },
          {
            "Action",
            (object) "Login"
          }
        });
            else
                base.OnActionExecuting(filterContext);
        }
    }

    public class SessionExpireFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //////HttpSessionStateBase session = filterContext.HttpContext.Session;

            //////if (session != null && session.IsNewSession)
            //////{
            //////    // Check if it's a new session (session has expired or started)
            //////    string sessionCookie = filterContext.HttpContext.Request.Headers["Cookie"];

            //////    if ((sessionCookie != null) && (sessionCookie.IndexOf("ASP.NET_SessionId", StringComparison.OrdinalIgnoreCase) >= 0))
            //////    {
            //////        // Session has expired
            //////        filterContext.Result = new RedirectResult("~/Account/Login"); // Replace with your desired redirection URL

            //////        return;
            //////    }
            //////}

            if ((UserSession)filterContext.HttpContext.Session.Contents["UserSession"] == null)
            {
                filterContext.Result = new RedirectResult("~/Account/RedirectToLoginPage");
                return;
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
