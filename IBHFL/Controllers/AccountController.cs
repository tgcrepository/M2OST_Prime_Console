// Decompiled with JetBrains decompiler
// Type: IBHFL.Controllers.AccountController
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using IBHFL.Models;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace IBHFL.Controllers
{

    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            var request = Request;
            var scheme = request.Url.Scheme;
            var server = request.Headers["Host"] ?? string.Format("{0}:{1}", request.Url.Host, request.Url.Port);
            var host = string.Format("{0}://{1}", scheme, server);
            var root = host + VirtualPathUtility.ToAbsolute("~");

            System.Web.HttpContext.Current.Session["rooturl"] = root;

            if ((UserSession)this.HttpContext.Session.Contents["UserSession"] != null)
            {
                return (ActionResult)this.RedirectToAction("Index", "Dashboard");
            }
            string str = "";
            if (this.TempData["error"] != null)
                str = (string)this.TempData["error"];
            this.ViewData["error"] = (object)str;
            return (ActionResult)this.View();
        }
        public ActionResult LoginUrl()
        {

            var data = "";
            if (RouteData.Values["name"].ToString() != null)
            {
                data = RouteData.Values["name"].ToString();
            }

            var _design = getUrl(data);


            var request = Request;
            var scheme = request.Url.Scheme;
            var server = request.Headers["Host"] ?? string.Format("{0}:{1}", request.Url.Host, request.Url.Port);
            var host = string.Format("{0}://{1}", scheme, server);
            var root = host + VirtualPathUtility.ToAbsolute("~");

            var url = request.Url.AbsoluteUri;
            
            System.Web.HttpContext.Current.Session["urlPath"] = url;
            System.Web.HttpContext.Current.Session["rooturl"] = root;

            if (_design == null)
            {
                return Redirect(root);
            }

            //System.Web.HttpContext.Current.Session["UserSession"] = (object)new UserSession()
            //{
            //    dynamic_url = url
            //};

            if ((UserSession)this.HttpContext.Session.Contents["UserSession"] != null)
            {
                return (ActionResult)this.RedirectToAction("Index", "Dashboard");
            }

            return View(_design);
        }
        public ActionResult NotFound()
        {

            var url = Request.Url.ToString();
            /*   var scheme1 = HttpContext.Request.Url.Host;

              var request = Request;
              var scheme = request.Url.Scheme;
              var server = request.Headers["Host"] ?? string.Format("{0}:{1}", request.Url.Host, request.Url.Port);
              var host = string.Format("{0}://{1}", scheme, server);
              var root = host + VirtualPathUtility.ToAbsolute("~");*/

            //  var data = RouteData.Values["name"].ToString();

            //return Redirect("~/"+url);
            //return RedirectToAction("Login", "Home", new { argname = argvalue });
            return RedirectToAction("Login", "Home", new { url1 = url });
        }

        [SessionExpireFilter]
        public ActionResult RedirectToGamificationDashboard()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            string EmployeeID = "";
            if (content != null)
            {
                EmployeeID = content.EMPLOYEEID;
            }
            return Redirect("https://coroebus.in/b2b-web/#/account/login?userid=" + EmployeeID);
        }

        public ActionResult RedirectToLoginPage()
        {
            this.Session.Abandon();
            this.Session.Clear();
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1.0));
            this.Response.Cache.SetNoStore();
            this.Response.Cookies.Clear();
            HttpCookie cookie2 = new HttpCookie("mycookies");
            this.Response.Cookies["mycookies"]["email"] = "";
            this.Response.Cookies["mycookies"]["pass"] = "";
            this.Response.Cookies["mycookies"].Expires = DateTime.Now.AddDays(1.0);
            this.Response.Cookies.Add(cookie2);

            //////return Redirect(ConfigurationManager.AppSettings["ApplicationUrl"].ToString());

            return (ActionResult)this.View();
        }

        [SessionExpireFilter]
        public ActionResult RedirectToGamifiedAssessment()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            string UserID = "", ID_User = "", Name = ""; int OrgId = 0;
            if (content != null)
            {
                UserID = content.USERID;
                OrgId = content.id_ORGANIZATION;
                ID_User = content.ID_USER;
            }

            tbl_profile tblProfile = new UtilityModel().getUserProfileDetails(ID_User.ToString());

            if (tblProfile != null)
            {
                Name = tblProfile.FIRSTNAME;
            }
            return Redirect("https://www.playtolearn.in/Word-Bandit/?Orgid=" + OrgId + "&UID=" + UserID + "&name=" + Name);
        }

        public ActionResult LoginCheckFromSSO(string USERID)
        {
            string encodedString = Uri.EscapeDataString(USERID);
            string UserID = this.Decrypt(USERID);

            tbl_user tblUser = new UtilityModel().getUserDetails(UserID);
            string REURL = "", EmployeeID = "";
            LoginResponseAuth loginResponseAuth = new LoginResponseAuth();
            if (tblUser != null)
            {
                tbl_organization tblOrganization = new UtilityModel().getOrganizationDetails(tblUser.ID_ORGANIZATION);

                loginResponseAuth.ResponseCode = "SUCCESS";
                loginResponseAuth.ResponseAction = 0;
                loginResponseAuth.ResponseMessage = "User successfully registered";
                loginResponseAuth.UserID = Convert.ToInt32(tblUser.ID_USER);
                loginResponseAuth.UserName = tblUser.USERID;
                loginResponseAuth.EMPLOYEEID = tblUser.EMPLOYEEID;
                int idOrganization = tblOrganization.ID_ORGANIZATION;
                loginResponseAuth.ROLEID = "1";
                loginResponseAuth.ORGID = idOrganization.ToString();
                loginResponseAuth.LogoPath = new UtilityModel().getOrgLogo(idOrganization);
                loginResponseAuth.BannerPath = new UtilityModel().getOrgBanner(idOrganization);
                loginResponseAuth.ORGEMAIL = tblOrganization.DEFAULT_EMAIL;
                tbl_profile tblProfile = new UtilityModel().getUserProfileDetails(tblUser.ID_USER.ToString());
                loginResponseAuth.fullname = tblProfile.FIRSTNAME + " " + tblProfile.LASTNAME;

                System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
                System.Web.HttpContext.Current.Session["UserSession"] = (object)new UserSession()
                {
                    Username = loginResponseAuth.fullname,
                    Roleid = "0",
                    ID_USER = loginResponseAuth.UserID.ToString(),
                    id_ORGANIZATION = Convert.ToInt32(loginResponseAuth.ORGID),
                    logo_path = loginResponseAuth.LogoPath,
                    USERID = loginResponseAuth.UserName,
                    IsSSO = true,
                    EMPLOYEEID = tblUser.EMPLOYEEID,
                    APIUrl = ConfigurationManager.AppSettings["api_raw"].ToString()
                };
                this.TempData["loginnotification"] = (object)"yes";

                return (ActionResult)this.RedirectToAction("Index", "Dashboard");
            }
            else
            {
                return (ActionResult)this.RedirectToAction("Thirdpartpage", "Account");
            }
        }

        //  public ActionResult LoginCheck()
        //  {
        //      string str1 = this.Request.Form["user_name"];
        //      string str2 = this.Request.Form["password"];

        //      if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
        //      {
        //          this.TempData["error"] = "User ID and Password required";

        //          if (System.Web.HttpContext.Current.Session["urlPath"] != null)
        //          {
        //              var _path = System.Web.HttpContext.Current.Session["urlPath"];

        //              if (!string.IsNullOrEmpty(_path.ToString()))
        //              {
        //                  return Redirect(_path.ToString());
        //              }
        //          }

        //          return (ActionResult)this.RedirectToAction("Login", "Account");
        //      }

        //      LoginResponseAuth loginResponseAuth = JsonConvert.DeserializeObject<LoginResponseAuth>(new UtilityModel().getApiPost(APIString.API + "B2BAuthentication", new NameValueCollection()
        //{
        //  {
        //    "IMEI",
        //    ""
        //  },
        //  {
        //    "USERID",
        //    str1
        //  },
        //  {
        //    "PASSWORD",
        //    str2
        //  },
        //  {
        //    "OS",
        //    ""
        //  },
        //  {
        //    "Network",
        //    ""
        //  },
        //  {
        //    "OSVersion",
        //    ""
        //  },
        //  {
        //    "Details",
        //    ""
        //  },
        //  {
        //    "LogoPath",
        //    ""
        //  }
        //}));
        //      if (loginResponseAuth.ResponseCode == "FAILURE")
        //      {
        //          string _error = loginResponseAuth.ResponseMessage;
        //          if (_error == "Invalid Username and Password...")
        //          {
        //              _error = "Invalid Username or Password...";
        //          }
        //          this.TempData["error"] = _error;

        //          if (System.Web.HttpContext.Current.Session["urlPath"] != null)
        //          {
        //              var _path = System.Web.HttpContext.Current.Session["urlPath"];

        //              if (!string.IsNullOrEmpty(_path.ToString()))
        //              {
        //                  System.Web.HttpContext.Current.Session["urlPath"] = null;
        //                  System.Web.HttpContext.Current.Session["error"] = TempData["error"];
        //                  return Redirect(_path.ToString());
        //              }
        //          }

        //          ////return (ActionResult)this.RedirectToAction("RedirectToLoginPage", "Account");
        //          return (ActionResult)this.RedirectToAction("Login", "Account");
        //      }
        //      if (!(loginResponseAuth.ResponseCode == "SUCCESS"))
        //          return (ActionResult)this.View();
        //      System.Web.HttpContext.Current.Session["UserSession"] = (object)null;

        //      tbl_user tblUser = new UtilityModel().getUserDetails(str1);
        //      tbl_user tblUser2 = new UtilityModel().getRMNames(tblUser.USERID,tblUser.ID_ORGANIZATION);
        //      string EmployeeID = "";

        //      if (tblUser != null)
        //      {
        //          EmployeeID = tblUser.EMPLOYEEID;
        //      }

        //      System.Web.HttpContext.Current.Session["UserSession"] = (object)new UserSession()
        //      {
        //          Username = loginResponseAuth.fullname,
        //          Roleid = "0",
        //          Role = tblUser2.Role,
        //          ID_USER = loginResponseAuth.UserID.ToString(),
        //          id_ORGANIZATION = Convert.ToInt32(loginResponseAuth.ORGID),
        //          DESIGNATION = tblUser.user_designation,
        //          GENDER = tblUser.GENDER,
        //          OFFICE_ADDRESS = tblUser.OFFICE_ADDRESS,
        //          UserFunction = tblUser.user_function,
        //          UserGrade = tblUser.user_grade,
        //          logo_path = loginResponseAuth.LogoPath,
        //          USERID = str1,
        //          EMPLOYEEID = EmployeeID,
        //          IsSSO = false,
        //          SM = tblUser2.SM,
        //          DM = tblUser2.DM,
        //          RM = tblUser2.RM,
        //          APIUrl = ConfigurationManager.AppSettings["api_raw"].ToString(),
        //          ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString()
        //      };
        //      this.TempData["loginnotification"] = (object)"yes";
        //      return (ActionResult)this.RedirectToAction("Index", "Dashboard");
        //  }

        // sujit change on 02/02/2024 optimize login
        public async Task<ActionResult> LoginCheck()
        {
            try
            {
                // Fetch username and password from form
                string username = this.Request.Form["user_name"];
                string password = this.Request.Form["password"];

                // Validate input
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    this.TempData["error"] = "User ID and Password are required.";
                    HandleSessionRedirect();
                    return RedirectToAction("Login", "Account");
                }

                // Initialize API response
                LoginResponseAuth loginResponseAuth = null;

                try
                {
                    // Call authentication API
                    loginResponseAuth = await System.Threading.Tasks.Task.Run(() =>
                        JsonConvert.DeserializeObject<LoginResponseAuth>(
                            new UtilityModel().GetApiPostAsync(APIString.API + "B2BAuthentication", new NameValueCollection
                            {
                        { "IMEI", "" },
                        { "USERID", username },
                        { "PASSWORD", password },
                        { "OS", "" },
                        { "Network", "" },
                        { "OSVersion", "" },
                        { "Details", "" },
                        { "LogoPath", "" }
                            }).Result)
                    );
                }
                catch (Exception apiEx)
                {
                    // Log API call failure
                    Log.Error(apiEx, "API call failed during authentication.");
                    this.TempData["error"] = "Unable to authenticate. Please try again later.";
                    return RedirectToAction("Login", "Account");
                }

                // Validate API response
                if (loginResponseAuth == null || string.IsNullOrEmpty(loginResponseAuth.ResponseCode))
                {
                    HandleLoginFailure("No response received from API.");
                    return RedirectToAction("Login", "Account");
                }

                // Handle failed login attempt
                if (loginResponseAuth.ResponseCode == "FAILURE")
                {
                    HandleLoginFailure(loginResponseAuth.ResponseMessage);
                    return RedirectToAction("Login", "Account");
                }

                // Proceed if login is successful
                if (loginResponseAuth.ResponseCode == "SUCCESS")
                {
                    // Fetch user details
                    var tblUser = GetCachedUserDetails(username);
                    if (tblUser == null)
                    {
                        this.TempData["error"] = "User details not found.";
                        return RedirectToAction("Login", "Account");
                    }

                    var tblUser2 = GetCachedRMNames(tblUser.USERID, tblUser.ID_ORGANIZATION);
                    string employeeID = tblUser?.EMPLOYEEID ?? "";

                    // Create session for authenticated user
                    System.Web.HttpContext.Current.Session["UserSession"] = new UserSession
                    {
                        Username = loginResponseAuth.fullname,
                        Roleid = "0",
                        Role = tblUser2.Role,
                        ID_USER = loginResponseAuth.UserID.ToString(),
                        id_ORGANIZATION = Convert.ToInt32(loginResponseAuth.ORGID),
                        DESIGNATION = tblUser.user_designation,
                        GENDER = tblUser.GENDER,
                        OFFICE_ADDRESS = tblUser.OFFICE_ADDRESS,
                        UserFunction = tblUser.user_function,
                        UserGrade = tblUser.user_grade,
                        logo_path = loginResponseAuth.LogoPath,
                        USERID = username,
                        EMPLOYEEID = employeeID,
                        IsSSO = false,
                        SM = tblUser2.SM,
                        DM = tblUser2.DM,
                        RM = tblUser2.RM,
                        APIUrl = ConfigurationManager.AppSettings["api_raw"].ToString(),
                        ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString()
                    };

                    // Log user login
                    this.TempData["loginnotification"] = "yes";
                    var userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

                    if (userSession != null)
                    {
                        string ID_USER = userSession.ID_USER;
                        int id_ORGANIZATION = userSession.id_ORGANIZATION;
                        string Page = "LoginPage";

                        new UtilityModel().AddUserDataLog(ID_USER, id_ORGANIZATION, Page);
                    }

                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, "An unexpected error occurred during login.");

                if (ex.InnerException != null)
                {
                    Log.Error(ex.InnerException, "Inner exception details.");
                }

                // Provide user-friendly message
                this.TempData["error"] = "An unexpected error occurred. Please try again later.";
                return RedirectToAction("Login", "Account");
            }
        }

        private dynamic GetCachedUserDetails(string username)
        {
            dynamic cachedUser = HttpContext.Cache["UserDetails_" + username];

            if (cachedUser == null)
            {
                cachedUser = new UtilityModel().getUserDetails(username);

                // Clear the existing cache item if present
                HttpContext.Cache.Remove("UserDetails_" + username);

                // Store the new data in the cache with an expiration time
                HttpContext.Cache.Insert("UserDetails_" + username, cachedUser, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }

            return cachedUser;
        }

        private dynamic GetCachedRMNames(string userId, int organizationId)
        {
            dynamic cachedRMNames = HttpContext.Cache["RMNames_" + userId + "_" + organizationId];

            if (cachedRMNames == null)
            {
                cachedRMNames = new UtilityModel().getRMNames(userId, organizationId);

                // Clear the existing cache item if present
                HttpContext.Cache.Remove("RMNames_" + userId + "_" + organizationId);

                // Store the new data in the cache with an expiration time
                HttpContext.Cache.Insert("RMNames_" + userId + "_" + organizationId, cachedRMNames, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            }

            return cachedRMNames;
        }



        private void HandleSessionRedirect()
        {
            if (System.Web.HttpContext.Current.Session["urlPath"] != null)
            {
                var path = System.Web.HttpContext.Current.Session["urlPath"]?.ToString();
                if (!string.IsNullOrEmpty(path))
                {
                    System.Web.HttpContext.Current.Session["urlPath"] = null;
                    System.Web.HttpContext.Current.Session["error"] = TempData["error"];
                    Response.Redirect(path);
                }
            }
        }

        private void HandleLoginFailure(string errorMessage)
        {
            string error = errorMessage == "Invalid Username and Password..." ? "Invalid Username or Password..." : errorMessage;
            this.TempData["error"] = error;

            HandleSessionRedirect();
        }


        public ActionResult LoginAPICheck(string USERID, string PASSWORD, string REURL)
        {
            PASSWORD = PASSWORD == null ? "0" : PASSWORD;
            string str1 = USERID;
            string str2 = PASSWORD;
            LoginResponseAuth loginResponseAuth = JsonConvert.DeserializeObject<LoginResponseAuth>(new UtilityModel().getApiPost(APIString.API + "B2BAuthentication", new NameValueCollection()
      {
        {
          "IMEI",
          ""
        },
        {
          "USERID",
          str1
        },
        {
          "PASSWORD",
          str2
        },
        {
          "OS",
          ""
        },
        {
          "Network",
          ""
        },
        {
          "OSVersion",
          ""
        },
        {
          "Details",
          ""
        },
        {
          "LogoPath",
          ""
        }
      }));
            if (loginResponseAuth.ResponseCode == "FAILURE")
            {
                this.TempData["error"] = (object)loginResponseAuth.ResponseMessage;
                loginResponseAuth.ResponseUrl = ConfigurationManager.AppSettings["ApplicationUrl"] + "Account/Login";
                return (ActionResult)this.RedirectToAction("Login", "Account");
                ////return Json((object)loginResponseAuth, JsonRequestBehavior.AllowGet);
            }
            if (!(loginResponseAuth.ResponseCode == "SUCCESS"))
                return (ActionResult)this.View();
            System.Web.HttpContext.Current.Session["UserSession"] = (object)null;

            tbl_user tblUser = new UtilityModel().getUserDetails(str1);
            tbl_user tblUser2 = new UtilityModel().getRMNames(tblUser.USERID, tblUser.ID_ORGANIZATION);
            string EmployeeID = "";

            if (tblUser != null)
            {
                EmployeeID = tblUser.EMPLOYEEID;
            }

            System.Web.HttpContext.Current.Session["UserSession"] = (object)new UserSession()
            {
                Username = loginResponseAuth.fullname,
                Roleid = "0",
                Role = tblUser2.Role,
                ID_USER = loginResponseAuth.UserID.ToString(),
                id_ORGANIZATION = Convert.ToInt32(loginResponseAuth.ORGID),
                DESIGNATION = tblUser.user_designation,
                GENDER = tblUser.GENDER,
                OFFICE_ADDRESS = tblUser.OFFICE_ADDRESS,
                UserFunction = tblUser.user_function,
                UserGrade = tblUser.user_grade,
                logo_path = loginResponseAuth.LogoPath,
                USERID = str1,
                EMPLOYEEID = EmployeeID,
                IsSSO = false,
                SM = tblUser2.SM,
                DM = tblUser2.DM,
                RM = tblUser2.RM,
                APIUrl = ConfigurationManager.AppSettings["api_raw"].ToString(),
                ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString()
            };
            this.TempData["loginnotification"] = (object)"yes";

            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                // Extract ID_USER and id_ORGANIZATION from the userSession object
                string ID_USER = userSession.ID_USER;
                int id_ORGANIZATION = userSession.id_ORGANIZATION;
                string Page = "LoginPage";
                // Call AddUserDataLog method with extracted values
                new UtilityModel().AddUserDataLog(ID_USER, id_ORGANIZATION, Page);
            }

            return (ActionResult)this.RedirectToAction("Index", "Dashboard");

            ////loginResponseAuth.ResponseUrl = ConfigurationManager.AppSettings["ApplicationUrl"] + "Dashboard/Index";
            ////return Json((object)loginResponseAuth, JsonRequestBehavior.AllowGet);
        }


        // sujit change on 02/02/2024 optimize login
        public async Task<ActionResult> LoginAPICheck_Response(string USERID, string PASSWORD, string REURL)
        {
            PASSWORD = PASSWORD ?? "0"; 
            string str1 = USERID;
            string str2 = PASSWORD;

            LoginResponseAuth loginResponseAuth = await System.Threading.Tasks.Task.Run(() =>
            {
                return JsonConvert.DeserializeObject<LoginResponseAuth>(new UtilityModel().getApiPost(APIString.API + "B2BAuthentication", new NameValueCollection()
        {
            { "IMEI", "" },
            { "USERID", str1 },
            { "PASSWORD", str2 },
            { "OS", "" },
            { "Network", "" },
            { "OSVersion", "" },
            { "Details", "" },
            { "LogoPath", "" }
        }));
            });

            if (loginResponseAuth.ResponseCode == "FAILURE")
            {
                TempData["error"] = loginResponseAuth.ResponseMessage;
                loginResponseAuth.ResponseUrl = ConfigurationManager.AppSettings["ApplicationUrl"] + "Account/Login";
                return Json(loginResponseAuth, JsonRequestBehavior.AllowGet);
            }
            else if (loginResponseAuth.ResponseCode != "SUCCESS")
            {
                return View();
            }

            var httpContext = ControllerContext.HttpContext;
            httpContext.Session["UserSession"] = null;

            tbl_user tblUser = await System.Threading.Tasks.Task.Run(() => new UtilityModel().getUserDetails(str1));

            string EmployeeID = tblUser != null ? tblUser.EMPLOYEEID : "";

            httpContext.Session["UserSession"] = new UserSession()
            {
                Username = loginResponseAuth.fullname,
                Roleid = "0",
                ID_USER = loginResponseAuth.UserID.ToString(),
                id_ORGANIZATION = Convert.ToInt32(loginResponseAuth.ORGID),
                logo_path = loginResponseAuth.LogoPath,
                USERID = str1,
                EMPLOYEEID = EmployeeID,
                IsSSO = false,
                APIUrl = ConfigurationManager.AppSettings["api_raw"].ToString(),
                ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString()
            };

            TempData["loginnotification"] = "yes";
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                // Extract ID_USER and id_ORGANIZATION from the userSession object
                string ID_USER = userSession.ID_USER;
                int id_ORGANIZATION = userSession.id_ORGANIZATION;
                string Page = "LoginPage";
                // Call AddUserDataLog method with extracted values
                new UtilityModel().AddUserDataLog(ID_USER, id_ORGANIZATION, Page);
            }
            loginResponseAuth.ResponseUrl = ConfigurationManager.AppSettings["ApplicationUrl"] + "Dashboard/Index";
            return Json(loginResponseAuth, JsonRequestBehavior.AllowGet);
        }


        //  public ActionResult LoginAPICheck_Response(string USERID, string PASSWORD, string REURL)
        //  {
        //      PASSWORD = PASSWORD == null ? "0" : PASSWORD;
        //      string str1 = USERID;
        //      string str2 = PASSWORD;
        //      LoginResponseAuth loginResponseAuth = JsonConvert.DeserializeObject<LoginResponseAuth>(new UtilityModel().getApiPost(APIString.API + "B2BAuthentication", new NameValueCollection()
        //{
        //  {
        //    "IMEI",
        //    ""
        //  },
        //  {
        //    "USERID",
        //    str1
        //  },
        //  {
        //    "PASSWORD",
        //    str2
        //  },
        //  {
        //    "OS",
        //    ""
        //  },
        //  {
        //    "Network",
        //    ""
        //  },
        //  {
        //    "OSVersion",
        //    ""
        //  },
        //  {
        //    "Details",
        //    ""
        //  },
        //  {
        //    "LogoPath",
        //    ""
        //  }
        //}));
        //      if (loginResponseAuth.ResponseCode == "FAILURE")
        //      {
        //          this.TempData["error"] = (object)loginResponseAuth.ResponseMessage;
        //          loginResponseAuth.ResponseUrl = ConfigurationManager.AppSettings["ApplicationUrl"] + "Account/Login";
        //          ////return (ActionResult)this.RedirectToAction("Login", "Account");
        //          return Json((object)loginResponseAuth, JsonRequestBehavior.AllowGet);
        //      }
        //      if (!(loginResponseAuth.ResponseCode == "SUCCESS"))
        //          return (ActionResult)this.View();
        //      System.Web.HttpContext.Current.Session["UserSession"] = (object)null;

        //      tbl_user tblUser = new UtilityModel().getUserDetails(str1);
        //      string EmployeeID = "";

        //      if (tblUser != null)
        //      {
        //          EmployeeID = tblUser.EMPLOYEEID;
        //      }

        //      System.Web.HttpContext.Current.Session["UserSession"] = (object)new UserSession()
        //      {
        //          Username = loginResponseAuth.fullname,
        //          Roleid = "0",
        //          ID_USER = loginResponseAuth.UserID.ToString(),
        //          id_ORGANIZATION = Convert.ToInt32(loginResponseAuth.ORGID),
        //          logo_path = loginResponseAuth.LogoPath,
        //          USERID = str1,
        //          EMPLOYEEID = EmployeeID,
        //          IsSSO = false,
        //          APIUrl = ConfigurationManager.AppSettings["api_raw"].ToString(),
        //          ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString()
        //      };
        //      this.TempData["loginnotification"] = (object)"yes";
        //      ////return (ActionResult)this.RedirectToAction("Index", "Dashboard");

        //      loginResponseAuth.ResponseUrl = ConfigurationManager.AppSettings["ApplicationUrl"] + "Dashboard/Index";
        //      return Json((object)loginResponseAuth, JsonRequestBehavior.AllowGet);
        //  }

        ////////  public ActionResult LoginAPICheck(string USERID, string PASSWORD, string REURL)
        ////////  {
        ////////      PASSWORD = PASSWORD == null ? "0" : PASSWORD;
        ////////      string str1 = USERID;
        ////////      string str2 = PASSWORD;
        ////////      LoginResponseAuth loginResponseAuth = JsonConvert.DeserializeObject<LoginResponseAuth>(new UtilityModel().getApiPost(APIString.API + "B2BAuthenticationLink", new NameValueCollection()
        ////////{
        ////////  {
        ////////    "IMEI",
        ////////    ""
        ////////  },
        ////////  {
        ////////    nameof (USERID),
        ////////    str1
        ////////  },
        ////////  {
        ////////    nameof (PASSWORD),
        ////////    str2
        ////////  },
        ////////  {
        ////////    "OS",
        ////////    ""
        ////////  },
        ////////  {
        ////////    "Network",
        ////////    ""
        ////////  },
        ////////  {
        ////////    "OSVersion",
        ////////    ""
        ////////  },
        ////////  {
        ////////    "Details",
        ////////    ""
        ////////  },
        ////////  {
        ////////    "LogoPath",
        ////////    ""
        ////////  }
        ////////}));
        ////////      if (loginResponseAuth.ResponseCode == "FAILURE")
        ////////      {
        ////////          this.TempData["error"] = (object)loginResponseAuth.ResponseMessage;
        ////////          this.TempData["url"] = (object)REURL;
        ////////          return (ActionResult)this.RedirectToAction("Thirdpartpage", "Account");
        ////////      }

        ////////      tbl_user tblUser = new UtilityModel().getUserDetails(str1);
        ////////      string EmployeeID = "";

        ////////      if (tblUser != null)
        ////////      {
        ////////          EmployeeID = tblUser.EMPLOYEEID;
        ////////      }

        ////////      if (!(loginResponseAuth.ResponseCode == "SUCCESS"))
        ////////          return (ActionResult)this.View();
        ////////      System.Web.HttpContext.Current.Session["UserSession"] = (object)null;

        ////////      System.Web.HttpContext.Current.Session["UserSession"] = (object)new UserSession()
        ////////      {
        ////////          REURL = REURL,
        ////////          Username = loginResponseAuth.UserName,
        ////////          Roleid = "0",
        ////////          ID_USER = loginResponseAuth.UserID.ToString(),
        ////////          id_ORGANIZATION = Convert.ToInt32(loginResponseAuth.ORGID),
        ////////          logo_path = loginResponseAuth.LogoPath,
        ////////          EMPLOYEEID= EmployeeID
        ////////      };
        ////////      this.TempData["loginnotification"] = (object)"yes";
        ////////      return (ActionResult)this.RedirectToAction("Index", "Dashboard");
        ////////  }


        public ActionResult SocialAuth() => (ActionResult)this.View();

        public JsonResult SocialLogin(string user_mail, string name, string uid) => this.Json((object)new IBHFL.Models.SocialAuth()
        {
            user_mail = user_mail,
            user_name = name,
            social_id = uid
        }, JsonRequestBehavior.AllowGet);

        public ActionResult SocialRegister(
          string suser_mail,
          string sname,
          string suid,
          string l_name,
          string occupation)
        {
            try
            {
                if (suid == null)
                    return (ActionResult)this.RedirectToAction("Login", "Account");
                string s = "m20st" + (object)DateTime.Now.Second;
                MD5 md5 = (MD5)new MD5CryptoServiceProvider();
                md5.ComputeHash(Encoding.ASCII.GetBytes(s));
                byte[] hash = md5.Hash;
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < hash.Length; ++index)
                    stringBuilder.Append(hash[index].ToString("x2"));
                string str = stringBuilder.ToString();
                SocailUserDetails socialUserId = new SocialAuthModel().GetSocialUserID(suid);
                if (socialUserId.social_id == suid)
                {
                    System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
                    System.Web.HttpContext.Current.Session["UserSession"] = (object)new UserSession()
                    {
                        Username = sname,
                        Roleid = "0",
                        ID_USER = Convert.ToString(socialUserId.ID_USER),
                        id_ORGANIZATION = socialUserId.id_organisation,
                        logo_path = "",
                        APIUrl = ConfigurationManager.AppSettings["api_raw"].ToString(),
                        ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString()
                    };
                    UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

                    if (userSession != null)
                    {
                        // Extract ID_USER and id_ORGANIZATION from the userSession object
                        string ID_USER = userSession.ID_USER;
                        int id_ORGANIZATION = userSession.id_ORGANIZATION;
                        string Page = "LoginPage";
                        // Call AddUserDataLog method with extracted values
                        new UtilityModel().AddUserDataLog(ID_USER, id_ORGANIZATION, Page);
                    }
                    return (ActionResult)this.RedirectToAction("Index", "Dashboard");
                }
                SocailUserDetails request1 = new SocailUserDetails();
                request1.social_id = suid;
                request1.social_email = suser_mail;
                if (suser_mail == null)
                    request1.social_email = sname;
                request1.social_name = sname;
                request1.spasswd = str;
                int num = new SocialAuthModel().InsertSocialUser(request1);
                if (num > 0)
                {
                    SocailUserDetails request2 = new SocailUserDetails();
                    request2.ID_USER = num;
                    request2.social_id = suid;
                    request2.social_email = suser_mail;
                    if (suser_mail == null)
                        request2.social_email = sname;
                    request2.social_name = sname;
                    request2.lastname = request2.lastname == null ? sname : l_name;
                    request2.age = 0;
                    request2.logo_path = "";
                    new SocialAuthModel().InsertSocialUserProfile(request2);
                    System.Web.HttpContext.Current.Session["UserSession"] = (object)new UserSession()
                    {
                        Username = sname,
                        Roleid = "0",
                        ID_USER = Convert.ToString(num).ToString(),
                        id_ORGANIZATION = Convert.ToInt32(16),
                        logo_path = "",
                        APIUrl = ConfigurationManager.AppSettings["api_raw"].ToString(),
                        ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString()
                    };
                    this.TempData["loginnotification"] = (object)"yes";
                    UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

                    if (userSession != null)
                    {
                        // Extract ID_USER and id_ORGANIZATION from the userSession object
                        string ID_USER = userSession.ID_USER;
                        int id_ORGANIZATION = userSession.id_ORGANIZATION;
                        string Page = "LoginPage";
                        // Call AddUserDataLog method with extracted values
                        new UtilityModel().AddUserDataLog(ID_USER, id_ORGANIZATION, Page);
                    }
                    return (ActionResult)this.RedirectToAction("Index", "Dashboard");
                }
                this.TempData["sucsess-msg"] = (object)"message not successfully send";
                return (ActionResult)this.RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("ErrorPage", "ErrorView");
            }
        }

        public ActionResult Logout()
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                // Extract ID_USER and id_ORGANIZATION from the userSession object
                string ID_USER = userSession.ID_USER;
                int id_ORGANIZATION = userSession.id_ORGANIZATION;
                string Page = "Logout";
                // Call AddUserDataLog method with extracted values
                new UtilityModel().AddUserDataLog(ID_USER, id_ORGANIZATION, Page);
            }
          
            var _path = System.Web.HttpContext.Current.Session["urlPath"];

           UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            if (content != null)
            {
                if (content.REURL != null)
                {
                    if (content.REURL != "OPP")
                    {
                        string reurl = content.REURL;
                        this.Session.Abandon();
                        this.Session.Clear();
                        this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        this.Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1.0));
                        this.Response.Cache.SetNoStore();
                        this.Response.Cookies.Clear();
                        HttpCookie cookie = new HttpCookie("mycookies");
                        this.Response.Cookies["mycookies"]["email"] = "";
                        this.Response.Cookies["mycookies"]["pass"] = "";
                        this.Response.Cookies["mycookies"].Expires = DateTime.Now.AddDays(1.0);
                        this.Response.Cookies.Add(cookie);
                        System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
                        return (ActionResult)this.Redirect(reurl);
                    }
                    this.Session.Abandon();
                    this.Session.Clear();
                    this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    this.Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1.0));
                    this.Response.Cache.SetNoStore();
                    this.Response.Cookies.Clear();
                    HttpCookie cookie1 = new HttpCookie("mycookies");
                    this.Response.Cookies["mycookies"]["email"] = "";
                    this.Response.Cookies["mycookies"]["pass"] = "";
                    this.Response.Cookies["mycookies"].Expires = DateTime.Now.AddDays(1.0);
                    this.Response.Cookies.Add(cookie1);
                    System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
                    return (ActionResult)this.RedirectToAction("oppertunelogout");
                }
            }
            this.Session.Abandon();
            this.Session.Clear();
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1.0));
            this.Response.Cache.SetNoStore();
            this.Response.Cookies.Clear();
            HttpCookie cookie2 = new HttpCookie("mycookies");
            this.Response.Cookies["mycookies"]["email"] = "";
            this.Response.Cookies["mycookies"]["pass"] = "";
            this.Response.Cookies["mycookies"].Expires = DateTime.Now.AddDays(1.0);
            this.Response.Cookies.Add(cookie2);
            ////System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
            ///
            if (_path != null)
            {
                return Redirect(_path.ToString());
            }

            ////return (ActionResult)this.RedirectToAction("RedirectToLoginPage", "Account");
            return (ActionResult)this.RedirectToAction("Login", "Account");
        }

        public string getSessionStatus() => (UserSession)this.HttpContext.Session.Contents["UserSession"] == null ? "0" : "1";

        public string setSessionStatus(string uname, string password)
        {
            LoginResponseAuth loginResponseAuth = JsonConvert.DeserializeObject<LoginResponseAuth>(new UtilityModel().getApiPost(APIString.API + "B2BAuthentication", new NameValueCollection()
      {
        {
          "IMEI",
          ""
        },
        {
          "USERID",
          uname
        },
        {
          "PASSWORD",
          password
        },
        {
          "OS",
          ""
        },
        {
          "Network",
          ""
        },
        {
          "OSVersion",
          ""
        },
        {
          "Details",
          ""
        },
        {
          "LogoPath",
          ""
        }
      }));
            if (loginResponseAuth.ResponseCode == "FAILURE")
                return "0";
            if (loginResponseAuth.ResponseCode == "SUCCESS")
            {
                System.Web.HttpContext.Current.Session["UserSession"] = (object)null;
                System.Web.HttpContext.Current.Session["UserSession"] = (object)new UserSession()
                {
                    Username = loginResponseAuth.UserName,
                    Roleid = "0",
                    ID_USER = loginResponseAuth.UserID.ToString(),
                    id_ORGANIZATION = Convert.ToInt32(loginResponseAuth.ORGID),
                    logo_path = loginResponseAuth.LogoPath,
                    APIUrl = ConfigurationManager.AppSettings["api_raw"].ToString(),
                    ApplicationUrl = ConfigurationManager.AppSettings["ApplicationUrl"].ToString()
                };
            }
            return "1";
        }

        public ActionResult Thirdpartpage() => (ActionResult)this.View();

        public string sendpassword(string uid)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            return new UtilityModel().getApiResponseString(APIString.API + "ForgetPassword?userid=" + uid);
        }

        [SessionExpireFilter]
        public void sendGCM()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            tbl_user_gcm_log tblUserGcmLog = new tbl_user_gcm_log();
            tblUserGcmLog.id_user = new int?(num2);
            tblUserGcmLog.status = "A";
            tblUserGcmLog.updated_date_time = new DateTime?(DateTime.Now);
            string str1 = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
            tblUserGcmLog.GCMID = str1;
            string str2 = ((IEnumerable<NetworkInterface>)NetworkInterface.GetAllNetworkInterfaces()).Where<NetworkInterface>((Func<NetworkInterface, bool>)(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)).Select<NetworkInterface, string>((Func<NetworkInterface, string>)(nic => nic.GetPhysicalAddress().ToString())).FirstOrDefault<string>();
            new UtilityModel().getApiPost(APIString.API + "logGCM", new NameValueCollection()
      {
        {
          "GCM",
          str2
        },
        {
          "UID",
          Convert.ToString(num2)
        },
        {
          "OID",
          Convert.ToString(num1)
        }
      });
        }

        public ActionResult oppertunelogout() => (ActionResult)this.View();

        //Added by Vidit on 04-08-2023
        public string Decrypt(string USERID)
        {
            string myKey = "L12M13S19$";
            TripleDESCryptoServiceProvider cryptDES3 = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider cryptMD5Hash = new MD5CryptoServiceProvider();
            cryptDES3.Key = cryptMD5Hash.ComputeHash(Encoding.ASCII.GetBytes(myKey));
            cryptDES3.Mode = CipherMode.ECB;
            ICryptoTransform desdencrypt = cryptDES3.CreateDecryptor();
            byte[] buff = Convert.FromBase64String(USERID);
            string decrypt = Encoding.ASCII.GetString(desdencrypt.TransformFinalBlock(buff, 0, (int)buff.Length));
            return decrypt;
        }

        #region static url section
        public LoginUrl_tbl getUrl(string url)
        {
           
            if (url.ToLower() == "bata")
            {
                var response = new LoginUrl_tbl();
                response.BackgroundUrl = "https://images.pexels.com/photos/36717/amazing-animal-beautiful-beautifull.jpg?auto=compress&cs=tinysrgb&dpr=1&w=500";
                response.IconUrl = "https://www.m2ost.in/M2OST_CMS_PriME/Content/SKILLMUNI_DATA/ORGLOGO/117.png";
                LoginPageGetData(117);
                return response;
            }
            else if (url.ToLower() == "paathshala")
            {
                var response = new LoginUrl_tbl();
                response.BackgroundUrl = "https://images.pexels.com/photos/36717/amazing-animal-beautiful-beautifull.jpg?auto=compress&cs=tinysrgb&dpr=1&w=500";
                response.IconUrl = "https://www.m2ost.in/M2OST_CMS_PriME/Content/SKILLMUNI_DATA/ORGLOGO/24.png";
                LoginPageGetData(24);
                return response;
            }
            return null;
        }
        #endregion

        //For the Login Data 

        public ActionResult LoginPageGetData(int num)
        {
            addLoginPageModel LoginPageModel = new addLoginPageModel();
            List<tbl_login_page> LoginPageList = LoginPageModel.GetLoginPageData(num);

            if (LoginPageList.Count == 0)
            {
                // Redirect to another action method when LoginPageList is empty
                //this.TempData["error"] = "An unexpected error occurred during login.";
                return RedirectToAction("Login");
            }
            else
            {
                string str1 = LoginPageList[0].Background_Image;
                string str2 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/LoginImage/" + str1;
                string str3 = LoginPageList[0].Logo_Image;
                string str4 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/LoginImage/" + str3;

                foreach (var loginItem in LoginPageList)
                {
                    loginItem.Background_Image = str2;
                    loginItem.Logo_Image = str4;
                }

                // Store the modified loginList in ViewData
                ViewData["LoginPageList"] = LoginPageList;

                // Return the view with LoginPageList
                return View(LoginPageList);
            }
        }

    }
}
