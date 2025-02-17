// Decompiled with JetBrains decompiler
// Type: IBHFL.Controllers.SSOController
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using IBHFL.Models;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Configuration;

namespace IBHFL.Controllers
{
    [SessionExpireFilter]
    public class SSOController : Controller
  {
    public ActionResult Index() => (ActionResult) this.View();

    public ActionResult LandingAuth(string UID)
    {
      LoginResponseAuth loginResponseAuth = JsonConvert.DeserializeObject<LoginResponseAuth>(new UtilityModel().getApiResponseString(APIString.API + "SSOLanding?USERID=" + HttpUtility.UrlEncode(UID, Encoding.UTF8)));
      System.Web.HttpContext.Current.Session["UserSession"] = (object) null;
            System.Web.HttpContext.Current.Session["UserSession"] = (object)new UserSession()
            {
                Username = loginResponseAuth.fullname,
                Roleid = "0",
                ID_USER = loginResponseAuth.UserID.ToString(),
                id_ORGANIZATION = Convert.ToInt32(loginResponseAuth.ORGID),
                logo_path = loginResponseAuth.LogoPath,
                USERID = loginResponseAuth.UserName,
                REURL = "OPP",
                APIUrl = ConfigurationManager.AppSettings["api_raw"].ToString(),
                ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString()
            };
      this.TempData["loginnotification"] = (object) "yes";
      return (ActionResult) this.View();
    }

    public bool ClearSession()
    {
      UserSession content = (UserSession) this.HttpContext.Session.Contents["UserSession"];
      if (content.REURL != null)
      {
        string reurl = content.REURL;
        this.Session.Abandon();
        this.Session.Clear();
        this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
        HttpCachePolicyBase cache = this.Response.Cache;
        DateTime dateTime1 = DateTime.UtcNow;
        DateTime date = dateTime1.AddHours(-1.0);
        cache.SetExpires(date);
        this.Response.Cache.SetNoStore();
        this.Response.Cookies.Clear();
        HttpCookie cookie1 = new HttpCookie("mycookies");
        this.Response.Cookies["mycookies"]["email"] = "";
        this.Response.Cookies["mycookies"]["pass"] = "";
        HttpCookie cookie2 = this.Response.Cookies["mycookies"];
        dateTime1 = DateTime.Now;
        DateTime dateTime2 = dateTime1.AddDays(1.0);
        cookie2.Expires = dateTime2;
        this.Response.Cookies.Add(cookie1);
        System.Web.HttpContext.Current.Session["UserSession"] = (object) null;
        return true;
      }
      this.Session.Abandon();
      this.Session.Clear();
      this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
      HttpCachePolicyBase cache1 = this.Response.Cache;
      DateTime dateTime3 = DateTime.UtcNow;
      DateTime date1 = dateTime3.AddHours(-1.0);
      cache1.SetExpires(date1);
      this.Response.Cache.SetNoStore();
      this.Response.Cookies.Clear();
      HttpCookie cookie3 = new HttpCookie("mycookies");
      this.Response.Cookies["mycookies"]["email"] = "";
      this.Response.Cookies["mycookies"]["pass"] = "";
      HttpCookie cookie4 = this.Response.Cookies["mycookies"];
      dateTime3 = DateTime.Now;
      DateTime dateTime4 = dateTime3.AddDays(1.0);
      cookie4.Expires = dateTime4;
      this.Response.Cookies.Add(cookie3);
      return true;
    }
  }
}
