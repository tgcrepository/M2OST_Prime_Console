// Decompiled with JetBrains decompiler
// Type: IBHFL.Controllers.DashboardController
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using IBHFL.Models;
using Newtonsoft.Json;
using RusticiSoftware.HostedEngine.Client;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Configuration;
using Microsoft.Ajax.Utilities;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Security.Policy;
using PdfiumViewer;
using System.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Windows.Forms;
using Org.BouncyCastle.Tsp;
using System.Security.Cryptography;
using WebGrease.Extensions;
using MySqlX.XDevAPI.Relational;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Tls;
using System.Web.Razor.Parser.SyntaxTree;
using System.Xml.Linq;


namespace IBHFL.Controllers
{
    ////[UserFilter]
    [SessionExpireFilter]
    public class DashboardController : Controller
    {
        public static string CorebusAPIUrl = ConfigurationManager.AppSettings["CorebusAPIUrl"].ToString();
        private db_m2ostEntities db = new db_m2ostEntities();
        private PdfDocument _pdfDocument;
        private string pdfUrl;
        public static string Tempoin1 = null;
        public static string DateTimecal1 = null;
        public static string myData1 = null;
        public static string valueloop1 = null;
        public static class GlobalVariables
        {
            public static string MyGlobalValue { get; set; }
        }
        public void AddPointsAPI(string KPIName, string Score)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            string GameID = "0";
            string UserID = "", EmpID = "";
            tbl_profile tblProfile = new tbl_profile();
            tbl_organization tblOrganization = new tbl_organization();
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
                UserID = content.USERID;
                EmpID = content.EMPLOYEEID;

                tblProfile = new UtilityModel().getUserProfileDetails(num2.ToString());
                tblOrganization = new UtilityModel().getOrganizationDetails(num1);
            }

            if (tblOrganization != null)
            {
                GameID = tblOrganization.CONTACTNUMBER;
            }

            //////UserID = "GOP_IN501";
            //////GameID = "183";

            GetPostAPIDetails("https://coroebus.in/coroebus-tgc-api-levels/dashboard/produce_1", UserID, GameID, "1", "1", "W", KPIName, Score);

        }

        public void EvaluateProgramCompletionStatus()
        {
            content_list_session content = (content_list_session)this.HttpContext.Session.Contents["ContentSession"];

            if (content != null)
            {
                int Category = content.CategoryID;
                int UserID = 0, TotalCount = 0, CompletedCount = 0;

                UserSession userData = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                UserID = Convert.ToInt32(userData.ID_USER);

                using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
                {
                    string sql1 = "select count(*) count FROM tbl_content_organization_mapping where id_category='" + Category + "' AND status='A'";
                    TotalCount = m2ostDbContext.Database.SqlQuery<int>(sql1).FirstOrDefault<int>();

                    string sql2 = "SELECT COUNT(distinct id_content) count FROM tbl_content_counters where id_content in (SELECT id_content FROM tbl_content_organization_mapping where id_category='" + Category + "' AND status='A') AND id_user='" + UserID + "'";
                    CompletedCount = m2ostDbContext.Database.SqlQuery<int>(sql2).FirstOrDefault<int>();

                    if (TotalCount > 0)
                    {
                        if (TotalCount == CompletedCount)
                        {
                            string KPIName = "Learning programs completed  (score = 5)", Score = "5";

                            AddPointsAPI(KPIName, Score);
                        }
                    }
                }
            }
        }

        public void EvaluateProgramCompletionOnTimeStatus()
        {
            content_list_session content = (content_list_session)this.HttpContext.Session.Contents["ContentSession"];

            if (content != null)
            {
                int Category = content.CategoryID;
                int UserID = 0, TotalCount = 0, CompletedCount = 0;

                UserSession userData = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                UserID = Convert.ToInt32(userData.ID_USER);

                using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
                {
                    string sql1 = "select count(*) count FROM tbl_content_organization_mapping where id_category='" + Category + "' AND status='A'";
                    TotalCount = m2ostDbContext.Database.SqlQuery<int>(sql1).FirstOrDefault<int>();

                    string sql2 = "SELECT COUNT(X.id_content) count FROM (SELECT id_content,MAX(updated_Date_time) UpdatedTime FROM tbl_content_counters where id_content in (SELECT id_content FROM tbl_content_organization_mapping where id_category=344 AND status='A') AND id_user='238' GROUP BY id_content)X WHERE x.UpdatedTime<(SELECT expiry_date FROM tbl_content_program_mapping where id_category='344' AND id_user='238')";
                    CompletedCount = m2ostDbContext.Database.SqlQuery<int>(sql2).FirstOrDefault<int>();

                    if (TotalCount > 0)
                    {
                        if (TotalCount == CompletedCount)
                        {
                            string KPIName = "Programs completed on time  (score = 5)", Score = "5";

                            AddPointsAPI(KPIName, Score);
                        }
                    }
                }
            }
        }

        public static async System.Threading.Tasks.Task<Rootobject> GetPostAPIDetails(string url, string UserID, string Game, string SectionView, string PageNum, string DeviceType, string KPIName, string Score)
        {
            var RootobjectResponse = new Rootobject();
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent("{\"_userid\":\"" + UserID + "\",\"_game\":\"" + Game + "\",\"_section_view\":\"" + SectionView + "\",\"page_number\":\"" + PageNum + "\",\"device_type\":\"" + DeviceType + "\"}", null, "application/json");
                //var content = new StringContent("{\"_userid\":\"GOP_IN501\",\"_game\":\"183\",\"_section_view\":\"1\",\"page_number\":\"1\",\"device_type\":\"W\"}", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                ////Console.WriteLine(await response.Content.ReadAsStringAsync());
                var response1 = await response.Content.ReadAsStringAsync();

                RootobjectResponse = JsonConvert.DeserializeObject<Rootobject>(response1);

                ////System.Threading.Thread.Sleep(5000);

                if (RootobjectResponse != null)
                {
                    AddPointsAPIDetails("https://www.coroebus.in/coroebus-tgc-api-levels//KpiPointsProcessing/add_points", RootobjectResponse, KPIName, Score);
                }
            }
            catch (Exception ex)
            {

            }
            return RootobjectResponse;
        }

        public static async void AddPointsAPIDetails(string url, Rootobject Resp, string KPIName, string Score)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new StringContent("{\"game_name\":\"" + Resp.data._personal_data.game_name + "\",\"team_name\":\"" + Resp.data._personal_data.team_name + "\",\"user_name\":\"" + Resp.data._personal_data.first_name + "\",\"employeeid\":\"" + Resp.data._personal_data.EMPLOYEEID + "\",\"kpi_name\":\"" + KPIName + "\",\"score\":\"" + Score + "\",\"date_time_stamp\":\"" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\",\"_game\":\"" + Resp.data._personal_data.id_coroebus_game + "\",\"_userid\":\"" + Resp.data._personal_data.USERID + "\"}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response.EnsureSuccessStatusCode();
            ////Console.WriteLine(await response.Content.ReadAsStringAsync());
            var response1 = await response.Content.ReadAsStringAsync();

            ////var RootobjectResponse = JsonConvert.DeserializeObject<Rootobject>(response1);
            ////return RootobjectResponse;
        }

        public ActionResult RedirectPage(string NEXTAPI)
        {
            try
            {
                this.ViewData["RedirectUrl"] = HttpUtility.UrlEncode(NEXTAPI).ToString();
                return (ActionResult)this.View();
            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult RedirectToBataRetailMastersPage()
        {
            return (ActionResult)this.View();
        }

        public ActionResult Index(string Message, string Flag = "")
        {

            try
            {
                ////AddPointsAPI();
                this.ControllerContext.RouteData.Values["action"].ToString();
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int num1 = 0;
                int num2 = 0;
                int GameID = 0;
                string UserID = "", Name = "", EmpID = "";
                tbl_profile tblProfile = new tbl_profile();
                if (content != null)
                {
                    num1 = Convert.ToInt32(content.id_ORGANIZATION);
                    num2 = Convert.ToInt32(content.ID_USER);
                    UserID = content.USERID;
                    EmpID = content.EMPLOYEEID;
                    GameID = new UtilityModel().getGameId(num2);
                    tblProfile = new UtilityModel().getUserProfileDetails(num2.ToString());
                }

                if (tblProfile != null)
                {
                    Name = tblProfile.FIRSTNAME;
                }

                APIRESPONSE apiresponse1 = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "Category?orgID=" + (object)num1 + "&uid=" + (object)num2));
                string str = (string)null;
                List<details> detailsList = new List<details>();
                if (apiresponse1.KEY == "SUCCESS")
                    detailsList = JsonConvert.DeserializeObject<List<details>>(apiresponse1.MESSAGE);
                else
                    str = apiresponse1.MESSAGE;
                APIRESPONSE apiresponse2 = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "Category?orgID=" + (object)num1 + "&uid=" + (object)num2));
                List<CategoryTile> categoryTileList1 = new List<CategoryTile>();
                if (!(apiresponse2.KEY == "SUCCESS"))
                    return (ActionResult)this.RedirectToAction("maintenance", (object)new
                    {
                        whatever = apiresponse2.MESSAGE
                    });
                List<CategoryTile> categoryTileList2 = JsonConvert.DeserializeObject<List<CategoryTile>>(apiresponse2.MESSAGE);
                foreach (CategoryTile categoryTile in categoryTileList2)
                {
                    //if (categoryTile.CategoryName == "Training Schedule")
                    //{
                    //    categoryTile.Template = "8";
                    //}
                    //if (categoryTile.CategoryName == "My Progress")
                    //{
                    //    categoryTile.Template = "9";
                    //}
                    //if (categoryTile.CategoryName == "My Milestone")
                    //{
                    //    categoryTile.Template = "10";
                    //}

                    if (categoryTile.Template == "8")
                    {
                        categoryTile.Template = "8";
                    }
                    if (categoryTile.Template == "9")
                    {
                        categoryTile.Template = "9";
                    }
                    if (categoryTile.Template == "10")
                    {
                        categoryTile.Template = "10";
                    }

                    if (categoryTile.NEXTAPI.Contains("@userid"))
                    {
                        categoryTile.NEXTAPI = categoryTile.NEXTAPI.Replace("@idgame", GameID.ToString()).Replace("@userid", UserID.ToString()).Replace("@name", Name).Replace("@iduser", num2.ToString()).Replace("@orgid", num1.ToString());
                    }
                    if (categoryTile.Template == "11")
                    {
                        categoryTile.NEXTAPI = categoryTile.NEXTAPI.Replace("@iduser", num2.ToString()).Replace("@orgid", num1.ToString());

                    }
                    else
                    {
                        if (categoryTile.CategoryName == "Rewards")
                        {
                            //categoryTile.NEXTAPI = categoryTile.NEXTAPI.Replace('&', '_');
                            categoryTile.NEXTAPI = categoryTile.NEXTAPI.Replace('&', '&');
                        }
                        else
                        {
                            categoryTile.NEXTAPI = categoryTile.NEXTAPI.Replace('&', '_');

                        }
                    }
                }

                this.ViewData["obj"] = (object)categoryTileList2;
                APIRESPONSE apiresponse3 = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "getNotificationList?userid=" + (object)num2 + "&orgid=" + (object)num1));
                NotificationList notificationList = new NotificationList();
                if (apiresponse3.KEY == "SUCCESS")
                    notificationList = JsonConvert.DeserializeObject<NotificationList>(apiresponse3.MESSAGE);
                APIRESPONSE apiresponse4 = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiPost(APIString.API + "getScheduledEventList", new NameValueCollection()
        {
          {
            "OID",
            num1.ToString()
          },
          {
            "UID",
            num2.ToString()
          },
          {
            "DNO",
            "0"
          },
          {
            "MNO",
            "0"
          },
          {
            "YNO",
            "0"
          }
        }));
                EventResponse eventResponse = new EventResponse();
                if (apiresponse4.KEY == "SUCCESS")
                    eventResponse = JsonConvert.DeserializeObject<EventResponse>(apiresponse4.MESSAGE);
                List<GameElements> myGames = new UtilityModel().getMyGames(num1, num2);
                string userName = new UtilityModel().getUserName(num2);
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(num1, num2);
                if (this.TempData["loginnotification"] != null)
                    this.TempData["Loginnotify"] = (object)"true";
                else
                    this.TempData["Loginnotify"] = (object)"false";
                string apiResponseString = new UtilityModel().getApiResponseString("https://www.m2ost.in/CoroebusBetaApi/api/GetAESconvertedUserId?UserId=" + content.USERID);
                this.ViewData["event"] = (object)eventResponse;
                this.ViewData["result"] = (object)notificationList;
                this.ViewData["message"] = (object)Message;
                this.ViewData["orgid"] = (object)num1;
                this.ViewData["userid"] = (object)num2;
                this.ViewData["Mygames"] = (object)myGames;
                this.ViewData["userName"] = (object)userName;
                this.ViewData["encrypt"] = (object)apiResponseString;
                this.ViewData["Name"] = (object)Name;


                //change for though and greeting

                string thoughtsQuery = " SELECT thoughts_name FROM tbl_thoughts WHERE id_organization = " + num1 + " AND NOW() BETWEEN start_date_time AND expired_data AND Status = 'A' ORDER BY start_date_time DESC LIMIT 1";

                List<string> thoughtsNames = new UtilityModel().GetThoughtsNames(thoughtsQuery);
                if (thoughtsNames.Count > 0)
                {
                    string str4 = ConfigurationManager.AppSettings["ThroughtandGreetingPATH"].ToString() + thoughtsNames[0];
                    //this.HttpContext.Session["thoughtsNames"] = str4;
                    this.ViewData["thoughtsNames"] = str4;

                }

                //string greetingsQuery = "SELECT image_and_gif FROM tbl_greetings WHERE id_organization = " + num1 + " AND Status = 'A'  ORDER BY id_greetings DESC LIMIT 1";
                //List<string> greetingsNames = new UtilityModel().GetgreetingsNames(greetingsQuery);
                //if (thoughtsNames.Count > 0)
                //{
                //    string firstGreetingName = greetingsNames.FirstOrDefault();
                //    string str5 = ConfigurationManager.AppSettings["ThroughtandGreetingPATH"].ToString() + firstGreetingName;

                //    this.ViewData["greetingsNames_1"] = str5;

                //}

                //string greetingsNamesString = string.Join(", ", greetingsQuery);
                //this.ViewData["greetingsNames_1"] = greetingsNamesString;


                //


                string VideoQuerry = "select COUNT(*) as count FROM tbl_login_content_log WHERE org_id = " + num1 + " AND id_user = " + num2 + "";
                int Video_Count = new UtilityModel().getRecordCount(VideoQuerry);
                if (Video_Count == 0)
                {
                    ViewBag.DisplayVideo = true;
                    VideoGetData();
                    string InsertVideoQuerry = "Insert into tbl_login_content_log (org_id,id_user,user_id,content_read,updated) values (" + num1 + "," + num2 + ",'" + UserID + "','Y',NOW())";
                    new UtilityModel().InsertDataToDB(InsertVideoQuerry);

                }
                string page_name = "Tile_Page";
                //  ProductTourinfo(page_name);

                return (ActionResult)this.View();
            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult IndexDetails() => (ActionResult)this.View();

        public ActionResult IndexViewMore() => (ActionResult)this.View();

        public ActionResult CreateEvent() => (ActionResult)this.View();

        public ActionResult Marketplace()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int user_id = 0;
            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                user_id = Convert.ToInt32(content.ID_USER);
            }
            this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
            return (ActionResult)this.View();
        }

        public ActionResult ProductView(string NEXTAPI, int CategoryID, string _categoryHeader, int? _categoryType, string Page)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                // Extract ID_USER and id_ORGANIZATION from the userSession object
                string ID_USER = userSession.ID_USER;
                int id_ORGANIZATION = userSession.id_ORGANIZATION;
                if (Page == null)
                {
                    string Page1 = _categoryHeader.ToString();
                    new UtilityModel().AddUserDataLog(ID_USER, id_ORGANIZATION, Page1);

                }
                else
                {
                    string Page1 = Page.ToString();
                    new UtilityModel().AddUserDataLog(ID_USER, id_ORGANIZATION, Page1);

                }

                // Call AddUserDataLog method with extracted values
            }
            List<DisplayCategory> displayCategoryList1 = new List<DisplayCategory>();

            try
            {
                DateTime currentDateTime = DateTime.Now;
                string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");

                this.ViewData["_nexr"] = (string)NEXTAPI;
                ViewBag._categoryType = _categoryType;
                ViewBag._categoryId = CategoryID;
                ViewBag._PageName = Page;
                List<DisplayCategory> displayCategoryList = null;
                this.TempData["Page"] = (object)CategoryID;
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int num = 0;
                string EmpID = "";
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    num = Convert.ToInt32(content.ID_USER);
                    EmpID = content.EMPLOYEEID;
                }
                NEXTAPI = NEXTAPI.Replace('_', '&');
                TempData["_nextAPI"] = NEXTAPI;
                if (CategoryID == 494 || CategoryID == 519)
                {
                    ViewBag.CategoryType = 8;
                }
                else if (CategoryID == 493 || CategoryID == 523)
                {
                    ViewBag.CategoryType = 9;
                }
                else if (CategoryID == 490 || CategoryID == 524)
                {
                    ViewBag.CategoryType = 10;
                }
                else
                {
                    displayCategoryList = JsonConvert.DeserializeObject<List<DisplayCategory>>(new UtilityModel().getApiResponseString(APIString.RAW + NEXTAPI));

                    if (!string.IsNullOrEmpty(_categoryHeader))
                    {
                        if (displayCategoryList != null)
                        {
                            KPIController kp = new KPIController();
                            foreach (DisplayCategory displayCategory in displayCategoryList)
                            {
                                if (displayCategory.Heading == _categoryHeader)
                                {
                                    //if (_categoryHeader == "Induction Training")
                                    //{
                                    //   // GlobalVariables.MyGlobalValue = "Induction Training";
                                    //    Session["productsIT"] = "Induction Training";
                                    //}
                                    //else if (_categoryHeader == "MTP")
                                    //{
                                    //    // GlobalVariables.MyGlobalValue = null;
                                    //    Session["productsIT"] = "MTP";
                                    //}
                                    //else 
                                    //{
                                    //    // GlobalVariables.MyGlobalValue = null;
                                    //    Session["productsIT"] = null;
                                    //}
                                   
                                   



                                        if (displayCategory.Categories.Count > 0)
                                        {
                                            var CategoryHeadingCountData = kp.CategoryHeadersCount(displayCategory.HeadingID);
                                            displayCategory.CategoryHeadingCount = CategoryHeadingCountData;

                                            foreach (details CategoryDetails in displayCategory.Categories)
                                            {
                                                List<Assessment_ID_for_certification> assessment_ID_For_ = new List<Assessment_ID_for_certification>();

                                                if (CategoryDetails.NEXTURL.Contains("@userid"))
                                                {
                                                    CategoryDetails.NEXTURL = CategoryDetails.NEXTURL.Replace("@userid", EmpID);
                                                }

                                                using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
                                                {


                                                string sql1 = "SELECT * FROM tbl_assessment_user_assignment a, tbl_assessment b " +
                                                                 "WHERE id_category = " + CategoryDetails.CategoryID +
                                                               " AND id_user = " + num +
                                                               " AND a.status = 'A' AND a.expire_date > '" + formattedDateTime + "' ";
                                                //For test 
                                                ///string sql1 = "SELECT * FROM tbl_assessment_user_assignment a, tbl_assessment b WHERE id_category = " + CategoryDetails.CategoryID + " AND id_user =" + num;


                                                assessment_ID_For_ = new UtilityModel().getUserAssignment(sql1);

                                                    string sql2 = string.Empty;

                                                    if (assessment_ID_For_.Count == 0)
                                                    {
                                                        string sql3 = "SELECT c.id_assessment, c.assessment_title  FROM tbl_assessment_categoty_mapping a, tbl_category b, tbl_assessment c WHERE a.id_category = b.id_category AND a.id_assessment = c.id_assessment AND b.id_organization = " + CategoryDetails.OrganisationId + " and b.status = 'A' and b.id_category =" + CategoryDetails.CategoryID;
                                                        assessment_ID_For_ = new UtilityModel().getAssessmentCategotyMapping(sql3);

                                                        string sql5 = "SELECT * FROM tbl_assessment_user_assignment a, tbl_assessment b WHERE id_category = " + CategoryDetails.CategoryID + " AND id_user =" + num + " AND  a.status = 'A' AND  a.expire_date > '" + formattedDateTime + "'";
                                                        assessment_ID_For_ = new UtilityModel().getUserAssignment(sql5);
                                                        //
                                                        if (assessment_ID_For_.Count > 0)
                                                        {
                                                            string sql4 = "SELECT * FROM tbl_assessment_user_assignment a, tbl_assessment b WHERE id_category = " + CategoryDetails.CategoryID + " AND id_user =" + num;
                                                            assessment_ID_For_ = new UtilityModel().getUserAssignment(sql4);

                                                            sql2 = " select MAX(attempt_number) attempt_number from tbl_rs_type_qna where id_user = " + num + " and id_assessment = " + assessment_ID_For_.FirstOrDefault().id_assessment + "";
                                                            //sql2 = "Select MAX(AttemptNo) AttemptNo from tbl_user_kpi_data_log where id_user = " + num + " and Content_Assessment_ID =" + assessment_ID_For_.FirstOrDefault().id_assessment + " and KPI_Name like '%Mastery Score%'";
                                                            var _count = new UtilityModel().getScore(sql2, 1);

                                                            ////|| CategoryDetails.CategoryHeader.ToString().Contains("Genesis")

                                                            if (_count.attempt_no > 1 || CategoryDetails.CategoryHeader.ToString().Contains("Ethics"))
                                                            {
                                                                sql2 = "Select MAX(scoring_value) scoring_value, AttemptNo as attempt_number from tbl_user_kpi_data_log where id_user = " + num + " and Content_Assessment_ID = " + assessment_ID_For_.FirstOrDefault().id_assessment + " and KPI_Name like '%Mastery Score%' and AttemptNo >= 1 order by scoring_value DESC;";
                                                                var _score = new UtilityModel().getScore(sql2, 2);

                                                                if (string.IsNullOrEmpty(_score.scoring_value.ToString()))
                                                                {
                                                                    sql2 = " Select (result_in_percentage) scoring_value,  attempt_number from tbl_rs_type_qna where id_user = " + num + " and id_assessment = " + assessment_ID_For_.FirstOrDefault().id_assessment + " and attempt_number >= 1  order by  result_in_percentage DESC limit 1;";
                                                                    _score = new UtilityModel().getScore(sql2, 2);
                                                                }

                                                                CategoryDetails._certificatePercentage = _score.scoring_value.ToString();
                                                                _count.attempt_no = _score.attempt_no;
                                                            }



                                                        }
                                                        //

                                                    }
                                                    if (assessment_ID_For_.Count > 0)
                                                    {
                                                        //sql2 = "Select MAX(AttemptNo) AttemptNo from tbl_user_kpi_data_log where id_user = " + num + " and Content_Assessment_ID =" + assessment_ID_For_.FirstOrDefault().id_assessment + " and KPI_Name like '%Mastery Score%'";
                                                        sql2 = "Select MAX(attempt_number) attempt_number from tbl_rs_type_qna where id_user = " + num + " and id_assessment =" + assessment_ID_For_.FirstOrDefault().id_assessment + "";
                                                        var _count = new UtilityModel().getScore(sql2, 1);

                                                        if (_count.attempt_no > 1 || CategoryDetails.CategoryHeader.ToString().Contains("Ethics"))
                                                        {
                                                            sql2 = "Select MAX(scoring_value) scoring_value, AttemptNo as attempt_number from tbl_user_kpi_data_log where id_user = " + num + " and Content_Assessment_ID = " + assessment_ID_For_.FirstOrDefault().id_assessment + " and KPI_Name like '%Mastery Score%' and AttemptNo >= 1 order by scoring_value DESC;";
                                                            var _score = new UtilityModel().getScore(sql2, 2);

                                                            if (string.IsNullOrEmpty(_score.scoring_value.ToString()))
                                                            {
                                                                sql2 = " Select (result_in_percentage) scoring_value, (attempt_number) attempt_number from tbl_rs_type_qna where id_user = " + num + " and id_assessment = " + assessment_ID_For_.FirstOrDefault().id_assessment + " and attempt_number >= 1  order by  result_in_percentage DESC limit 1;";
                                                                _score = new UtilityModel().getScore(sql2, 2);
                                                            }

                                                            CategoryDetails._certificatePercentage = _score.scoring_value.ToString();
                                                            _count.attempt_no = _score.attempt_no;
                                                        }
                                                        else if (_count.attempt_no == 1)
                                                        {
                                                            CategoryDetails._certificatePercentage = "Your 2nd attempt is pending";
                                                        }
                                                        CategoryDetails.assessment_ID = assessment_ID_For_.FirstOrDefault().id_assessment;

                                                        CategoryDetails.id_user = num;
                                                        CategoryDetails.AttemptNo = _count.attempt_no;
                                                        string Idorg = CategoryDetails.OrganisationId.ToString();
                                                    // For downlode the certificate

                                                    if (int.TryParse(CategoryDetails._certificatePercentage, out int certificatePercentage))
                                                    {
                                                        // Define SQL query with parameters
                                                        string sql12 = $"SELECT certificate_percentage FROM tbl_assessment WHERE id_assessment = {CategoryDetails.assessment_ID} ORDER BY id_assessment DESC LIMIT 1;";

                                                        // Fetch certificate percentage
                                                        int? certificatePercentageValue = Convert.ToInt32(new UtilityModel().getCertificatepercentage(sql12));

                                                        CategoryDetails.certificatepercentagecompletion = certificatePercentageValue.Value;

                                                        

                                                        if (Convert.ToInt32(CategoryDetails._certificatePercentage) < CategoryDetails.certificatepercentagecompletion)
                                                        {
                                                        }
                                                        else
                                                        {
                                                            if (assessment_ID_For_.FirstOrDefault().id_assessment == 468 || assessment_ID_For_.FirstOrDefault().id_assessment == 474 || CategoryDetails.CategoryHeader.ToString().Contains("Ethics"))
                                                            {
                                                                if (Convert.ToInt32(CategoryDetails._certificatePercentage) < CategoryDetails.certificatepercentagecompletion)
                                                                {

                                                                }
                                                                else if (Convert.ToInt32(CategoryDetails._certificatePercentage) == CategoryDetails.certificatepercentagecompletion)
                                                                {
                                                                    //test 

                                                                    string attempt = CategoryDetails.AttemptNo.ToString();
                                                                    int headingId = Convert.ToInt32(displayCategory.HeadingID);
                                                                    CertificateController certificate = new CertificateController();
                                                                    string c1 = certificate.DownloadeCreateCertificate(CategoryDetails.assessment_ID, CategoryDetails.id_user, attempt, Idorg, headingId);

                                                                }
                                                            }
                                                            else
                                                            {
                                                                string attempt = CategoryDetails.AttemptNo.ToString();
                                                                int headingId = Convert.ToInt32(displayCategory.HeadingID);
                                                                CertificateController certificate = new CertificateController();
                                                                // certificate.DownloadeCreateCertificate(CategoryDetails.assessment_ID, CategoryDetails.id_user, attempt, Idorg, headingId);
                                                                string c1 = certificate.DownloadeCreateCertificatenormal(CategoryDetails.assessment_ID, CategoryDetails.id_user, attempt, CategoryDetails.CategoryName, Idorg, headingId);

                                                            }
                                                        }
                                                    }

                                                    //


                                                    //coins


                                                }
                                            }
                                                //
                                                if (_categoryHeader == "Moodle Courses")
                                                {
                                                string apiUrl = "https://m2ostelearning.com/moodle/webservice/rest/server.php";
                                                string wsFunction = "local_coursecompletionapi_get_user_enrolled_courses";
                                                string moodleWsRestFormat = "json";
                                                string wsToken = "6c45768964f0622acab07c1067f74339";
                                                string userId = content.EMPLOYEEID; 

                                                // Build the URL with query parameters
                                                string requestUrl = $"{apiUrl}?wsfunction={wsFunction}&moodlewsrestformat={moodleWsRestFormat}&wstoken={wsToken}&userid={userId}";
                                                JArray jsonResponse = null;
                                                try
                                                {
                                                    //using (HttpClient client = new HttpClient())
                                                    //{
                                                    //    // Send the GET request synchronously
                                                    //    HttpResponseMessage response = client.GetAsync(requestUrl).Result;

                                                    //    // Ensure the response was successful
                                                    //    response.EnsureSuccessStatusCode();

                                                    //    // Read the response content synchronously
                                                    //    string responseData = response.Content.ReadAsStringAsync().Result;


                                                    //    // Parse the JSON response as a JArray (since it is a JSON array)
                                                    //    jsonResponse = JArray.Parse(responseData);


                                                    //}
                                                    using (HttpClient client = new HttpClient())
                                                    {
                                                        // Send the GET request synchronously
                                                        HttpResponseMessage response = client.GetAsync(requestUrl).Result;

                                                        // Ensure the response was successful
                                                        response.EnsureSuccessStatusCode();

                                                        // Read the response content synchronously
                                                        string responseData = response.Content.ReadAsStringAsync().Result;

                                                        // Debugging: Print the response to check its structure
                                                        Console.WriteLine(responseData);

                                                        // Check if response is an object or an array
                                                        if (responseData.TrimStart().StartsWith("["))
                                                        {
                                                            // Parse as JArray
                                                            jsonResponse = JArray.Parse(responseData);
                                                        }
                                                        else
                                                        {
                                                            // Parse as JObject
                                                            JObject jsonObject = JObject.Parse(responseData);

                                                            // If courses exist as an array inside an object, extract it
                                                            if (jsonObject["courses"] != null)
                                                            {
                                                                jsonResponse = (JArray)jsonObject["courses"];
                                                            }
                                                            else
                                                            {
                                                                Console.WriteLine("Unexpected response format: " + responseData);
                                                            }
                                                        }
                                                    }

                                                }
                                                catch (Exception ex)
                                                {
                                                    ViewBag.ErrorMessage = $"An error occurred: {ex.Message}";

                                                }


                                                
                                                var Data = kp.CalculateCategoryCompletionStatus(CategoryDetails.CategoryID);



                                                int categoryId = CategoryDetails.CategoryID;

                                                List<MoodleAssessmentStatus> moodleAssessmentStatus = kp.GetMoodleAssessmentStatus(CategoryDetails.CategoryID);

                                                MoodleAssessmentStatus validStatus = moodleAssessmentStatus.FirstOrDefault(s => s.Status == "completed" || s.Status == "passed");

                                                if (validStatus != null)
                                                {
                                                    int headingId = Convert.ToInt32(displayCategory.HeadingID);
                                                    string attempt = validStatus.AttemptNumber.ToString();
                                                    string Idorg = validStatus.IdOrganization.ToString();

                                                    CertificateController certificate = new CertificateController();
                                                    string c1 = certificate.DownloadeCreateCertificatenormal(CategoryDetails.assessment_ID, CategoryDetails.id_user, attempt, CategoryDetails.CategoryName, Idorg, headingId);

                                                }




                                                // CategoryDetails.courseCompletion = Data;
                                                // Update the Totalcomplicationper property to 1
                                                if (Data != null)
                                                {
                                                    // Process the data
                                                    if (jsonResponse != null && jsonResponse.Count > 0)
                                                    {
                                                        // Example: Iterate through the array
                                                        foreach (var course in jsonResponse)
                                                        {
                                                            // Compare course ID with Category ID
                                                            //if (course["courseid"] != null &&
                                                            //    course["courseid"].ToString() == CategoryDetails.CategoryID.ToString())
                                                            //if (course["courseid"] != null &&
                                                            //    course["fullname"].ToString() == course["fullname"];
                                                            if (course["courseid"] != null &&
                                                                CategoryDetails.CategoryName == course["fullname"].ToString())
                                                            {
                                                                // Update TotalCompletionPer with the completion status
                                                                if (course["completion"] != null && int.TryParse(course["completion"].ToString(), out int completion))
                                                                {
                                                                    Data.TotalCompletionPer = completion;
                                                                }
                                                            }

                                                        }

                                                        // Pass the array to the view
                                                        // return View(jsonResponse);
                                                    }
                                                    else
                                                    {
                                                        ViewBag.Message = "No courses found for the user.";
                                                        // return View();
                                                    }
                                                     // Assuming Totalcomplicationper is a property of the Data object
                                                }

                                                // Assign the updated data back to the courseCompletion property
                                                CategoryDetails.courseCompletion = Data;

                                            }

                                               //
                                        
                                                else
                                                {
                                               

                                                    var Data = kp.CalculateCategoryCompletionStatus(CategoryDetails.CategoryID);

                                                    CategoryDetails.courseCompletion = Data;
                                                 
                                                }
                                               
                                            }
                                        }
                                        //var _leftQuries = displayCategoryList.Where(x => x.Heading != _categoryHeader).Select(x => x).ToList();
                                        //displayCategoryList1 = _leftQuries;

                                        //displayCategoryList1.Insert(0, displayCategory);
                                    
                                }
                             
                            }

                            ViewBag._index = displayCategoryList.FindIndex(x => x.Heading == _categoryHeader) + 1;
                        }
                    
                    }
                    else
                    {

                        if (displayCategoryList != null)
                        {
                            KPIController kp = new KPIController();
                            foreach (DisplayCategory displayCategory in displayCategoryList)
                            {
                                if (displayCategory.Categories.Count > 0)
                                {
                                    var CategoryHeadingCountData = kp.CategoryHeadersCount(displayCategory.HeadingID);
                                    displayCategory.CategoryHeadingCount = CategoryHeadingCountData;

                                    foreach (details CategoryDetails in displayCategory.Categories)
                                    {

                                        List<Assessment_ID_for_certification> assessment_ID_For_ = new List<Assessment_ID_for_certification>();

                                        if (CategoryDetails.NEXTURL.Contains("@userid"))
                                        {
                                            CategoryDetails.NEXTURL = CategoryDetails.NEXTURL.Replace("@userid", EmpID);
                                        }

                                        using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
                                        {
                                            string sql1 = "SELECT * FROM tbl_assessment_user_assignment a, tbl_assessment b WHERE id_category = " + CategoryDetails.CategoryID + " AND id_user =" + num;
                                            assessment_ID_For_ = new UtilityModel().getUserAssignment(sql1);

                                            string sql2 = string.Empty;

                                            if (assessment_ID_For_.Count == 0)
                                            {
                                                string sql3 = "SELECT c.id_assessment, c.assessment_title  FROM tbl_assessment_categoty_mapping a, tbl_category b, tbl_assessment c WHERE a.id_category = b.id_category AND a.id_assessment = c.id_assessment AND b.id_organization = " + CategoryDetails.OrganisationId + " and b.status = 'A' and b.id_category =" + CategoryDetails.CategoryID;
                                                assessment_ID_For_ = new UtilityModel().getAssessmentCategotyMapping(sql3);
                                            }
                                            if (assessment_ID_For_.Count > 0)
                                            {
                                                sql2 = " select MAX(attempt_number) attempt_number from tbl_rs_type_qna where id_user = " + num + " and id_assessment = " + assessment_ID_For_.FirstOrDefault().id_assessment + "";
                                                //sql2 = "Select MAX(AttemptNo) AttemptNo from tbl_user_kpi_data_log where id_user = " + num + " and Content_Assessment_ID =" + assessment_ID_For_.FirstOrDefault().id_assessment + " and KPI_Name like '%Mastery Score%'";
                                                var _count = new UtilityModel().getScore(sql2, 1);

                                                ////|| CategoryDetails.CategoryHeader.ToString().Contains("Genesis")

                                                if (_count.attempt_no > 1 || CategoryDetails.CategoryHeader.ToString().Contains("Ethics"))
                                                {
                                                    sql2 = "Select MAX(scoring_value) scoring_value, AttemptNo as attempt_number from tbl_user_kpi_data_log where id_user = " + num + " and Content_Assessment_ID = " + assessment_ID_For_.FirstOrDefault().id_assessment + " and KPI_Name like '%Mastery Score%' and AttemptNo >= 1 order by scoring_value DESC;";
                                                    var _score = new UtilityModel().getScore(sql2, 2);

                                                    if (string.IsNullOrEmpty(_score.scoring_value.ToString()))
                                                    {
                                                        sql2 = " Select (result_in_percentage) scoring_value,  attempt_number from tbl_rs_type_qna where id_user = " + num + " and id_assessment = " + assessment_ID_For_.FirstOrDefault().id_assessment + " and attempt_number >= 1  order by  result_in_percentage DESC limit 1;";
                                                        _score = new UtilityModel().getScore(sql2, 2);
                                                    }

                                                    CategoryDetails._certificatePercentage = _score.scoring_value.ToString();
                                                    _count.attempt_no = _score.attempt_no;
                                                  


                                                }
                                                else if (_count.attempt_no == 1)
                                                {
                                                    CategoryDetails._certificatePercentage = "Your 2nd attempt is pending";
                                                }
                                                CategoryDetails.assessment_ID = assessment_ID_For_.FirstOrDefault().id_assessment;

                                                CategoryDetails.id_user = num;
                                                CategoryDetails.AttemptNo = _count.attempt_no;
                                                string Idorg = CategoryDetails.OrganisationId.ToString();
                                                // For downlode the certificate

                                                if (int.TryParse(CategoryDetails._certificatePercentage, out int certificatePercentage))
                                                {
                                                    // Define SQL query with parameters
                                                    string sql12 = $"SELECT certificate_percentage FROM tbl_assessment WHERE id_assessment = {CategoryDetails.assessment_ID} ORDER BY id_assessment DESC LIMIT 1;";

                                                    // Fetch certificate percentage
                                                    int? certificatePercentageValue = Convert.ToInt32(new UtilityModel().getCertificatepercentage(sql12));

                                                    CategoryDetails.certificatepercentagecompletion = certificatePercentageValue.Value;


                                                    if (Convert.ToInt32(CategoryDetails._certificatePercentage) < CategoryDetails.certificatepercentagecompletion)
                                                    {
                                                    }
                                                    else
                                                    {
                                                        if (assessment_ID_For_.FirstOrDefault().id_assessment == 468 || assessment_ID_For_.FirstOrDefault().id_assessment == 474 || CategoryDetails.CategoryHeader.ToString().Contains("Ethics"))
                                                        {
                                                            if (Convert.ToInt32(CategoryDetails._certificatePercentage) < CategoryDetails.certificatepercentagecompletion)
                                                            {

                                                            }
                                                            else if (Convert.ToInt32(CategoryDetails._certificatePercentage) == CategoryDetails.certificatepercentagecompletion)
                                                            {
                                                                string attempt = CategoryDetails.AttemptNo.ToString();
                                                                int headingId = Convert.ToInt32(displayCategory.HeadingID);
                                                                CertificateController certificate = new CertificateController();
                                                                // certificate.DownloadeCreateCertificate(CategoryDetails.assessment_ID, CategoryDetails.id_user, attempt, Idorg, headingId);
                                                                string c1 = certificate.DownloadeCreateCertificate(CategoryDetails.assessment_ID, CategoryDetails.id_user, attempt, Idorg, headingId);

                                                            }
                                                        }
                                                        else
                                                        {
                                                            string attempt = CategoryDetails.AttemptNo.ToString();
                                                            int headingId = Convert.ToInt32(displayCategory.HeadingID);
                                                            CertificateController certificate = new CertificateController();
                                                            // certificate.DownloadeCreateCertificate(CategoryDetails.assessment_ID, CategoryDetails.id_user, attempt, Idorg, headingId);
                                                            string c1 = certificate.DownloadeCreateCertificatenormal(CategoryDetails.assessment_ID, CategoryDetails.id_user, attempt, CategoryDetails.CategoryName, Idorg, headingId);

                                                        }
                                                    }
                                                }

                                                //


                                            }
                                        }

                                        var Data = kp.CalculateCategoryCompletionStatus(CategoryDetails.CategoryID);
                                        ////Data.StartDate = Data.StartDate != "" ? Convert.ToDateTime(Data.StartDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";
                                        ////Data.ExpiryDate = Data.ExpiryDate != "" ? Convert.ToDateTime(Data.ExpiryDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "";
                                        CategoryDetails.courseCompletion = Data;
                                    }
                                    ////if (displayCategory.Categories[0].NEXTURL.Contains("@userid"))
                                    ////{
                                    ////    displayCategory.Categories[0].NEXTURL = displayCategory.Categories[0].NEXTURL.Replace("@userid", EmpID);
                                    ////}
                                }

                                ViewBag._index = 1;
                                break;
                            }
                        }
                    }
                    if (displayCategoryList.Any())
                    {
                        var _first = displayCategoryList.First();
                        _first.NEXTAPIURL = (string)NEXTAPI;
                    }
                    else
                    {
                        // Handle the case where displayCategoryList is empty
                        // For example, you could log a message or take appropriate action.
                    }
                }
                if (displayCategoryList != null)
                {
                    foreach (var category in displayCategoryList)
                    {
                        foreach (var item in category.Categories)
                        {
                            int? assessmentIdNullable = item.assessment_ID;

                            // Check if the nullable int has a value before converting it to a non-nullable int
                            if (assessmentIdNullable.HasValue)
                            {
                                int assessmentId = assessmentIdNullable.Value;

                                var SqlQuery2 = "select * from tbl_coins_master where Id_organization =" + item.OrganisationId + " AND id_assessment =" + assessmentId + " AND status ='A'";
                                List<tbl_coins_master> resultList1 = new UtilityModel().GetRecordDataCoinMaster(SqlQuery2);
                                if (resultList1.Count >= 0)
                                {

                                    Session["productsIT"] = "Yes";

                                }
                                else
                                {
                                    Session["productsIT"] = null;
                                }
                            }
                            else
                            {

                            }
                        }
                    }
                }
           


                this.ViewData["obj"] = (object)displayCategoryList;
                this.ViewData["userName"] = (object)new UtilityModel().getUserName(num);
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, num);
                this.ViewData["Colorlist"] = (object)JsonConvert.DeserializeObject<RootObjectcolor>(new category().GetColorApi(APIString.API + "getColorConfig?orgID=" + (object)org_id));

                //string catidNew =
                //   ProductTourinfo(CategoryID.ToString());
                if (this.TempData["loginnotification"] != null)
                    this.TempData["Loginnotify"] = (object)"true";
                else
                    this.TempData["Loginnotify"] = (object)"false";
                return (ActionResult)this.View();
            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        /// For the Search bar

       /// Your existing action for search
        public ActionResult Searchbar(string searchTerm, string categoryIds)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int user_id = 0;

            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                user_id = Convert.ToInt32(content.ID_USER);
            }

            // Call SearchbarList to fetch data
            var Searchresult = new UtilityModel().SearchbarList(org_id, categoryIds, searchTerm);

            // Return data as JSON
            return Json(Searchresult, JsonRequestBehavior.AllowGet);

        }

       // Example of a search method that processes both searchTerm and categoryIds
        //private IEnumerable<YourSearchResult> YourSearchMethod(string searchTerm, int[] categoryIds)
        //{
        //    // Your search logic goes here, e.g., query the database using searchTerm and categoryIds
        //    // For example:
        //    var results = db.Products
        //                    .Where(p => p.Name.Contains(searchTerm) && categoryIds.Contains(p.CategoryID))
        //                    .ToList();

        //    return results;
        //}
        /// <returns></returns>




        [HttpPost]
        public JsonResult ReturnContentRead(int CategoryID, int AssId)
        {
            int IsContentRead = 0; string LastAttemptGap = ""; double CourseCompletedPer = 0;
            KPIController kp = new KPIController();
            var ContentReadDetailsData = kp.ContentReadDetailsByCategory(CategoryID);
            var ContentDetailsByCategoryData = kp.ContentDetailsByCategory(CategoryID);
            //var AttemptDetails = kp.GetUserMaxAttemptDetails(AssId);

            //if (AttemptDetails != null)
            //{
            //    int AtemptNo = Convert.ToInt32(AttemptDetails.Attemp_No);
            //    DateTime dtLastAttempt = Convert.ToDateTime(AttemptDetails.LastAttempt);

            //    if (dtLastAttempt != null && AtemptNo<=1)
            //    {
            //        if ((DateTime.Now - dtLastAttempt).TotalHours <24)
            //        {
            //            LastAttemptGap = "Try After " + (dtLastAttempt.AddHours(24)).ToString("dd-MM-yyyy HH:mm") ;
            //        }
            //    }
            //}
            if (ContentReadDetailsData != null && ContentDetailsByCategoryData != null && ContentReadDetailsData.Count > 0 && ContentDetailsByCategoryData.Count > 0)
            {
                int TotalContentLength = Convert.ToInt32(ContentDetailsByCategoryData.FirstOrDefault().COUNT_REQUIRED);
                int ReadContentLength = Convert.ToInt32(ContentReadDetailsData.FirstOrDefault().COUNT_REQUIRED);

                CourseCompletedPer = (ReadContentLength / TotalContentLength) * 100;

                if (Convert.ToInt32(CourseCompletedPer) == 100)
                {
                    IsContentRead = 1;
                }
            }
            return Json(new { TotalContent = IsContentRead, Duration = LastAttemptGap });
        }
        public ActionResult ProductViewDetails(int CategoryID)
        {
            try
            {
                System.Web.HttpContext.Current.Session["ContentSession"] = (object)null;
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                string EmpID = "";
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                    EmpID = content.EMPLOYEEID;
                }
                CategroyDashboard categroyDashboard1 = (CategroyDashboard)null;
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "getCategoryDashboard?catid=" + (object)CategoryID + "&orgid=" + (object)org_id + "&userid=" + (object)user_id));
                if (apiresponse.KEY == "SUCCESS")
                {
                    CategroyDashboard categroyDashboard2 = new CategroyDashboard();
                    categroyDashboard1 = JsonConvert.DeserializeObject<CategroyDashboard>(apiresponse.MESSAGE);
                }
                List<content_list_session> contentListSessionList = new List<content_list_session>();
                if (categroyDashboard1 != null)
                {
                    int num = 0;
                    foreach (SearchResponce searchResponce in categroyDashboard1.CONTENTLIST)
                    {
                        contentListSessionList.Add(new content_list_session()
                        {
                            content_index = num,
                            prev_con_id = num != 0 ? categroyDashboard1.CONTENTLIST[num - 1].ID_CONTENT : 0,
                            next_con_id = num != categroyDashboard1.CONTENTLIST.Count - 1 ? categroyDashboard1.CONTENTLIST[num + 1].ID_CONTENT : 0,
                            CategoryID = CategoryID,
                            total_count = categroyDashboard1.CONTENTLIST.Count
                        });
                        ++num;
                    }

                    //foreach (AssessmentList assessmentDetails in categroyDashboard1.ASSESSMENTLIST)
                    //{
                    //    #region Course Completion Check

                    //    double CourseCompletedPer = 0;
                    //    int IsCourseCompleted = 0, IsContentRead = 0;
                    //    string LastAttemptGap = "";
                    //    KPIController kp = new KPIController();

                    //    var CategoryCompletionStatusData = kp.CalculateCategoryCompletionStatus(CategoryID);

                    //    //Content Read Completion Per
                    //    var ContentReadDetailsData = kp.ContentReadDetailsByCategory(CategoryID);
                    //    var ContentDetailsByCategoryData = kp.ContentDetailsByCategory(CategoryID);
                    //    //var AttemptDetails = kp.GetUserMaxAttemptDetails(assessmentDetails.id_assessment);

                    //    //if (AttemptDetails != null)
                    //    //{
                    //    //    int AtemptNo = Convert.ToInt32(AttemptDetails.Attemp_No);
                    //    //    DateTime dtLastAttempt = Convert.ToDateTime(AttemptDetails.LastAttempt);

                    //    //    if (dtLastAttempt != null && AtemptNo <= 1)
                    //    //    {
                    //    //        if ((DateTime.Now - dtLastAttempt).TotalHours < 24)
                    //    //        {
                    //    //            LastAttemptGap = "Try After " + (dtLastAttempt.AddHours(24)).ToString("dd-MM-yyyy HH:mm");
                    //    //        }
                    //    //    }
                    //    //}

                    //    if (CategoryCompletionStatusData != null)
                    //    {
                    //        CourseCompletedPer = CategoryCompletionStatusData.TotalCompletionPer;

                    //        if (Convert.ToInt32(CourseCompletedPer) == 100)
                    //        {
                    //            IsCourseCompleted = 1;
                    //        }
                    //    }

                    //    if (ContentReadDetailsData != null && ContentDetailsByCategoryData != null && ContentReadDetailsData.Count > 0 && ContentDetailsByCategoryData.Count > 0)
                    //    {
                    //        int TotalContentLength = Convert.ToInt32(ContentDetailsByCategoryData.FirstOrDefault().COUNT_REQUIRED);
                    //        int ReadContentLength = Convert.ToInt32(ContentReadDetailsData.FirstOrDefault().COUNT_REQUIRED);

                    //        CourseCompletedPer = (ReadContentLength / TotalContentLength) * 100;

                    //        if (Convert.ToInt32(CourseCompletedPer) == 100)
                    //        {
                    //            IsContentRead = 1;
                    //        }
                    //    }

                    //    assessmentDetails.IsCourseCompleted = IsCourseCompleted;
                    //    assessmentDetails.IsContentRead = IsContentRead;

                    //    #endregion

                    //    if (assessmentDetails.answer_description.Contains("@userid"))
                    //    {
                    //        assessmentDetails.answer_description = assessmentDetails.answer_description.Replace("@userid", EmpID);
                    //    }

                    //    if (assessmentDetails.answer_description.Contains("@asid"))
                    //    {
                    //        assessmentDetails.answer_description = assessmentDetails.answer_description.Replace("@asid", Convert.ToString(assessmentDetails.id_assessment));
                    //    }
                    //}

                    foreach (AssessmentList assessmentDetails in categroyDashboard1.ASSESSMENTLIST)
                    {
                        #region Course Completion Check

                        double CourseCompletedPer = 0;
                        int IsCourseCompleted = 0, IsContentRead = 0;
                        string LastAttemptGap = "";
                        KPIController kp = new KPIController();

                        var CategoryCompletionStatusData = kp.CalculateCategoryCompletionStatus(CategoryID);

                        //Content Read Completion Per
                        var ContentReadDetailsData = kp.ContentReadDetailsByCategory(CategoryID);
                        var ContentDetailsByCategoryData = kp.ContentDetailsByCategory(CategoryID);
                        //var AttemptDetails = kp.GetUserMaxAttemptDetails(assessmentDetails.id_assessment);

                        //if (AttemptDetails != null)
                        //{
                        //    int AtemptNo = Convert.ToInt32(AttemptDetails.Attemp_No);
                        //    DateTime dtLastAttempt = Convert.ToDateTime(AttemptDetails.LastAttempt);

                        //    if (dtLastAttempt != null && AtemptNo <= 1)
                        //    {
                        //        if ((DateTime.Now - dtLastAttempt).TotalHours < 24)
                        //        {
                        //            LastAttemptGap = "Try After " + (dtLastAttempt.AddHours(24)).ToString("dd-MM-yyyy HH:mm");
                        //        }
                        //    }
                        //}

                        if (CategoryCompletionStatusData != null)
                        {
                            CourseCompletedPer = CategoryCompletionStatusData.TotalCompletionPer;

                            if (Convert.ToInt32(CourseCompletedPer) == 100)
                            {
                                IsCourseCompleted = 1;
                            }
                        }

                        if (ContentReadDetailsData != null && ContentDetailsByCategoryData != null && ContentReadDetailsData.Count > 0 && ContentDetailsByCategoryData.Count > 0)
                        {
                            int TotalContentLength = Convert.ToInt32(ContentDetailsByCategoryData.FirstOrDefault().COUNT_REQUIRED);
                            int ReadContentLength = Convert.ToInt32(ContentReadDetailsData.FirstOrDefault().COUNT_REQUIRED);

                            CourseCompletedPer = (ReadContentLength / TotalContentLength) * 100;

                            if (Convert.ToInt32(CourseCompletedPer) == 100)
                            {
                                IsContentRead = 1;
                            }
                        }

                        assessmentDetails.IsCourseCompleted = IsCourseCompleted;
                        assessmentDetails.IsContentRead = IsContentRead;

                        #endregion

                        // For the comma
                        //string input = assessmentDetails.assessment_name;
                        //char[] delimiter = { ',' };

                        //// Remove the first character if it is a comma
                        //if (input.StartsWith(","))
                        //{
                        //    input = input.Substring(1);
                        //}

                        //// Now split the string based on a delimiter
                        //string[] parts = input.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);

                        //// Display the split parts
                        //foreach (string part in parts)
                        //{
                        //    assessmentDetails.assessment_name = part;
                        //}






                        // Decode the URL if it contains encoded characters
                        if (assessmentDetails.answer_description.Contains("%"))
                        {
                            string decodedUrl = HttpUtility.UrlDecode(assessmentDetails.answer_description);

                            // string decodedUrl1 = Uri.UnescapeDataString(decodedUrl);
                            string decodedUrl2 = DecodeUrl(decodedUrl);

                            string decodedUrl3 = Uri.UnescapeDataString(decodedUrl2);

                            // string decodedUrl2 = DecodeUrl(decodedUrl1);

                            //Uri.UnescapeDataString(encodedUrl);
                            assessmentDetails.answer_description = decodedUrl3;
                        }

                        // Check for placeholders and replace them with appropriate values
                        if (assessmentDetails.answer_description.Contains("@userid"))
                        {
                            assessmentDetails.answer_description = assessmentDetails.answer_description.Replace("@userid", EmpID);
                        }

                        if (assessmentDetails.answer_description.Contains("@asid"))
                        {
                            assessmentDetails.answer_description = assessmentDetails.answer_description.Replace("@asid", Convert.ToString(assessmentDetails.id_assessment));
                        }
                        else
                        {
                            // Construct redirection URL with additional parameters
                            string GameUrl = assessmentDetails.answer_description;
                            string M2ostAssessmentId = Convert.ToString(assessmentDetails.id_assessment);
                            string redirectionUrl = $"{GameUrl}&Email={EmpID}&M2ostAssessmentId={M2ostAssessmentId}";

                            // Update the answer_description with the redirection URL
                            assessmentDetails.answer_description = redirectionUrl;
                        }
                    }
                    System.Web.HttpContext.Current.Session["ContentSession"] = (object)contentListSessionList;
                }

                UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

                if (userSession != null)
                {
                    // Extract ID_USER and id_ORGANIZATION from the userSession object
                    string ID_USER = userSession.ID_USER;
                    int id_ORGANIZATION = userSession.id_ORGANIZATION;
                    string Page = categroyDashboard1.CATEGORY.CategoryName;
                    // Call AddUserDataLog method with extracted values
                    new UtilityModel().AddUserDataLog(ID_USER, id_ORGANIZATION, Page);
                }


                this.ViewData["Colorlist"] = (object)JsonConvert.DeserializeObject<RootObjectcolor>(new category().GetColorApi(APIString.API + "getColorConfig?orgID=" + (object)org_id));
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                foreach (var item in categroyDashboard1.ASSESSMENTLIST)
                {
                    string url = item.answer_description;
                    string url1 = HttpUtility.UrlEncode(item.answer_description);
                }
                this.ViewData["obj"] = (object)categroyDashboard1;

                FeedbackGetData();

                return (ActionResult)this.View();
            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }


        static string DecodeUrl(string encodedUrl)
        {
            // Create a variable to store the decoded URL
            string decodedUrl = encodedUrl;

            // Loop until there are no '%' characters left in the decoded URL
            while (decodedUrl.Contains('%'))
            {
                // Decode the URL
                decodedUrl = HttpUtility.UrlDecode(decodedUrl);
            }

            return decodedUrl;
        }


        public ActionResult ProductQueAns(int ContentID, int CategoryID, string message)
        {
            //this is first
            try
            {
                List<content_list_session> contentListSessionList1 = (List<content_list_session>)this.HttpContext.Session.Contents["ContentSession"];
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                AnswerResponse answerResponse = JsonConvert.DeserializeObject<AnswerResponse>(new UtilityModel().getApiResponseString(APIString.API + "GetContentDetails?conId=" + (object)ContentID + "&userid=" + (object)user_id + "&orgid=" + (object)org_id));

                ////double videotimer = new UtilityModel().GetVideoTimer("select video_timer from tbl_prod_que_ans_video where ContentID=" + ContentID + " and CategoryID=" + CategoryID + " and user_id=" + user_id);
                double videotimer = new UtilityModel().GetVideoTimer("select video_timer from tbl_prod_que_ans_video where ContentID=" + ContentID + " and CategoryID=" + CategoryID + " and user_id=" + user_id + " ORDER BY ID DESC LIMIT 1");
                answerResponse.video_timer = videotimer;
                answerResponse.ID_ORGANIZATION = org_id;

                //for test commentout
                //using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
                //    answerResponse.video_timer = m2ostDbContext.Database.SqlQuery<double>("select video_timer from tbl_prod_que_ans_video where ContentID={0} and CategoryID={1} and user_id={2}", (object)ContentID, (object)CategoryID, (object)user_id).FirstOrDefault<double>();
                /////

                string apiResponseString = new UtilityModel().getApiResponseString(APIString.API + "Satisfied?answerID=" + (object)answerResponse.ID_CONTENT_ANSWER + "&oid=" + (object)org_id + "&uid=" + (object)user_id);
                List<AssessmentList> assessmentListList = (List<AssessmentList>)null;
                if (answerResponse.ASSESSMENT_FLAG == "1")
                    assessmentListList = JsonConvert.DeserializeObject<List<AssessmentList>>(new UtilityModel().getApiResponseString(APIString.API + "AssessmentList?CID=" + (object)ContentID + "&UID=" + (object)user_id + "&OID=" + (object)org_id));
                List<SatisfiedResult> satisfiedResultList = JsonConvert.DeserializeObject<List<SatisfiedResult>>(apiResponseString);

                //for moodel
                if (satisfiedResultList != null)
                {
                    string pathmoodle = satisfiedResultList[0].PATH;


                    bool isMoodlePresent = pathmoodle.ToLower().Contains("moodle".ToLower());

                    if (isMoodlePresent)
                    {

                        pathmoodle = pathmoodle.Replace("@orgid", content.id_ORGANIZATION.ToString());
                        pathmoodle = pathmoodle.Replace("@userid", content.USERID);
                        pathmoodle = pathmoodle.Replace("@iduser", content.ID_USER);


                        SatisfiedResult newResult = new SatisfiedResult
                        {
                            PATH = pathmoodle,

                        };


                        satisfiedResultList[0].PATH = pathmoodle;


                    }
                    string pathmoodle_1 = satisfiedResultList[0].PATH;
                    bool isMoodlePresent_1 = pathmoodle.ToLower().Contains("moodle".ToLower());

                    if (isMoodlePresent_1)
                    {
                       

                        // Deserialize the JSON response into a list of SatisfiedResult
                        try
                        {
                            string rawResponse = new UtilityModel().getApiResponseString(pathmoodle_1);

                            string unescapedJson;
                            // Step 1: Determine the structure of the rawResponse
                            if (rawResponse.StartsWith("{") && rawResponse.Contains("\\"))
                            {
                                // Case 2: JSON-encoded string
                                unescapedJson = rawResponse;
                            }
                            else
                            {
                                // Case 1: Direct JSON object
                                unescapedJson = JsonConvert.DeserializeObject<string>(rawResponse);
                               
                            }

                            // Step 2: Parse the unescaped JSON string into a JObject
                            JObject jsonObject = JObject.Parse(unescapedJson);

                            // Step 3: Convert it back to a formatted JSON string
                            string formattedJson = jsonObject.ToString(Formatting.Indented);
                            string loginUrl = jsonObject["loginurl"]?.ToString();
                            this.ViewData["LinkToOpenmoodel"] = loginUrl;



                        }
                        catch
                        {

                        }
                    }
                }

                int num1 = 0;
                if (satisfiedResultList != null)
                    num1 = satisfiedResultList.Count;
                List<NewAnswerSteps> newAnswerStepsList = JsonConvert.DeserializeObject<List<NewAnswerSteps>>(new UtilityModel().getApiResponseString(APIString.API + "CalltoAction?answerID=" + (object)answerResponse.ID_CONTENT_ANSWER + "&orgID=" + (object)org_id + "&uid=" + (object)user_id));
                int num2 = 0;
                if (newAnswerStepsList != null)
                    num2 = newAnswerStepsList.Count;
                RootObjectcolor rootObjectcolor = JsonConvert.DeserializeObject<RootObjectcolor>(new category().GetColorApi(APIString.API + "getColorConfig?orgID=" + (object)org_id));
                CategroyDashboard categroyDashboard1 = (CategroyDashboard)null;
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "getCategoryDashboard?catid=" + (object)CategoryID + "&orgid=" + (object)org_id + "&userid=" + (object)user_id));
                if (apiresponse.KEY == "SUCCESS")
                {
                    CategroyDashboard categroyDashboard2 = new CategroyDashboard();
                    categroyDashboard1 = JsonConvert.DeserializeObject<CategroyDashboard>(apiresponse.MESSAGE);
                }
                List<content_list_session> contentListSessionList2 = new List<content_list_session>();
                if (categroyDashboard1 != null)
                {
                    int num3 = 0;
                    foreach (SearchResponce searchResponce in categroyDashboard1.CONTENTLIST)
                    {
                        contentListSessionList2.Add(new content_list_session()
                        {
                            content_index = num3,
                            prev_con_id = num3 != 0 ? categroyDashboard1.CONTENTLIST[num3 - 1].ID_CONTENT : 0,
                            next_con_id = num3 != categroyDashboard1.CONTENTLIST.Count - 1 ? categroyDashboard1.CONTENTLIST[num3 + 1].ID_CONTENT : 0,
                            CategoryID = CategoryID,
                            total_count = categroyDashboard1.CONTENTLIST.Count
                        });
                        ++num3;
                    }
                    System.Web.HttpContext.Current.Session["ContentSession"] = (object)contentListSessionList2;
                    contentListSessionList1 = contentListSessionList2;
                }
                int num4 = 0;
                int index = 0;
                foreach (SearchResponce searchResponce in categroyDashboard1.CONTENTLIST)
                {
                    if (ContentID == searchResponce.ID_CONTENT)
                        index = num4;
                    ++num4;
                }
                content_list_session contentListSession = new content_list_session();
                this.ViewData["con_info"] = (object)contentListSessionList1[index];
                this.ViewData["Colorlist"] = (object)rootObjectcolor;
                this.ViewData[nameof(message)] = (object)message;
                this.ViewData["obj"] = (object)answerResponse;
                this.ViewData["Satisfied"] = (object)satisfiedResultList;
                this.ViewData["AssessmentList"] = (object)assessmentListList;
                this.ViewData["sCount"] = (object)num1;
                this.ViewData["StepCount"] = (object)num2;
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                return (ActionResult)this.View();

            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        /// <summary>
        private string GetNewLoginUrlFromApi(string apiEndpoint)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    // Step 1: Send a GET request to the provided API endpoint
                    var response = client.GetAsync(apiEndpoint).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Step 2: Parse the response JSON to extract the login URL
                        string jsonResponse = response.Content.ReadAsStringAsync().Result;
                        var jsonObject = JObject.Parse(jsonResponse);

                        // Extract the login URL
                        return jsonObject["loginurl"]?.ToString();
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    Console.WriteLine($"Error fetching new login key: {ex.Message}");
                }
            }

            return null; // Return null if key generation fails
        }
        ///
        public void ProductQueAns_VideoUpdate(int ContentID, int CategoryID, double VideoTimer, int IsCompleted, int OrgID, double CompletePer, double TotalVideoTimer)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int user_id = 0;
                if (content != null)
                {
                    user_id = Convert.ToInt32(content.ID_USER);
                }

                double videotimer = new UtilityModel().GetVideoTimer("select video_timer from tbl_prod_que_ans_video where ContentID=" + ContentID + " and CategoryID=" + CategoryID + " and user_id=" + user_id + " ORDER BY ID DESC LIMIT 1");
                int IsVideoCompleted = new UtilityModel().IsVideoCompleted("select is_completed from tbl_prod_que_ans_video where ContentID=" + ContentID + " and CategoryID=" + CategoryID + " and user_id=" + user_id + " AND is_completed=1 ORDER BY ID DESC LIMIT 1");

                //////if (IsVideoCompleted == 0)
                //////{
                if (videotimer > 0)
                {
                    ////new UtilityModel().UpdateVideoTimer(ContentID, CategoryID, user_id, VideoTimer);
                    new UtilityModel().InsertVideoTimer(ContentID, CategoryID, user_id, VideoTimer, IsCompleted, OrgID, CompletePer, TotalVideoTimer);
                }
                else
                {
                    new UtilityModel().InsertVideoTimer(ContentID, CategoryID, user_id, VideoTimer, IsCompleted, OrgID, CompletePer, TotalVideoTimer);
                }
                //////}
            }
            catch (Exception ex)
            {
                ////return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult AnswerStep(int ID_CONTENT_ANSWER, int ContentID, string message)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                AnswerResponse answerResponse = JsonConvert.DeserializeObject<AnswerResponse>(new UtilityModel().getApiResponseString(APIString.API + "GetContentDetails?conId=" + (object)ContentID + "&userid=" + (object)user_id + "&orgid=" + (object)org_id));
                this.ViewData["obj"] = (object)JsonConvert.DeserializeObject<List<NewAnswerSteps>>(new UtilityModel().getApiResponseString(APIString.API + "CalltoAction?answerID=" + (object)ID_CONTENT_ANSWER + "&orgID=" + (object)org_id + "&uid=" + (object)user_id));
                this.ViewData["additional"] = (object)answerResponse;
                List<SatisfiedResult> satisfiedResultList = JsonConvert.DeserializeObject<List<SatisfiedResult>>(new UtilityModel().getApiResponseString(APIString.API + "Satisfied?answerID=" + (object)ID_CONTENT_ANSWER + "&oid=" + (object)org_id + "&uid=" + (object)user_id));
                int num = 0;
                if (satisfiedResultList != null)
                    num = satisfiedResultList.Count;
                this.ViewData["sCount"] = (object)num;
                this.ViewData[nameof(message)] = (object)message;
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                return (ActionResult)this.View();
            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        //public ActionResult LinksView(int ID_CONTENT_ANSWER)
        //{
        //    try
        //    {
        //        UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //        int org_id = 0;
        //        int user_id = 0;
        //        string UserID = "";
        //        if (content != null)
        //        {
        //            org_id = Convert.ToInt32(content.id_ORGANIZATION);
        //            user_id = Convert.ToInt32(content.ID_USER);
        //            UserID = content.USERID;
        //        }

        //        var LinksData = JsonConvert.DeserializeObject<List<SatisfiedResult>>(new UtilityModel().getApiResponseString(APIString.API + "Satisfied?answerID=" + (object)ID_CONTENT_ANSWER + "&oid=" + (object)org_id + "&uid=" + (object)user_id));

        //        if (LinksData != null)
        //        {
        //            for (var i = 0; i < LinksData.Count; i++)
        //            {
        //                if (LinksData[i].PATH != "")
        //                {
        //                    LinksData[i].PATH = LinksData[i].PATH.Replace("@userid", UserID);
        //                }
        //            }
        //        }

        //        this.ViewData["Links"] = LinksData;
        //        this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
        //        return (ActionResult)this.View();
        //    }
        //    catch
        //    {
        //        return (ActionResult)this.RedirectToAction("error");
        //    }
        //}


        public class MoodleResponse
        {
            public string loginurl { get; set; }
            public int id_user { get; set; }
            public string userid { get; set; }
            public int assessment_id { get; set; }
            public int organization_id { get; set; }
        }


        public ActionResult LinksView(int ID_CONTENT_ANSWER)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                string UserID = "";
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                    UserID = content.USERID;
                }

                var LinksData = JsonConvert.DeserializeObject<List<SatisfiedResult>>(new UtilityModel().getApiResponseString(APIString.API + "Satisfied?answerID=" + (object)ID_CONTENT_ANSWER + "&oid=" + (object)org_id + "&uid=" + (object)user_id));
                //for moodel
                string pathmoodle = LinksData[0].PATH;


                bool isMoodlePresent = pathmoodle.ToLower().Contains("moodle".ToLower());

                if (isMoodlePresent)
                {

                    pathmoodle = pathmoodle.Replace("@orgid", content.id_ORGANIZATION.ToString());
                    pathmoodle = pathmoodle.Replace("@userid", content.USERID);
                    pathmoodle = pathmoodle.Replace("@iduser", content.ID_USER);


                    SatisfiedResult newResult = new SatisfiedResult
                    {
                        PATH = pathmoodle,

                    };


                    LinksData[0].PATH = pathmoodle;


                }

                if (LinksData != null)
                {
                    for (var i = 0; i < LinksData.Count; i++)
                    {
                        string pathmoodle_1 = LinksData[0].PATH;
                        bool isMoodlePresent_1 = pathmoodle.ToLower().Contains("moodle".ToLower());

                        if (isMoodlePresent_1)
                        {


                            // Deserialize the JSON response into a list of SatisfiedResult
                            try
                            {
                                string rawResponse = new UtilityModel().getApiResponseString(pathmoodle_1);

                                string unescapedJson = JsonConvert.DeserializeObject<string>(rawResponse);

                                // Step 2: Parse the unescaped JSON string into a JObject
                                JObject jsonObject = JObject.Parse(unescapedJson);

                                // Step 3: Convert it back to a formatted JSON string
                                string formattedJson = jsonObject.ToString(Formatting.Indented);
                                string loginUrl = jsonObject["loginurl"]?.ToString();
                                this.ViewData["LinkToOpen"] = loginUrl;

                                return View();


                            }
                            catch (JsonReaderException ex)
                            {
                                Console.WriteLine("JSON Deserialization Error: " + ex.Message);
                                // Log the invalid response
                            }
                        }


                        if (LinksData[i].TYPE == "3")
                        {
                            for (int k = 0; k < LinksData.Count; k++)
                            {
                                //var linkOpener = new LinkOpener();
                                //linkOpener.OpenUrl(LinksData[i].PATH);

                                //return Redirect(pdfUrl);

                                this.ViewData["LinkToOpen"] = LinksData[i].PATH;
                                return View();

                            }

                        }
                        else
                        {
                            if (LinksData[i].PATH != "")
                            {
                                LinksData[i].PATH = LinksData[i].PATH.Replace("@userid", UserID);

                            }

                            string pdfUrl = LinksData[0].PATH;

                            string extension = Path.GetExtension(pdfUrl);
                            if (extension.Equals(".pdf", StringComparison.OrdinalIgnoreCase))
                            {
                                Session["pdfUrlTempSession"] = pdfUrl;


                                this.ViewData["Links"] = LinksData;
                                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                                // return (ActionResult)this.View();
                                return RedirectToAction("ViewShowPdf", "Dashboard", new { PdfUrl = pdfUrl, pageNumber = 1 });


                            }

                            return Redirect(pdfUrl);
                        }

                    }
                }

                return RedirectToAction("Index", "Dashboard");



            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public class LinkOpener
        {
            public void OpenUrl(string url)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // This makes sure the URL is opened using the default browser
                });
            }
        }

        /// Pdf viewer  
        public ActionResult ViewShowPdf(string PdfUrl, int pageNumber)
        {
            try
            {
                string pdfUrlSession = Session["pdfUrlTempSession"] as string;

                //string decodedUrl = System.Web.HttpUtility.UrlDecode(pdfUrlSession);

                if (string.IsNullOrEmpty(pdfUrlSession))
                {
                    // Handle the case where pdfUrlSession is null or empty
                    return Content("PDF URL is missing from the session.");
                }

                using (var webClient = new System.Net.WebClient())
                {
                    byte[] pdfBytes = webClient.DownloadData(pdfUrlSession);

                    using (MemoryStream pdfStream = new MemoryStream(pdfBytes))
                    {
                        _pdfDocument = PdfDocument.Load(pdfStream);
                        RenderPdfPage(pageNumber);

                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                // Log the exception or return an error message
                return Content("Error: " + ex.Message);
            }

        }


        public ActionResult NextPage(int currentPage)
        {
            string pdfUrlSession = Session["pdfUrlTempSession"] as string;

            if (_pdfDocument != null && currentPage < _pdfDocument.PageCount)
            {
                RenderPdfPage(currentPage + 1);
            }

            return RedirectToAction("ViewShowPdf", new { PdfUrl = pdfUrlSession, pageNumber = currentPage });

        }

        public ActionResult PreviousPage(int currentPage)
        {
            string pdfUrlSession = Session["pdfUrlTempSession"] as string;

            if (_pdfDocument != null && currentPage > 1)
            {
                RenderPdfPage(currentPage - 1);
            }

            return RedirectToAction("ViewShowPdf", new { PdfUrl = pdfUrlSession, pageNumber = currentPage });


        }

        private void RenderPdfPage(int pageNumber)
        {
            using (var pageImage = _pdfDocument.Render(pageNumber - 1, 96, 96, false))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pageImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    byte[] imageBytes = ms.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    string imageDataUrl = string.Format("data:image/png;base64,{0}", base64String);

                    ViewBag.ImageDataUrl = imageDataUrl;
                    ViewBag.CurrentPage = pageNumber;
                    ViewBag.TotalPages = _pdfDocument.PageCount;
                }
            }
        }
        /// <returns></returns>

        public ActionResult AssessmentView(string NEXTAPI, int CategoryID)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                NEXTAPI = NEXTAPI.Replace('_', '&');
                this.ViewData["obj"] = (object)JsonConvert.DeserializeObject<List<AssessmentList>>(new UtilityModel().getApiResponseString(APIString.RAW + NEXTAPI));
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult AssessmentSheet(int ASID)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }

                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "AssessmentSheet?ASID=" + (object)ASID + "&UID=" + (object)user_id + "&OID=" + (object)org_id));
                IBHFL.Models.AssessmentSheet assessmentSheet = new IBHFL.Models.AssessmentSheet();
                string str = (string)null;
                if (apiresponse.KEY == "SUCCESS")
                    assessmentSheet = JsonConvert.DeserializeObject<IBHFL.Models.AssessmentSheet>(apiresponse.MESSAGE);
                else
                    str = apiresponse.MESSAGE;
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                this.ViewData["message"] = (object)str;
                this.ViewData["result"] = (object)assessmentSheet;
                this.ViewData["asid"] = (object)ASID;
                return (ActionResult)this.View();
            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult ConflictManagementAssessment() => (ActionResult)this.View();

        public ActionResult AssessmentResult()
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                int int32_1 = Convert.ToInt32(this.Request.Form["qtn_count"]);
                int int32_2 = Convert.ToInt32(this.Request.Form["asid"]);
                string str1 = "";
                for (int index = 1; index <= int32_1; ++index)
                    str1 = str1 + this.Request.Form["qna" + (object)index] + ";";
                string str2 = str1.TrimEnd(';').Trim();
                string apiResponseString = new UtilityModel().getApiResponseString(APIString.API + "EvaluateAssessment?UID=" + (object)user_id + "&ASID=" + (object)int32_2 + "&ASRQ=" + str2);
                List<Task> task = new UtilityModel().getTask(org_id, user_id);
                AssessmentResponce assessmentResponce = JsonConvert.DeserializeObject<AssessmentResponce>(apiResponseString);
                // KPIController data = new KPIController(assessmentResponce);
                string str3 = "";
                //if(assessmentResponce.Attempt.Equals(4))
                //{

                //}
                if (assessmentResponce != null)
                {
                    string[] source = assessmentResponce.Message.Split('|');
                    this.ViewData["rs1"] = (object)source[1];
                    for (int index = 2; index < ((IEnumerable<string>)source).Count<string>() - 1; ++index)
                        str3 = str3 + "\n " + source[index];
                    this.ViewData["rs2"] = (object)str3;
                    double num = Math.Round((double)Convert.ToInt32(((IEnumerable<string>)str3.Split(':')).Last<string>()) / (double)assessmentResponce.QuestionAnswer.Count * 100.0, 2);
                    this.ViewData["qtnBody"] = (object)assessmentResponce.QuestionAnswer;
                    this.ViewData["assessment"] = (object)assessmentResponce.Assessment[0];
                    this.ViewData["attemp"] = (object)assessmentResponce.Attempt;
                    this.ViewData["percentage"] = (object)num;
                    this.ViewData["id_ass_sheet"] = (object)assessmentResponce.id_assessment_sheet;
                    this.ViewData["id_ass"] = (object)Convert.ToString(assessmentResponce.id_assessment);
                    this.ViewData["date"] = (object)Convert.ToString(DateTime.Now.Date);
                    this.ViewData["Tasklist"] = (object)task;
                    return (ActionResult)this.View();
                }
                this.ViewData["result"] = (object)null;
                this.ViewData["qtnBody"] = (object)null;
                this.ViewData["assessment"] = (object)null;
                this.ViewData["attempt"] = (object)null;
                this.ViewData["Tasklist"] = (object)task;
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        /// <summary>
        //For the Coin insert
        //public ActionResult Coin(int unique_id,int id_assessment_log,int id_user,int id_organization,int id_assessment,int attempt_number,int total_questions,int right_answers_count,int wrong_answers_count,int set_value,int result_percentage, int coins_scored ,string status)
        //{

        //    new UtilityModel().InsertCoin(unique_id, id_assessment_log, id_user, id_organization, id_assessment, attempt_number, total_questions, right_answers_count, wrong_answers_count, set_value, result_percentage, coins_scored, status);

        //    return this.View();
        //}
        /// <returns></returns>

        public ActionResult MyReportSummary(int LID, int SID, int id_assessmnent = 0, string date = " ")
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                AssessmentResponce assessmentResponce = JsonConvert.DeserializeObject<AssessmentResponce>(new UtilityModel().getApiResponseString(APIString.API + "getMyReportDetails?LID=" + (object)LID + "&SID=" + (object)SID + "&UID=" + (object)user_id + "&OID=" + (object)org_id));
                List<Task> task = new UtilityModel().getTask(org_id, user_id);
                string str = "";
                if (assessmentResponce != null)
                {
                    string[] source = assessmentResponce.Message.Split('|');
                    this.ViewData["rs1"] = (object)source[1];
                    for (int index = 2; index < ((IEnumerable<string>)source).Count<string>() - 1; ++index)
                        str = str + "\n " + source[index];
                    this.ViewData["rs2"] = (object)str;
                    double num = Math.Round((double)Convert.ToInt32(((IEnumerable<string>)str.Split(':')).Last<string>()) / (double)assessmentResponce.QuestionAnswer.Count * 100.0, 2);
                    this.ViewData["qtnBody"] = (object)assessmentResponce.QuestionAnswer;
                    this.ViewData["assessment"] = (object)assessmentResponce.Assessment[0];
                    this.ViewData["attemp"] = (object)assessmentResponce.Attempt;
                    this.ViewData["percentage"] = (object)num;
                    this.ViewData["Tasklist"] = (object)task;
                    this.ViewData["id_ass_sheet"] = (object)assessmentResponce.id_assessment_sheet;
                    this.ViewData["id_ass"] = (object)Convert.ToString(id_assessmnent);
                    this.ViewData[nameof(date)] = (object)date;
                    return (ActionResult)this.View();
                }
                this.ViewData["Tasklist"] = (object)task;
                this.ViewData["result"] = (object)null;
                this.ViewData["qtnBody"] = (object)null;
                this.ViewData["assessment"] = (object)null;
                this.ViewData["attempt"] = (object)null;
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult MyReport(int cid)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                this.ViewData["result"] = (object)JsonConvert.DeserializeObject<IBHFL.Models.MyReport>(new UtilityModel().getApiResponseString(APIString.API + "getMyReports?UID=" + (object)user_id + "&OID=" + (object)org_id + "&FLAG=0&MID=0"));
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult Search(string s)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                List<SearchResponce> searchResponceList1 = JsonConvert.DeserializeObject<List<SearchResponce>>(new UtilityModel().getApiPost(APIString.API + nameof(Search), new NameValueCollection()
        {
          {
            "patternString",
            s
          },
          {
            "Category",
            "0"
          },
          {
            "OrganizationId",
            org_id.ToString()
          },
          {
            "UserId",
            user_id.ToString()
          },
          {
            "AccessRole",
            "0"
          }
        }));
                List<SearchResponce> searchResponceList2 = new List<SearchResponce>();
                foreach (SearchResponce searchResponce1 in searchResponceList1)
                {
                    SearchResponce searchResponce2 = new SearchResponce();
                    using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
                        searchResponce2.ID_Category = m2ostDbContext.Database.SqlQuery<int>("select COALESCE( id_category,0) from tbl_content_organization_mapping where id_content={0} and id_organization={1}", (object)searchResponce1.ID_CONTENT, (object)org_id).FirstOrDefault<int>();
                    searchResponce2.CONTENT_QUESTION = searchResponce1.CONTENT_QUESTION;
                    searchResponce2.EXPIRYDATE = searchResponce1.EXPIRYDATE;
                    searchResponce2.ID_CONTENT = searchResponce1.ID_CONTENT;
                    searchResponce2.ID_CONTENT_LEVEL = searchResponce1.ID_CONTENT_LEVEL;
                    searchResponceList2.Add(searchResponce2);
                }
                this.ViewData["result"] = (object)searchResponceList2;
                List<Task> task = new UtilityModel().getTask(org_id, user_id);
                this.ViewData["Colorlist"] = (object)JsonConvert.DeserializeObject<RootObjectcolor>(new category().GetColorApi(APIString.API + "getColorConfig?orgID=" + (object)org_id));
                this.ViewData["Tasklist"] = (object)task;
                this.ViewData["search"] = (object)s;
                return (ActionResult)this.View();
            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult Notification()
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "getNotificationList?userid=" + (object)user_id + "&orgid=" + (object)org_id));
                NotificationList notificationList = new NotificationList();
                if (apiresponse.KEY == "SUCCESS")
                    notificationList = JsonConvert.DeserializeObject<NotificationList>(apiresponse.MESSAGE);
                this.ViewData["result"] = (object)notificationList;
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public string NotificationCalendar()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "getNotificationList?userid=" + (object)num2 + "&orgid=" + (object)num1));
            NotificationList notificationList = new NotificationList();
            string str = "";
            if (apiresponse.KEY == "SUCCESS")
                str = JsonConvert.DeserializeObject<NotificationList>(apiresponse.MESSAGE).READ[0].EXPIRYDATE;
            return str;
        }

        public JsonResult GetAlldate()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiPost(APIString.API + "getScheduledEventList", new NameValueCollection()
      {
        {
          "OID",
          num1.ToString()
        },
        {
          "UID",
          num2.ToString()
        },
        {
          "DNO",
          "0"
        },
        {
          "MNO",
          "0"
        },
        {
          "YNO",
          "0"
        }
      }));
            EventResponse data = new EventResponse();
            if (apiresponse.KEY == "SUCCESS")
                data = JsonConvert.DeserializeObject<EventResponse>(apiresponse.MESSAGE);
            return this.Json((object)data, JsonRequestBehavior.AllowGet);
        }

        public int NotificationCount()
        {
            int num1 = 0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num2 = 0;
            int num3 = 0;
            if (content != null)
            {
                num2 = Convert.ToInt32(content.id_ORGANIZATION);
                num3 = Convert.ToInt32(content.ID_USER);
            }
            APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "getNotificationList?userid=" + (object)num3 + "&orgid=" + (object)num2));
            NotificationList notificationList = new NotificationList();
            if (apiresponse.KEY == "SUCCESS")
                num1 = JsonConvert.DeserializeObject<NotificationList>(apiresponse.MESSAGE).UNREAD.Count;
            return num1;
        }

        public string Logo()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            return JsonConvert.DeserializeObject<string>(new UtilityModel().getApiResponseString(APIString.API + "GetLogo?orgID=" + (object)num1));
        }

        public ActionResult NotificationAlert(int configid)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "getNotificationAlert?configid=" + (object)configid + "&userid=" + (object)user_id + "&orgid=" + (object)org_id));
                IBHFL.Models.NotificationAlert notificationAlert = new IBHFL.Models.NotificationAlert();
                if (apiresponse.KEY == "SUCCESS")
                {
                    notificationAlert = JsonConvert.DeserializeObject<IBHFL.Models.NotificationAlert>(apiresponse.MESSAGE);
                    notificationAlert.REDIRECTION_URL = notificationAlert.REDIRECTION_URL.Replace('&', '_');
                }
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                this.ViewData["result"] = (object)notificationAlert;
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult NotificationAssessment(string NEXTAPI)
        {
            try
            {
                NEXTAPI = NEXTAPI.Replace('_', '&');
                int int32 = Convert.ToInt32(NEXTAPI.Split('?')[1].Split('&')[0].Split('=')[1]);
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.RAW + NEXTAPI));
                IBHFL.Models.AssessmentSheet assessmentSheet = new IBHFL.Models.AssessmentSheet();
                string str = (string)null;
                if (apiresponse.KEY == "SUCCESS")
                    assessmentSheet = JsonConvert.DeserializeObject<IBHFL.Models.AssessmentSheet>(apiresponse.MESSAGE);
                else
                    str = apiresponse.MESSAGE;
                this.ViewData["message"] = (object)str;
                this.ViewData["result"] = (object)assessmentSheet;
                this.ViewData["AssessmentSheet"] = (object)apiresponse;
                this.ViewData["asid"] = (object)int32;
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                this.ViewData["Colorlist"] = (object)JsonConvert.DeserializeObject<RootObjectcolor>(new category().GetColorApi(APIString.API + "getColorConfig?orgID=" + (object)org_id));
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult NotificationProgram(string NEXTAPI)
        {
            try
            {
                NEXTAPI = NEXTAPI.Replace('_', '&');
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                string api = APIString.RAW + NEXTAPI;
                CategroyDashboard categroyDashboard1 = (CategroyDashboard)null;
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(api));
                if (apiresponse.KEY == "SUCCESS")
                {
                    CategroyDashboard categroyDashboard2 = new CategroyDashboard();
                    categroyDashboard1 = JsonConvert.DeserializeObject<CategroyDashboard>(apiresponse.MESSAGE);
                }
                this.ViewData["Colorlist"] = (object)JsonConvert.DeserializeObject<RootObjectcolor>(new category().GetColorApi(APIString.API + "getColorConfig?orgID=" + (object)org_id));
                this.ViewData["obj"] = (object)categroyDashboard1;
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult NotificationContent(string NEXTAPI)
        {
            try
            {
                NEXTAPI = NEXTAPI.Replace('_', '&');
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                AnswerResponse answerResponse = JsonConvert.DeserializeObject<AnswerResponse>(new UtilityModel().getApiResponseString(APIString.RAW + NEXTAPI));
                List<SatisfiedResult> satisfiedResultList = JsonConvert.DeserializeObject<List<SatisfiedResult>>(new UtilityModel().getApiResponseString(APIString.API + "Satisfied?answerID=" + (object)answerResponse.ID_CONTENT_ANSWER + "&oid=" + (object)org_id + "&uid=" + (object)user_id));
                int num1 = 0;
                if (satisfiedResultList != null)
                    num1 = satisfiedResultList.Count;
                this.ViewData["Colorlist"] = (object)JsonConvert.DeserializeObject<RootObjectcolor>(new category().GetColorApi(APIString.API + "getColorConfig?orgID=" + (object)org_id));
                List<NewAnswerSteps> newAnswerStepsList = JsonConvert.DeserializeObject<List<NewAnswerSteps>>(new UtilityModel().getApiResponseString(APIString.API + "CalltoAction?answerID=" + (object)answerResponse.ID_CONTENT_ANSWER + "&orgID=" + (object)org_id + "&uid=" + (object)user_id));
                int num2 = 0;
                if (newAnswerStepsList != null)
                    num2 = newAnswerStepsList.Count;
                this.ViewData["obj"] = (object)answerResponse;
                this.ViewData["Satisfied"] = (object)satisfiedResultList;
                this.ViewData["sCount"] = (object)num1;
                this.ViewData["StepCount"] = (object)num2;
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public JsonResult Notificationjson()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "getNotificationList?userid=" + (object)num2 + "&orgid=" + (object)num1));
            NotificationList data = new NotificationList();
            if (apiresponse.KEY == "SUCCESS")
                data = JsonConvert.DeserializeObject<NotificationList>(apiresponse.MESSAGE);
            return this.Json((object)data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EventList(string datecl)
        {
            try
            {
                string[] strArray = datecl.Split('-');
                string str1 = strArray[0];
                string str2 = strArray[1];
                string str3 = strArray[2];
                int length1 = str1.Length;
                int length2 = str2.Length;
                if (length1 == 1)
                    str1 = "0" + strArray[0];
                if (length2 == 1)
                    str2 = "0" + strArray[1];
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiPost(APIString.API + "getScheduledEventList", new NameValueCollection()
        {
          {
            "OID",
            org_id.ToString()
          },
          {
            "UID",
            user_id.ToString()
          },
          {
            "DNO",
            str1
          },
          {
            "MNO",
            str2
          },
          {
            "YNO",
            str3
          }
        }));
                EventResponse eventResponse = new EventResponse();
                if (apiresponse.KEY == "SUCCESS")
                    eventResponse = JsonConvert.DeserializeObject<EventResponse>(apiresponse.MESSAGE);
                this.ViewData["obj"] = (object)eventResponse;
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult ScheduledEvent(int sid)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiPost(APIString.API + "getScheduledEvent", new NameValueCollection()
        {
          {
            "OID",
            org_id.ToString()
          },
          {
            "UID",
            user_id.ToString()
          },
          {
            "EID",
            sid.ToString()
          }
        }));
                IBHFL.Models.ScheduledEvent scheduledEvent = new IBHFL.Models.ScheduledEvent();
                if (apiresponse.KEY == "SUCCESS")
                    scheduledEvent = JsonConvert.DeserializeObject<IBHFL.Models.ScheduledEvent>(apiresponse.MESSAGE);
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                this.ViewData["obj"] = (object)scheduledEvent;
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult EventSubscribe(int Eid)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int num1 = 0;
                int num2 = 0;
                if (content != null)
                {
                    num1 = Convert.ToInt32(content.id_ORGANIZATION);
                    num2 = Convert.ToInt32(content.ID_USER);
                }
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiPost(APIString.API + "setScheduledEventSubscrioption", new NameValueCollection()
        {
          {
            "OID",
            num1.ToString()
          },
          {
            "UID",
            num2.ToString()
          },
          {
            "EID",
            Eid.ToString()
          },
          {
            "OPT",
            "1"
          },
          {
            "COMMENT",
            ""
          }
        }));
                IBHFL.tbl_scheduled_event_subscription_log eventSubscriptionLog = new IBHFL.tbl_scheduled_event_subscription_log();
                string str = (string)null;
                if (apiresponse.KEY == "SUCCESS")
                    eventSubscriptionLog = JsonConvert.DeserializeObject<IBHFL.tbl_scheduled_event_subscription_log>(apiresponse.MESSAGE);
                else
                    str = apiresponse.MESSAGE;
                this.ViewData["message"] = (object)str;
                return (ActionResult)this.RedirectToAction("Index", (object)new
                {
                    Message = str
                });
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult EventUnSubscribe()
        {
            try
            {
                int int32 = Convert.ToInt32(this.Request.Form["Eid"].ToString());
                string str1 = this.Request.Form["comment"].ToString();
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int num1 = 0;
                int num2 = 0;
                if (content != null)
                {
                    num1 = Convert.ToInt32(content.id_ORGANIZATION);
                    num2 = Convert.ToInt32(content.ID_USER);
                }
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiPost(APIString.API + "setScheduledEventSubscrioption", new NameValueCollection()
        {
          {
            "OID",
            num1.ToString()
          },
          {
            "UID",
            num2.ToString()
          },
          {
            "EID",
            int32.ToString()
          },
          {
            "OPT",
            "0"
          },
          {
            "COMMENT",
            str1
          }
        }));
                IBHFL.tbl_scheduled_event_subscription_log eventSubscriptionLog = new IBHFL.tbl_scheduled_event_subscription_log();
                string str2 = (string)null;
                if (apiresponse.KEY == "SUCCESS")
                    eventSubscriptionLog = JsonConvert.DeserializeObject<IBHFL.tbl_scheduled_event_subscription_log>(apiresponse.MESSAGE);
                else
                    str2 = apiresponse.MESSAGE;
                return (ActionResult)this.RedirectToAction("Index", (object)new
                {
                    Message = str2
                });
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult Comment(int Eid)
        {
            try
            {
                this.ViewData["obj"] = (object)Eid;
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult LikeContent(int choice, int cid, int catID)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }

                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                new UtilityModel().getApiResponseString(APIString.API + "LikeContent?FLAG=" + (object)choice + "&UID=" + (object)user_id + "&CID=" + (object)cid + "&OID=2" + (object)org_id);

                if (choice == 1)
                {

                    return Json(new { success = true, message = "You have liked the content" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { success = true, message = "Thanks for your feedback!" }, JsonRequestBehavior.AllowGet);

                }

                //this.ViewData[nameof(cid)] = (object)cid;
                //this.ViewData[nameof(catID)] = (object)catID;
                //this.ViewData["mes"] = (object)"Thanks for your feedback! :)";
                return (ActionResult)this.View();
            }
            catch
            {

                return Json(new { success = false, message = "An error occurred while processing your request." });
            }
        }


        public ActionResult LikeContentStep(int choice, int cid, int IDcontentanswer)
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                List<Task> task = new UtilityModel().getTask(org_id, user_id);
                new UtilityModel().getApiResponseString(APIString.API + "LikeContent?FLAG=" + (object)choice + "&UID=" + (object)user_id + "&CID=" + (object)cid + "&OID=2" + (object)org_id);
                if (choice == 1)
                {
                    string str = "You have liked the content";
                    return (ActionResult)this.RedirectToAction("AnswerStep", (object)new
                    {
                        ID_CONTENT_ANSWER = IDcontentanswer,
                        ContentID = cid,
                        message = str
                    });
                }
                this.ViewData["Tasklist"] = (object)task;
                this.ViewData[nameof(cid)] = (object)cid;
                this.ViewData["mes"] = (object)"Thanks for your feedback! :)";
                return (ActionResult)this.View();
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult Chatbot() => (ActionResult)this.View();

        public ActionResult maintenance(string msg)
        {
            this.ViewData["msgg"] = (object)msg;
            return (ActionResult)this.View();
        }

        public ActionResult emaildata()
        {
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int uid = 0;
                int oid = 0;
                if (content != null)
                {
                    uid = Convert.ToInt32(content.id_ORGANIZATION);
                    oid = Convert.ToInt32(content.ID_USER);
                }
                mailmod admin = new mailmod();
                admin.sub = this.Request.Form["sub"].ToString();
                admin.msg = this.Request.Form["msg"].ToString();
                string str = this.Request.Form["mes"].ToString();
                int cid = admin.cid = Convert.ToInt32(this.Request.Form["cid"].ToString());
                int int32 = Convert.ToInt32(this.Request.Form["CAT"].ToString());
                new category().Email(admin, uid, oid, cid);
                return (ActionResult)this.RedirectToAction("ProductQueAns", (object)new
                {
                    ContentID = cid,
                    CategoryID = int32,
                    message = str
                });
            }
            catch
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public ActionResult error() => (ActionResult)this.View();

        public ActionResult Gamification(string NEXTAPI)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int user_id = 0;
            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                user_id = Convert.ToInt32(content.ID_USER);
            }
            this.ViewData["oid"] = (object)org_id;
            this.ViewData["uid"] = (object)user_id;
            this.ViewData["api"] = (object)NEXTAPI;
            this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
            return (ActionResult)this.View();
        }

        public string ScheduleContent(string NEXTAPI)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiPost(APIString.API + "getScheduledEventList", new NameValueCollection()
      {
        {
          "OID",
          num1.ToString()
        },
        {
          "UID",
          num2.ToString()
        },
        {
          "DNO",
          "0"
        },
        {
          "MNO",
          "0"
        },
        {
          "YNO",
          "0"
        }
      }));
            EventResponse eventResponse1 = new EventResponse();
            string str1 = "";
            if (apiresponse.KEY == "SUCCESS")
            {
                EventResponse eventResponse2 = JsonConvert.DeserializeObject<EventResponse>(apiresponse.MESSAGE);
                if (eventResponse2.READ.Count != 0 || eventResponse2.UNREAD.Count != 0)
                {
                    string str2 = "<table class='table-responsive full'>";
                    if (eventResponse2.READ.Count != 0)
                    {
                        foreach (EventThumbnail eventThumbnail in eventResponse2.READ)
                        {
                            string str3 = ((IEnumerable<string>)eventThumbnail.event_start_datetime.Split(' ')).First<string>();
                            str2 = str2 + "<tr><td><a href='../Dashboard/EventList?datecl=" + str3 + "'><small class='font-bold sml'>" + eventThumbnail.event_title + "</small></a></td><td class='pull-right text-right'> <small class='font-bold sml' style='font-size: 12px;'><span class='label1 label-default'></span>" + eventThumbnail.event_start_datetime + "</small></td></tr>";
                        }
                    }
                    if (eventResponse2.UNREAD.Count != 0)
                    {
                        foreach (EventThumbnail eventThumbnail in eventResponse2.UNREAD)
                        {
                            string str4 = ((IEnumerable<string>)eventThumbnail.event_start_datetime.Split(' ')).First<string>();
                            str2 = str2 + "<tr><td><a href='../Dashboard/EventList?datecl=" + str4 + "'><small class='font-bold sml'>" + eventThumbnail.event_title + "</small></a></td><td class='pull-right text-right'> <small class='font-bold sml' style='font-size: 12px;'><span class='label1 label-default'></span>" + eventThumbnail.event_start_datetime + "</small></td></tr>";
                        }
                    }
                    str1 = str2 + "</table>";
                }
            }
            return str1;
        }

        public ActionResult addEvent()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int user_id = 0;
            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                user_id = Convert.ToInt32(content.ID_USER);
            }
            string task = this.Request.Form["task"];
            new UtilityModel().addTask(org_id, user_id, task);
            return (ActionResult)this.RedirectToAction("Index");
        }

        public List<Task> getTask()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int user_id = 0;
            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                user_id = Convert.ToInt32(content.ID_USER);
            }
            List<Task> task = new UtilityModel().getTask(org_id, user_id);
            this.ViewData["Tasklist"] = (object)task;
            return task;
        }

        public string deleteTask(string id)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            new UtilityModel().deleteTask(Convert.ToInt32(id));
            return "Success";
        }

        public string getPerformance()
        {
            string str = "";
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int user_id = 0;
            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                user_id = Convert.ToInt32(content.ID_USER);
            }
            int performance1 = new UtilityModel().getPerformance(user_id, org_id);
            string performance2;
            if (performance1 != 0)
                performance2 = str + "<div class='GaugeMeter' id='PreviewGaugeMeter_4' data-percent='" + (object)performance1 + "' data-append='' data-size='180' data-theme='Red-Gold-Green' data-back='RGBa(0,0,0,.1)' data-animate_gauge_colors='1' data-animate_text_colors='1' data-width='15' data-label='Flux Capacitor' data-label_color='#FFF' data-stripe='3'></div> <p>Performance Percentile</p>";
            else
                performance2 = str + "<div class='GaugeMeter' id='PreviewGaugeMeter_4' data-percent='" + (object)0 + "' data-append='' data-size='180' data-theme='Red-Gold-Green' data-back='RGBa(0,0,0,.1)' data-animate_gauge_colors='1' data-animate_text_colors='1' data-width='15' data-label='Flux Capacitor' data-label_color='#FFF' data-stripe='3'></div> <p>Performance Percentile</p>";
            return performance2;
        }

        public string getMyGames()
        {
            string myGames1 = "null";
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int orgid = 0;
            int userid = 0;
            if (content != null)
            {
                orgid = Convert.ToInt32(content.id_ORGANIZATION);
                userid = Convert.ToInt32(content.ID_USER);
            }
            List<GameElements> myGames2 = new UtilityModel().getMyGames(orgid, userid);
            if (myGames2.Count != 0)
            {
                string str = " <table class='table-responsive full'><tbody>";
                foreach (GameElements gameElements in myGames2)
                    str = str + "<tr> <td> <small class='font-bold sml'>" + gameElements.game_title + "</small></td> <td> <small class='font-bold sml'>" + gameElements.game_description + "</small></td> </tr>";
                myGames1 = str + " </tbody></table>";
            }
            return myGames1;
        }

        public JsonResult getTicker()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int num = 0;
            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                num = Convert.ToInt32(content.ID_USER);
            }
            Ticker ticker = new UtilityModel().getTicker(org_id);
            ////return "<marquee id= 'marque1' style='background-color:" + ticker.background_color + ";color:" + ticker.font_color + "'  behavior = 'scroll' direction = 'left' >" + ticker.ticker_news + "</ marquee >";
            //return ticker;
            return this.Json((object)ticker, JsonRequestBehavior.AllowGet);
        }

        public string getSkill()
        {
            string skill = "";
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int user_id = 0;
            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                user_id = Convert.ToInt32(content.ID_USER);
            }
            int num = new UtilityModel().OverallScore(user_id, org_id);
            if (num != 0)
            {
                if ((double)num < 2.0)
                    skill += "<div class='row bs-wizard' style='border-bottom:0' align='center'> <div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Rookie</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> <div class='col-xs-2 col2 bs-wizard-step active'> <div class='text-center bs-wizard-stepnum'>Beginner</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> <div class='col-xs-2 col2 bs-wizard-step active'> <div class='text-center bs-wizard-stepnum'>Expert</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div><div class='col-xs-2 col2 bs-wizard-step active'> <div class='text-center bs-wizard-stepnum'>Grandmaster</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> </div>";
                else if ((double)num >= 2.0 && (double)num <= 3.5)
                    skill += "<div class='row bs-wizard' style='border-bottom:0' align='center'> <div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Rookie</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> <div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Beginner</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> <div class='col-xs-2 col2 bs-wizard-step active'> <div class='text-center bs-wizard-stepnum'>Expert</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div><div class='col-xs-2 col2 bs-wizard-step active'> <div class='text-center bs-wizard-stepnum'>Grandmaster</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> </div>";
                else if ((double)num >= 3.6 && (double)num <= 4.5)
                    skill += "<div class='row bs-wizard' style='border-bottom:0' align='center'> <div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Rookie</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> <div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Beginner</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> <div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Expert</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div><div class='col-xs-2 col2 bs-wizard-step active'> <div class='text-center bs-wizard-stepnum'>Grandmaster</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> </div>";
                else if ((double)num > 4.5)
                    skill += "<div class='row bs-wizard' style='border-bottom:0' align='center'> <div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Rookie</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> <div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Beginner</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> <div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Expert</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div><div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Grandmaster</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> </div>";
            }
            else
                skill += "<div class='row bs-wizard' style='border-bottom:0' align='center'> <div class='col-xs-2 col2 bs-wizard-step complete'> <div class='text-center bs-wizard-stepnum'>Rookie</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> <div class='col-xs-2 col2 bs-wizard-step active'> <div class='text-center bs-wizard-stepnum'>Beginner</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> <div class='col-xs-2 col2 bs-wizard-step active'> <div class='text-center bs-wizard-stepnum'>Expert</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div><div class='col-xs-2 col2 bs-wizard-step active'> <div class='text-center bs-wizard-stepnum'>Grandmaster</div> <div class='progress'><div class='progress-bar'></div></div> <a href='#' class='bs-wizard-dot'></a> </div> </div>";
            return skill;
        }

        public string checkProgramCompletion()
        {
            double num1 = 0.0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int int32_1 = Convert.ToInt32(content.ID_USER);
            int int32_2 = Convert.ToInt32(content.id_ORGANIZATION);
            List<tbl_category> source = new UtilityModel().getprg("select * from tbl_category where id_category in (select distinct id_category from tbl_content_program_mapping where id_organization=" + (object)int32_2 + ")");
            List<tbl_category> collection = new UtilityModel().getprg("select * from tbl_category where id_category in (select distinct id_category from tbl_content_organization_mapping where id_organization =" + (object)int32_2 + ")");
            source.AddRange((IEnumerable<tbl_category>)collection);
            int num2 = source.Count<tbl_category>();
            int num3 = 0;
            if (num2 > 0)
            {
                foreach (tbl_category tblCategory in source)
                {
                    int recordCount1 = new UtilityModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object)tblCategory.ID_CATEGORY ?? "");
                    int num4 = 0;
                    if (recordCount1 > 0)
                    {
                        int recordCount2 = new UtilityModel().getRecordCount("select count(*) count from tbl_content_organization_mapping where id_category=" + (object)tblCategory.ID_CATEGORY + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object)int32_1 + ")");
                        num4 = recordCount1 - recordCount2;
                    }
                    if (num4 == recordCount1)
                        ++num3;
                }
                num1 = (double)Convert.ToInt32((double)num3 / (double)num2 * 100.0);
            }
            return "<div class='GaugeMeter' id='PreviewGaugeMeter_5' data-percent='" + (object)num1 + "' data-append=' % ' data-size='180' data-theme='Red-Gold-Green' data-back='RGBa(0, 0, 0, .1)' data-animate_gauge_colors='1' data-animate_text_colors='1' data-width='15' data-label='Flux Capacitor' data-label_color='#FFF' data-stripe='3'></div> <p>Progress</p>";
        }

        public string getLeaderBoard()
        {
            string leaderBoard1 = "null";
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int num = 0;
            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                num = Convert.ToInt32(content.ID_USER);
            }
            List<LeaderBoard> leaderBoard2 = new UtilityModel().getLeaderBoard(org_id);
            if (leaderBoard2.Count != 0)
            {
                string str = "<table class='table-responsive full'>";
                foreach (LeaderBoard leaderBoard3 in leaderBoard2)
                    str = str + "<tr><td><small class='font-bold sml'>" + leaderBoard3.username + "</small></td><td class=''><small class='font-bold sml'>Score: " + (object)leaderBoard3.OverallScore + "</small></td><td><small class='font-bold sml'>Percentile: " + (object)leaderBoard3.Percentile + "<sup>" + leaderBoard3.Ordinal + "</sup></small></td></tr>";
                leaderBoard1 = str + " </table>";
            }
            return leaderBoard1;
        }

        public string checkpassword(string password)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num = 0;
            int uid = 0;
            if (content != null)
            {
                num = Convert.ToInt32(content.id_ORGANIZATION);
                uid = Convert.ToInt32(content.ID_USER);
            }
            password = password.ToMD5Hash();
            return new UtilityModel().check_password(password, uid);
        }

        public string ChangePassword(string password)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            return new UtilityModel().getApiPost(APIString.API + nameof(ChangePassword), new NameValueCollection()
      {
        {
          "ID_ORGANIZATION",
          Convert.ToString(num1)
        },
        {
          "ID_USER",
          content.USERID
        },
        {
          "PASSWORD",
          password
        }
      });
        }

        public string checkforcepasswordchange()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            return new UtilityModel().getApiResponseString(APIString.API + "ForcePasswordChange?uid=" + (object)num2 + "&oid=" + (object)num1);
        }

        public string getNonDisclosureContent()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            return Convert.ToString(JsonConvert.DeserializeObject<List<tbl_non_disclousure_clause_content>>(new UtilityModel().getApiResponseString(APIString.API + "getNonDisclosureContent?oid=" + (object)num1)).Count);
        }

        public string getNonDisclosureContentlog()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            return new UtilityModel().getApiResponseString(APIString.API + "CheckNonDisclosureLog?uid=" + (object)num2 + "&oid=" + (object)num1);
        }

        public JsonResult getNonDisclosureContentload()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            return this.Json((object)JsonConvert.DeserializeObject<List<tbl_non_disclousure_clause_content>>(new UtilityModel().getApiResponseString(APIString.API + "getNonDisclosureContent?oid=" + (object)num1))[0], JsonRequestBehavior.AllowGet);
        }

        public string NonDisclosureLogRecord()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            new UtilityModel().getApiPost(APIString.API + "LogNonClosureClause", new NameValueCollection()
      {
        {
          "id_org",
          Convert.ToString(content.id_ORGANIZATION)
        },
        {
          "id_user",
          Convert.ToString(num2)
        }
      });
            return "";
        }

        public string getNotificationcount()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num1 = 0;
            int num2 = 0;
            if (content != null)
            {
                num1 = Convert.ToInt32(content.id_ORGANIZATION);
                num2 = Convert.ToInt32(content.ID_USER);
            }
            APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "getNotificationList?userid=" + (object)num2 + "&orgid=" + (object)num1));
            NotificationList notificationList = new NotificationList();
            if (apiresponse.KEY == "SUCCESS")
                notificationList = JsonConvert.DeserializeObject<NotificationList>(apiresponse.MESSAGE);
            return Convert.ToString(notificationList.UNREAD.Count);
        }

        public ActionResult ProductQueAnsPartial(int ContentID, int CategoryID, string message)
        {
            try
            {
                List<content_list_session> contentListSessionList1 = (List<content_list_session>)this.HttpContext.Session.Contents["ContentSession"];
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                AnswerResponse answerResponse = JsonConvert.DeserializeObject<AnswerResponse>(new UtilityModel().getApiResponseString(APIString.API + "GetContentDetails?conId=" + (object)ContentID + "&userid=" + (object)user_id + "&orgid=" + (object)org_id));
                string apiResponseString = new UtilityModel().getApiResponseString(APIString.API + "Satisfied?answerID=" + (object)answerResponse.ID_CONTENT_ANSWER + "&oid=" + (object)org_id + "&uid=" + (object)user_id);
                List<AssessmentList> assessmentListList = (List<AssessmentList>)null;
                if (answerResponse.ASSESSMENT_FLAG == "1")
                    assessmentListList = JsonConvert.DeserializeObject<List<AssessmentList>>(new UtilityModel().getApiResponseString(APIString.API + "AssessmentList?CID=" + (object)ContentID + "&UID=" + (object)user_id + "&OID=" + (object)org_id));
                List<SatisfiedResult> satisfiedResultList = JsonConvert.DeserializeObject<List<SatisfiedResult>>(apiResponseString);
                int num1 = 0;
                if (satisfiedResultList != null)
                    num1 = satisfiedResultList.Count;
                List<NewAnswerSteps> newAnswerStepsList = JsonConvert.DeserializeObject<List<NewAnswerSteps>>(new UtilityModel().getApiResponseString(APIString.API + "CalltoAction?answerID=" + (object)answerResponse.ID_CONTENT_ANSWER + "&orgID=" + (object)org_id + "&uid=" + (object)user_id));
                int num2 = 0;
                if (newAnswerStepsList != null)
                    num2 = newAnswerStepsList.Count;
                RootObjectcolor rootObjectcolor = JsonConvert.DeserializeObject<RootObjectcolor>(new category().GetColorApi(APIString.API + "getColorConfig?orgID=" + (object)org_id));
                CategroyDashboard categroyDashboard1 = (CategroyDashboard)null;
                APIRESPONSE apiresponse = JsonConvert.DeserializeObject<APIRESPONSE>(new UtilityModel().getApiResponseString(APIString.API + "getCategoryDashboard?catid=" + (object)CategoryID + "&orgid=" + (object)org_id + "&userid=" + (object)user_id));
                if (apiresponse.KEY == "SUCCESS")
                {
                    CategroyDashboard categroyDashboard2 = new CategroyDashboard();
                    categroyDashboard1 = JsonConvert.DeserializeObject<CategroyDashboard>(apiresponse.MESSAGE);
                }
                List<content_list_session> contentListSessionList2 = new List<content_list_session>();
                if (categroyDashboard1 != null)
                {
                    int num3 = 0;
                    foreach (SearchResponce searchResponce in categroyDashboard1.CONTENTLIST)
                    {
                        contentListSessionList2.Add(new content_list_session()
                        {
                            content_index = num3,
                            prev_con_id = num3 != 0 ? categroyDashboard1.CONTENTLIST[num3 - 1].ID_CONTENT : 0,
                            next_con_id = num3 != categroyDashboard1.CONTENTLIST.Count - 1 ? categroyDashboard1.CONTENTLIST[num3 + 1].ID_CONTENT : 0,
                            CategoryID = CategoryID,
                            total_count = categroyDashboard1.CONTENTLIST.Count
                        });
                        ++num3;
                    }
                    System.Web.HttpContext.Current.Session["ContentSession"] = (object)contentListSessionList2;
                    contentListSessionList1 = contentListSessionList2;
                }
                int num4 = 0;
                int index = 0;
                foreach (SearchResponce searchResponce in categroyDashboard1.CONTENTLIST)
                {
                    if (ContentID == searchResponce.ID_CONTENT)
                        index = num4;
                    ++num4;
                }
                content_list_session contentListSession = new content_list_session();
                this.ViewData["con_info"] = (object)contentListSessionList1[index];
                this.ViewData["Colorlist"] = (object)rootObjectcolor;
                this.ViewData[nameof(message)] = (object)message;
                this.ViewData["obj"] = (object)answerResponse;
                this.ViewData["Satisfied"] = (object)satisfiedResultList;
                this.ViewData["AssessmentList"] = (object)assessmentListList;
                this.ViewData["sCount"] = (object)num1;
                this.ViewData["StepCount"] = (object)num2;
                this.ViewData["Tasklist"] = (object)new UtilityModel().getTask(org_id, user_id);
                return (ActionResult)this.View();
            }
            catch (Exception ex)
            {
                return (ActionResult)this.RedirectToAction("error");
            }
        }

        public string GetAssessmentsAverage()
        {
            string assessmentsAverage = "";
            double num1 = 0.0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int user_id = 0;
            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                user_id = Convert.ToInt32(content.ID_USER);
            }
            List<AssessmentResponce> assessmentResponceList = JsonConvert.DeserializeObject<List<AssessmentResponce>>(new UtilityModel().getApiResponseString(APIString.API + "getAllAssessmnetsSummary?UID=" + (object)user_id));
            new UtilityModel().getTask(org_id, user_id);
            if (assessmentResponceList.Count != 0)
            {
                foreach (AssessmentResponce assessmentResponce in assessmentResponceList)
                {
                    string[] source = assessmentResponce.Message.Split('|');
                    this.ViewData["rs1"] = (object)source[1];
                    for (int index = 2; index < ((IEnumerable<string>)source).Count<string>() - 1; ++index)
                        assessmentsAverage = assessmentsAverage + "\n " + source[index];
                    this.ViewData["rs2"] = (object)assessmentsAverage;
                    double num2 = Math.Round((double)Convert.ToInt32(((IEnumerable<string>)assessmentsAverage.Split(':')).Last<string>()) / (double)assessmentResponce.QuestionAnswer.Count * 100.0, 2);
                    num1 += num2;
                }
                num1 = Math.Round(num1 / (double)assessmentResponceList.Count, 2);
            }
            if (num1 <= 40.0)
                assessmentsAverage = "<label style='font-size:13px;font-weight:500;margin-left:-75%'>Overall</label><div class='progress' style='width:90%; height:17px;'> <div class='progress-bar progress-bar-success progress-bar-striped' role='progressbar' aria-valuenow='70' aria-valuemin='0' aria-valuemax='100' style='width:" + (object)num1 + "%;font-size:13px;background-color: #ec3e3e;'>" + (object)num1 + "% </div> </div>";
            else if (num1 <= 75.0)
                assessmentsAverage = "<label style='font-size:13px;font-weight:500;margin-left:-75%'>Overall</label><div class='progress' style='width:90%; height:17px;'> <div class='progress-bar progress-bar-success progress-bar-striped' role='progressbar' aria-valuenow='70' aria-valuemin='0' aria-valuemax='100' style='width:" + (object)num1 + "%;font-size:13px;background-color: #d0d01a;color:black'>" + (object)num1 + "% </div> </div> ";
            else if (num1 > 75.0)
                assessmentsAverage = "<label style='font-size:13px;font-weight:500;margin-left:-75%'>Overall</label><div class='progress' style='width:90%; height:17px;'> <div class='progress-bar progress-bar-success progress-bar-striped' role='progressbar' aria-valuenow='70' aria-valuemin='0' aria-valuemax='100' style='width:" + (object)num1 + "%;font-size:13px;background-color: green;'>" + (object)num1 + "% </div> </div>";
            return assessmentsAverage;
        }

        public string GetUserAssignedAssessmentAverage()
        {
            string assessmentAverage = "";
            double num1 = 0.0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int org_id = 0;
            int user_id = 0;
            if (content != null)
            {
                org_id = Convert.ToInt32(content.id_ORGANIZATION);
                user_id = Convert.ToInt32(content.ID_USER);
            }
            List<AssessmentResponce> assessmentResponceList = JsonConvert.DeserializeObject<List<AssessmentResponce>>(new UtilityModel().getApiResponseString(APIString.API + "getAllAssessmnetsSummary?UID=" + (object)user_id));
            new UtilityModel().getTask(org_id, user_id);
            List<tbl_assessment_user_assignment> assessmentUserAssignmentList = JsonConvert.DeserializeObject<List<tbl_assessment_user_assignment>>(new UtilityModel().getApiResponseString(APIString.API + "getUserAssignedAssessments?UID=" + (object)user_id));
            if (assessmentResponceList.Count != 0)
            {
                foreach (tbl_assessment_user_assignment assessmentUserAssignment in assessmentUserAssignmentList)
                {
                    foreach (AssessmentResponce assessmentResponce in assessmentResponceList)
                    {
                        int? idAssessment1 = assessmentUserAssignment.id_assessment;
                        int idAssessment2 = assessmentResponce.Assessment[0].id_assessment;
                        if ((idAssessment1.GetValueOrDefault() == idAssessment2 ? (idAssessment1.HasValue ? 1 : 0) : 0) != 0)
                        {
                            string[] source = assessmentResponce.Message.Split('|');
                            this.ViewData["rs1"] = (object)source[1];
                            for (int index = 2; index < ((IEnumerable<string>)source).Count<string>() - 1; ++index)
                                assessmentAverage = assessmentAverage + "\n " + source[index];
                            this.ViewData["rs2"] = (object)assessmentAverage;
                            double num2 = Math.Round((double)Convert.ToInt32(((IEnumerable<string>)assessmentAverage.Split(':')).Last<string>()) / (double)assessmentResponce.QuestionAnswer.Count * 100.0, 2);
                            num1 += num2;
                        }
                    }
                }
                num1 = Math.Round(num1 / (double)assessmentUserAssignmentList.Count, 2);
            }
            if (assessmentUserAssignmentList.Count == 0)
                assessmentAverage = "<label style='font-size:13px;font-weight:500;margin-left:-71%'>Assigned</label><div class='progress' style='width:90%; height:17px;'> <div class='progress-bar progress-bar-success progress-bar-striped' role='progressbar' aria-valuenow='70' aria-valuemin='0' aria-valuemax='100' style='width:" + (object)0 + "%;font-size:13px;background-color: #ec3e3e;color:black'>" + (object)0 + "% </div> </div> ";
            else if (num1 <= 40.0)
                assessmentAverage = "<label style='font-size:13px;font-weight:500;margin-left:-71%'>Assigned</label><div class='progress' style='width:90%; height:17px;'> <div class='progress-bar progress-bar-success progress-bar-striped' role='progressbar' aria-valuenow='70' aria-valuemin='0' aria-valuemax='100' style='width:" + (object)num1 + "%;font-size:13px;background-color: #ec3e3e;'>" + (object)num1 + "% </div> </div> ";
            else if (num1 <= 75.0)
                assessmentAverage = "<label style='font-size:13px;font-weight:500;margin-left:-71%'>Assigned</label><div class='progress' style='width:90%; height:17px;'> <div class='progress-bar progress-bar-success progress-bar-striped' role='progressbar' aria-valuenow='70' aria-valuemin='0' aria-valuemax='100' style='width:" + (object)num1 + "%;font-size:13px;background-color: #d0d01a;color:black'>" + (object)num1 + "% </div> </div>";
            else if (num1 > 75.0)
                assessmentAverage = "<label style='font-size:13px;font-weight:500;margin-left:-71%'>Assigned</label><div class='progress' style='width:90%; height:17px;'> <div class='progress-bar progress-bar-success progress-bar-striped' role='progressbar' aria-valuenow='70' aria-valuemin='0' aria-valuemax='100' style='width:" + (object)num1 + "%;font-size:13px;background-color: green;'>" + (object)num1 + "% </div> </div>";
            return assessmentAverage;
        }

        public void GetAssessmentsAverageInst()
        {
            string str1 = "";
            double num1 = 0.0;
            using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
            {
                foreach (int num2 in m2ostDbContext.Database.SqlQuery<int>("select ID_USER from tbl_user where ID_ORGANIZATION =48").ToList<int>())
                {
                    List<AssessmentResponce> assessmentResponceList = JsonConvert.DeserializeObject<List<AssessmentResponce>>(new UtilityModel().getApiResponseString("https://www.skillmuni.in/m2ostproductionapi/api/getAllAssessmnetsSummary?UID=" + (object)num2));
                    if (assessmentResponceList.Count != 0)
                    {
                        foreach (AssessmentResponce assessmentResponce in assessmentResponceList)
                        {
                            string[] source = assessmentResponce.Message.Split('|');
                            for (int index = 2; index < ((IEnumerable<string>)source).Count<string>() - 1; ++index)
                                str1 = str1 + "\n " + source[index];
                            double num3 = Math.Round((double)Convert.ToInt32(((IEnumerable<string>)str1.Split(':')).Last<string>()) / (double)assessmentResponce.QuestionAnswer.Count * 100.0, 2);
                            num1 += num3;
                        }
                        double num4 = Math.Round(num1 / (double)assessmentResponceList.Count, 2);
                        string str2 = m2ostDbContext.Database.SqlQuery<string>("SELECT CONCAT(FIRSTNAME, ' ',LASTNAME) AS name from tbl_profile where ID_USER={0}", (object)num2).FirstOrDefault<string>();
                        string str3 = m2ostDbContext.Database.SqlQuery<string>("SELECT CITY from tbl_profile where ID_USER={0}", (object)num2).FirstOrDefault<string>();
                        m2ostDbContext.Database.ExecuteSqlCommand("insert into tbl_overall_assessment_user_log (userid,name,location,percentage) values({0},{1},{2},{3})", (object)num2, (object)str2, (object)str3, (object)num4);
                        num1 = 0.0;
                    }
                }
            }
        }

        public void checkProgramCompletionins()
        {
            double num1 = 0.0;
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int num2 = 48;
            using (M2ostDbContext m2ostDbContext = new M2ostDbContext())
            {
                List<int> list = m2ostDbContext.Database.SqlQuery<int>("select ID_USER from tbl_user where ID_ORGANIZATION =48").ToList<int>();
                int num3 = 1;
                foreach (int num4 in list)
                {
                    if (num3 > 121)
                    {
                        List<tbl_category> source = new UtilityModel().getprg("select * from tbl_category where id_category in (select distinct id_category from tbl_content_program_mapping where id_organization=" + (object)num2 + ")");
                        List<tbl_category> collection = new UtilityModel().getprg("select * from tbl_category where id_category in (select distinct id_category from tbl_content_organization_mapping where id_organization =" + (object)num2 + ")");
                        source.AddRange((IEnumerable<tbl_category>)collection);
                        int num5 = source.Count<tbl_category>();
                        int num6 = 0;
                        if (num5 > 0)
                        {
                            foreach (tbl_category tblCategory in source)
                            {
                                string sql1 = "select count(*) count from tbl_content_organization_mapping where id_category=" + (object)tblCategory.ID_CATEGORY ?? "";
                                int num7 = m2ostDbContext.Database.SqlQuery<int>(sql1).FirstOrDefault<int>();
                                int num8 = 0;
                                if (num7 > 0)
                                {
                                    string sql2 = "select count(*) count from tbl_content_organization_mapping where id_category=" + (object)tblCategory.ID_CATEGORY + " and id_content not in (select distinct id_content from tbl_content_counters where id_user=" + (object)num4 + ")";
                                    int num9 = m2ostDbContext.Database.SqlQuery<int>(sql2).FirstOrDefault<int>();
                                    num8 = num7 - num9;
                                }
                                if (num8 == num7)
                                    ++num6;
                            }
                            double num10 = (double)num6 / (double)num5 * 100.0;
                            string str1 = m2ostDbContext.Database.SqlQuery<string>("SELECT CONCAT(FIRSTNAME, ' ',LASTNAME) AS name from tbl_profile where ID_USER={0}", (object)num4).FirstOrDefault<string>();
                            string str2 = m2ostDbContext.Database.SqlQuery<string>("SELECT CITY from tbl_profile where ID_USER={0}", (object)num4).FirstOrDefault<string>();
                            double num11 = Math.Round(num10, 2);
                            m2ostDbContext.Database.ExecuteSqlCommand("insert into tbl_overall_program_content_user_log (userid,name,location,percentage) values({0},{1},{2},{3})", (object)num4, (object)str1, (object)str2, (object)num11);
                        }
                        num1 = 0.0;
                    }
                    ++num3;
                }
            }
        }
        public PartialViewResult AddMilestoneDashboard()
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            tbl_user User = new tbl_user();
            List<tbl_user> DesignationUsers = new List<tbl_user>();
            if (content != null)
            {
                User = new UtilityModel().getUserDetails(content.USERID);
                if (User.user_designation != null)
                {
                    string RoleName_LB = new UtilityModel().getRoleName_Lb(User.ID_ROLE, content.id_ORGANIZATION);
                    if (!string.IsNullOrEmpty(RoleName_LB))
                    {
                        DesignationUsers = new UtilityModel().getUserDesignationDetails(RoleName_LB, content.id_ORGANIZATION);
                        if (DesignationUsers.Count > 0)
                        {
                            tbl_user MyRank = DesignationUsers.Find(x => x.ID_USER == Convert.ToInt32(content.ID_USER));
                            if (MyRank != null)
                            {
                                ViewBag.UserId = MyRank.USERID;
                                ViewBag.PointsScored = MyRank.PointsScored;
                                ViewBag.Name = MyRank.FullName;
                                ViewBag.MyRank = MyRank.Dense_Rank;

                                // For the Coins 
                                int Coins = 0;

                                var SqlQuery = "SELECT COALESCE(SUM(coins_scored), 0) AS avg_coins_scored FROM tbl_coins_details WHERE id_user = '" + content.ID_USER + "' AND result_in_percentage >= 80 AND attempt_number IN(1, 2) AND status = 'A' AND id_organization = '" + content.id_ORGANIZATION + "'";
                                var TotalCoin = new UtilityModel().getTotalCoin(SqlQuery).ToList();
                                if (TotalCoin.Count > 0)
                                {
                                    ViewBag.CoinPoints = TotalCoin[0];

                                }
                                else
                                {
                                    ViewBag.CoinPoints = TotalCoin[0];
                                }


                                //string SqlQuery = "SELECT distinct id_category,0 as id_category_heading,null as heading_title,0 as id_category_tiles,status FROM tbl_category_associantion WHERE id_category_heading = 1025 UNION SELECT id_category,0 as id_category_heading,null as heading_title,0 as id_category_tiles,status FROM tbl_content_program_mapping WHERE id_category_heading = 1025";
                                //var tblData = new UtilityModel().GetContentCategoryHeadingData(SqlQuery).ToList();
                                //if (tblData != null)
                                //{
                                //    foreach (tbl_category_heading catId in tblData)
                                //    {
                                //        string Id_AssessmentSqlQuery = "select distinct id_assessment from tbl_assessment_user_assignment where id_category = " + catId.id_category + " and id_organization = " + User.ID_ORGANIZATION + " union select distinct id_assessment from tbl_assessment_categoty_mapping where id_category = " + catId.id_category + "";
                                //        var tblAssessmentData = new UtilityModel().IsAssessmentIdCheck(Id_AssessmentSqlQuery).ToList();
                                //        if (tblAssessmentData != null)
                                //        {
                                //            foreach (int id in tblAssessmentData)
                                //            {
                                //                string CheckCoinsQuery = "select id_user, attempt_number,result_in_percentage from tbl_rs_type_qna where id_assessment = " + id + " and id_user = " + User.ID_USER + " and status = 'A' and id_organization = " + User.ID_ORGANIZATION + "";
                                //                //Change for the Coins
                                //                //string CheckCoinsQuery = "select id_user, attempt_number,result_in_percentage from tbl_coins_details  where id_user = " + User.ID_USER+ " and status = 'A' and id_organization = "+User.ID_ORGANIZATION+"";
                                //                var CoinsData = new UtilityModel().getCoinsCount(CheckCoinsQuery);
                                //                if (CoinsData != null)
                                //                {
                                //                    foreach (var item in CoinsData)
                                //                    {
                                //                        if (item.points_scored >= 80 && item.AttemptNo == 1)
                                //                        {
                                //                            Coins = Coins + 100;
                                //                        }
                                //                        else if (item.points_scored >= 80 && item.AttemptNo == 2)
                                //                        {
                                //                            Coins = Coins + 50;
                                //                        }
                                //                    }
                                //                    ViewBag.CoinPoints = Coins;
                                //                }
                                //                else
                                //                {
                                //                    ViewBag.CoinPoints = Coins;
                                //                }
                                //            }
                                //        }
                                //        else
                                //        {
                                //            ViewBag.CoinPoints = Coins;
                                //        }
                                //    }
                                //}
                            }
                            else
                            {
                                ViewBag.UserId = User.USERID;
                                ViewBag.PointsScored = 0;
                                ViewBag.Name = User.FullName;
                                ViewBag.MyRank = 0;
                                ViewBag.CoinPoints = 0;
                            }
                            DesignationUsers = DesignationUsers.OrderByDescending(x => x.PointsScored).ToList();
                            return PartialView(DesignationUsers.Take(20).ToList());
                        }
                        else
                        {
                            ViewBag.UserId = User.USERID;
                            ViewBag.PointsScored = 0;
                            ViewBag.Name = User.FullName;
                            ViewBag.MyRank = 0;
                            ViewBag.CoinPoints = 0;
                            return PartialView();

                        }
                    }
                    else
                    {
                        ViewBag.UserId = User.USERID;
                        ViewBag.PointsScored = 0;
                        ViewBag.Name = User.FullName;
                        ViewBag.MyRank = 0;
                        ViewBag.CoinPoints = 0;
                        return PartialView();
                    }


                }
                else
                {
                    ViewBag.UserId = User.USERID;
                    ViewBag.PointsScored = 0;
                    ViewBag.Name = User.FullName;
                    ViewBag.MyRank = 0;
                    ViewBag.CoinPoints = 0;
                    return PartialView();
                }
            }
            else
            {
                ViewBag.UserId = User.USERID;
                ViewBag.PointsScored = 0;
                ViewBag.Name = User.FullName;
                ViewBag.MyRank = 0;
                ViewBag.CoinPoints = 0;
                return PartialView();
            }

        }
        public ActionResult Feedback(string TextArea, int ContentId, string CategoryName)
        {
            UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
            int output = 0;
            var CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
            try
            {
                string querry = "INSERT INTO tbl_feedback_user_response(" +
                    "org_id,feedback_form_number,feedback_context,context_id," +
                    "context_name,user_id,id_user,question_id,response_value," +
                    "response_score,user_comments,status,updated_datetime)VALUES " +
                    "(" + content.id_ORGANIZATION + "," + 1 + ",null," +
                    "" + ContentId + ",'" + CategoryName + "','" + content.USERID + "'," +
                    "" + content.ID_USER + "," + 1 + ",0,0,'" + TextArea + "','A',NOW())";
                output = new UtilityModel().InsertUserKPIData(querry);
            }
            catch (Exception ex)
            {

            }
            return Json(new { TotalContent = output });
        }

        //For the Feedback Data 

        public List<tbl_feedback_configuration> FeedbackGetData()
        {
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            addFeedbackModel feedbackModel = new addFeedbackModel();
            List<tbl_feedback_configuration> feedbackList = feedbackModel.GetFeedbackData(num);
            if (feedbackList.Count == 0)
            {
                ViewData["feedbackList"] = feedbackList;
                return feedbackList;
            }
            string str1 = feedbackList[0].Image_path;
            string str2 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/feedbackimage/" + str1;
            // string str2 = ConfigurationManager.AppSettings["Feedbackimage"].ToString()  + str1;
            foreach (var feedbackItem in feedbackList)
            {
                feedbackItem.Image_path = str2;
            }

            // Store the modified feedbackList in ViewData
            ViewData["feedbackList"] = feedbackList;
            return feedbackList;
        }

        //For the Video Data 
        public List<tbl_video_configuration> VideoGetData()
        {
            UserSession item = (UserSession)base.HttpContext.Session.Contents["UserSession"];
            int num = Convert.ToInt32(item.id_ORGANIZATION);
            Convert.ToInt32(item.ID_USER);
            addVideoModel videoModel = new addVideoModel();
            List<tbl_video_configuration> videoList = videoModel.GetVideoData(num);
            if (videoList.Count == 0)
            {
                ViewData["videoList"] = videoList;
                return videoList;
            }
            string str1 = videoList[0].Video_name_web;
            string str2 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "Video/" + str1;
            // string str2 = ConfigurationManager.AppSettings["Feedbackimage"].ToString()  + str1;
            foreach (var videoItem in videoList)
            {
                videoItem.Video_name_web = str2;
            }
            string str3 = videoList[0].Video_name_mobile;
            string str4 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "Video/" + str3;
            // string str2 = ConfigurationManager.AppSettings["Feedbackimage"].ToString()  + str1;
            foreach (var videoItem in videoList)
            {
                videoItem.Video_name_mobile = str4;
            }
            // Store the modified feedbackList in ViewData
            ViewData["videoList"] = videoList;
            return videoList;
        }
        public PartialViewResult CertificateDashboard()
        {
            List<Assessment_ID_for_certification> UserDetails = new List<Assessment_ID_for_certification>();
            try
            {
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                if (content != null)
                {
                    int OrgId = Convert.ToInt32(content.id_ORGANIZATION);
                    int ID_User = Convert.ToInt32(content.ID_USER);
                    string UserID = content.USERID;
                    string querry = "select tc.id_assessment,tc.pdfURL,tc.certificateFileName,tc.addedDate as createdDate,ta.assessment_title,tc.id_heading,ch.Heading_title from tbl_certificate_log tc left join tbl_assessment ta on tc.id_assessment = ta.id_assessment left join tbl_category_heading ch on tc.id_heading = ch.id_category_heading where tc.id_user = " + ID_User + " ORDER BY  tc.id_assessment DESC;";

                   // string querry = "select tc.id_assessment,tc.pdfURL,tc.certificateFileName,tc.addedDate as createdDate,ta.assessment_title from tbl_certificate_log tc left join tbl_assessment ta on tc.id_assessment = ta.id_assessment  where tc.id_user = " + ID_User + "";
                    UserDetails = new UtilityModel().getAssessmentCategotyMapping(querry);

                  

                    if (UserDetails.Any())
                    {
                        foreach (Assessment_ID_for_certification certificate in UserDetails)
                        {
                            string Catquerry = "SELECT DISTINCT categoryname FROM tbl_category a " +
                                "LEFT JOIN tbl_assessment_user_assignment b ON a.id_category = b.id_category AND b.status = 'A'" +
                                "LEFT JOIN tbl_assessment_categoty_mapping c ON a.id_category = c.id_category AND c.status = 'A'" +
                                "LEFT JOIN tbl_assessment d ON d.id_assessment = b.id_assessment LEFT JOIN tbl_assessment e ON e.id_assessment = c.id_assessment " +
                                "WHERE a.id_organization = " + OrgId + " AND a.status = 'A' AND ( e.id_assessment = " + certificate.id_assessment + " OR d.id_assessment = " + certificate.id_assessment + ")";
                            string categoryName = new UtilityModel().getCatName(Catquerry);

                            if (categoryName == "")
                            {
                                certificate.assessment_title = "";
                            }
                            else
                            {
                                certificate.assessment_title = categoryName;
                            }
                        }
                    }

                    //string querry_1 = "select tc.id_assessment,tc.pdfURL,tc.certificateFileName,tc.addedDate as createdDate,ta.assessment_title from tbl_certificate_log tc left join tbl_assessment ta on tc.id_assessment = ta.id_assessment left join tbl_category_heading ch on tc.id_heading = ch.id_category_heading where tc.id_user = " + ID_User + "";

                }
            }
            catch (Exception ex)
            {

            }
            return PartialView(UserDetails);
        }
        public ActionResult Download(string filePath)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(filePath).Result;

                if (response.IsSuccessStatusCode)
                {
                    byte[] fileBytes = response.Content.ReadAsByteArrayAsync().Result;
                    return File(fileBytes, "application/pdf", "Certificate.pdf");
                }
                else
                {
                    // Handle error, for example, return a view indicating the download failed.
                    return View("DownloadFailed");
                }
            }
        }




        [HttpPost]
        public ActionResult CoinsMethod(int AssessmentID, int UserID)
        {
            DateTime currentDateAndTime = DateTime.Now;

            // Formatting date and time
            string contact = currentDateAndTime.ToString("yyyy-MM-dd HH:mm:ss");

            var ITName = Session["productsIT"];
            Dictionary<string, string> noOfCoinsAndAttemptNo = new Dictionary<string, string>();

            if (ITName != null)
            {
                noOfCoinsAndAttemptNo["AttemptNo"] = "0";
                // Placeholder for valueloop1

                if (DateTimecal1 == null)
                {
                    DateTimecal1 = contact;
                    if (DateTimecal1 != null)
                    {
                        DateTimecal1 = contact;
                    }
                }
                UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
                int org_id = 0;
                int user_id = 0;
                if (content != null)
                {
                    org_id = Convert.ToInt32(content.id_ORGANIZATION);
                    user_id = Convert.ToInt32(content.ID_USER);
                }
                var s1 = "SELECT * FROM tbl_rs_type_qna WHERE id_user='" + UserID + "' AND id_organization='" + org_id + "' AND id_assessment='" + AssessmentID + "' AND updated_date_time >='" + DateTimecal1 + "'";

                List<tbl_rs_type_qna> resultList123 = new UtilityModel().GetRecordDataID(s1);

                if (resultList123.Count > 0)
                {

                    var SqlQuery2 = "select * from tbl_coins_master where Id_organization =" + org_id + " AND id_assessment =" + AssessmentID + " AND Attempt_no =" + resultList123[0].attempt_number + " AND status ='A'";
                    List<tbl_coins_master> resultList1 = new UtilityModel().GetRecordDataCoinMaster(SqlQuery2);

                    if (resultList1.Count > 0)
                    {

                        if (resultList123.Count > 0 && (double)resultList123[0].result_in_percentage >= resultList1[0].Set_percentage)
                        {
                            foreach (var item in resultList1)
                            {
                                if (resultList123[0].attempt_number == item.Attempt_no && (double)resultList123[0].result_in_percentage >= item.Set_percentage)
                                {
                                    Tempoin1 = Convert.ToString(resultList1[0].Set_Score);
                                    foreach (tbl_rs_type_qna record in resultList123)
                                    {
                                        new UtilityModel().InsertCoin(record, Tempoin1);
                                    }
                                }
                                else
                                {
                                    Tempoin1 = "0";
                                    foreach (tbl_rs_type_qna record in resultList123)
                                    {
                                        new UtilityModel().InsertCoin(record, Tempoin1);
                                    }
                                }

                            }
                            var SqlQuery12 = "SELECT * FROM tbl_coins_details WHERE id_user =" + UserID + " AND id_assessment =" + AssessmentID + " ORDER BY Update_date DESC LIMIT 1";

                            List<tbl_coins_details> resultList2 = new UtilityModel().GetRecordDataCoin(SqlQuery12);
                            if (resultList2.Count > 0)
                            {
                                noOfCoinsAndAttemptNo["Coin"] = Convert.ToString(resultList2[0].coins_scored);
                                noOfCoinsAndAttemptNo["AttemptNo"] = Convert.ToString(resultList2[0].attempt_number);
                                Tempoin1 = null;
                                DateTimecal1 = null;
                                valueloop1 = null;

                            }
                            // Tempoin = null;

                            // Return the dictionary data as JSON
                            //     return Json(new Dictionary<string, string>());
                            return Json(noOfCoinsAndAttemptNo);

                            //if (resultList123[0].attempt_number == 1 && resultList1[0].Attempt_no == 1 && (double)resultList123[0].result_in_percentage >= resultList1[0].Set_percentage)
                            //{
                            //    Tempoin1 = Convert.ToString(resultList1[0].Set_Score);
                            //    foreach (tbl_rs_type_qna record in resultList123)
                            //    {
                            //        new UtilityModel().InsertCoin(record, Tempoin1);
                            //    }
                            //}
                            //else if (resultList123[0].attempt_number == 2 && resultList1[0].Attempt_no == 2 && (double)resultList123[0].result_in_percentage >= resultList1[0].Set_percentage)
                            //{
                            //    Tempoin1 = Convert.ToString(resultList1[0].Set_Score);
                            //    foreach (tbl_rs_type_qna record in resultList123)
                            //    {
                            //        new UtilityModel().InsertCoin(record, Tempoin1);
                            //    }
                            //}
                            //else
                            //{
                            //    Tempoin1 = "0";
                            //    foreach (tbl_rs_type_qna record in resultList123)
                            //    {
                            //        new UtilityModel().InsertCoin(record, Tempoin1);
                            //    }
                            //}
                        }
                        else
                        {
                            Tempoin1 = "0";
                            foreach (tbl_rs_type_qna record in resultList123)
                            {
                                new UtilityModel().InsertCoin(record, Tempoin1);
                            }

                        }
                        //  var SqlQuery123 = "select * from tbl_rs_type_qna where id_user =" + id_user + " AND id_assessment =" + Content_Assessment_ID + " ORDER BY updated_date_time DESC LIMIT 1";

                        // List<tbl_rs_type_qna> resultList1234 = new UtilityModel().GetRecordData(SqlQuery123);

                        var SqlQuery1 = "SELECT * FROM tbl_coins_details WHERE id_user =" + UserID + " AND id_assessment =" + AssessmentID + " ORDER BY Update_date DESC LIMIT 1";

                        List<tbl_coins_details> resultList = new UtilityModel().GetRecordDataCoin(SqlQuery1);
                        if (resultList.Count > 0)
                        {
                            noOfCoinsAndAttemptNo["Coin"] = Convert.ToString(resultList[0].coins_scored);
                            noOfCoinsAndAttemptNo["AttemptNo"] = Convert.ToString(resultList[0].attempt_number);
                            Tempoin1 = null;
                            DateTimecal1 = null;
                            valueloop1 = null;

                        }
                        // Tempoin = null;

                        // Return the dictionary data as JSON
                        //     return Json(new Dictionary<string, string>());
                        return Json(noOfCoinsAndAttemptNo);


                    }
                    var SqlQuery3 = "select * from tbl_coins_master where Id_organization =" + org_id + " AND id_assessment =" + AssessmentID + " AND status ='A'";
                    List<tbl_coins_master> resultList3 = new UtilityModel().GetRecordDataCoinMaster(SqlQuery3);
                    if (resultList3.Count > 0)
                    {
                        Tempoin1 = "0";
                        foreach (tbl_rs_type_qna record in resultList123)
                        {
                            new UtilityModel().InsertCoin(record, Tempoin1);
                        }
                        var SqlQuery4 = "SELECT * FROM tbl_coins_details WHERE id_user =" + UserID + " AND id_assessment =" + AssessmentID + " ORDER BY Update_date DESC LIMIT 1";

                        List<tbl_coins_details> resultList = new UtilityModel().GetRecordDataCoin(SqlQuery4);
                        if (resultList.Count > 0)
                        {
                            noOfCoinsAndAttemptNo["Coin"] = Convert.ToString(resultList[0].coins_scored);
                            noOfCoinsAndAttemptNo["AttemptNo"] = Convert.ToString(resultList[0].attempt_number);
                            Tempoin1 = null;
                            DateTimecal1 = null;
                            valueloop1 = null;

                        }

                        return Json(noOfCoinsAndAttemptNo);

                    }



                    //error for coins
                    //else
                    //{
                    //    Tempoin1 = "0";
                    //    foreach (tbl_rs_type_qna record in resultList123)
                    //    {
                    //        new UtilityModel().InsertCoin(record, Tempoin1);
                    //    }
                    //    var SqlQuery1 = "SELECT * FROM tbl_coins_details WHERE id_user =" + UserID + " AND id_assessment =" + AssessmentID + " ORDER BY Update_date DESC LIMIT 1";

                    //    List<tbl_coins_details> resultList = new UtilityModel().GetRecordDataCoin(SqlQuery1);
                    //    if (resultList.Count > 0)
                    //    {
                    //        noOfCoinsAndAttemptNo["Coin"] = Convert.ToString(resultList[0].coins_scored);
                    //        noOfCoinsAndAttemptNo["AttemptNo"] = Convert.ToString(resultList[0].attempt_number);
                    //        Tempoin1 = null;
                    //        DateTimecal1 = null;
                    //        valueloop1 = null;

                    //    }
                    //    // Tempoin = null;

                    //    // Return the dictionary data as JSON
                    //    // return Json(new Dictionary<string, string>());
                    //    return Json(noOfCoinsAndAttemptNo);


                    //}
                }
                // Rest of your code here...

                // Temporary return for testing
                //return RedirectToAction("SomeAction", "SomeController");
                return Json(noOfCoinsAndAttemptNo);
            }
            else
            {
                // Return a suitable ActionResult if ITName is null
                // return RedirectToAction("SomeAction", "SomeController");
                return Json(noOfCoinsAndAttemptNo);
            }

        }

        [HttpPost]
        public void UserLogRedidect(string Page)
        {
            UserSession userSession = (UserSession)System.Web.HttpContext.Current.Session["UserSession"];

            if (userSession != null)
            {
                // Extract ID_USER and id_ORGANIZATION from the userSession object
                string ID_USER = userSession.ID_USER;
                int id_ORGANIZATION = userSession.id_ORGANIZATION;
                string Page1 = Page;
                // Call AddUserDataLog method with extracted values
                new UtilityModel().AddUserDataLog(ID_USER, id_ORGANIZATION, Page1);
            }
        }

        // for Product Tour  
        //[HttpPost]
        //public ActionResult ProductTour(tbl_product_tour_details model)
        //{

        //    UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //    int org_id = 0;
        //    int user_id = 0;
        //    string page_name = model.Page_name;
        //    org_id = Convert.ToInt32(content.id_ORGANIZATION);
        //    user_id = Convert.ToInt32(content.ID_USER);

        //    bool data1 = new UtilityModel().InsertTourData(org_id, user_id, page_name);

        //    return Json(new { success = data1 });



        //}
        //public ActionResult ProductTourinfo(string page_name)
        //{

        //    UserSession content = (UserSession)this.HttpContext.Session.Contents["UserSession"];
        //    int org_id = 0;
        //    int user_id = 0;

        //    org_id = Convert.ToInt32(content.id_ORGANIZATION);
        //    user_id = Convert.ToInt32(content.ID_USER);
        //    //page_name = Page_name;
        //    bool data = new UtilityModel().GetTourDataExistOrNot(org_id, user_id, page_name);

        //    if (data == false)
        //    {
        //        TempData["tour"] = data;
        //    }
        //    else
        //    {
        //        TempData["tour"] = data;
        //    }


        //    // Return a JSON result indicating the success or failure

        //    return Json(new { success = data });



        //}

    }
}


