using IBHFL.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Mvc;
using System.Web.SessionState;
using System.Web;
using System.Globalization;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.Ajax.Utilities;
using Google.Protobuf.WellKnownTypes;
using RusticiSoftware.HostedEngine.Client;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Web.UI.WebControls;
using System.Linq.Expressions;
using System.Windows.Forms;
using System.Diagnostics.Contracts;
using static IBHFL.Controllers.DashboardController;
using MySql.Data.MySqlClient;

namespace IBHFL.Controllers
{
    [RoutePrefix("api/KPI")]
    public class KPIController : ApiController
    {
        public static string CorebusAPIUrl = ConfigurationManager.AppSettings["CorebusAPIUrl"].ToString();
        private db_m2ostEntities DbContext = new db_m2ostEntities();
        public static string Tempoin = null;
        public static string DateTimecal = null;
        public static string myData = null;
        public static string valueloop = null;

        [Route("PostTestAPI")]
        [HttpPost]
        public IHttpActionResult PostTestAPI(int Content_Assessment_ID)
        {
            return Json("value");
        }

        [Route("CalculateKPIDetails_Assessment")]
        [HttpPost]
        public IHttpActionResult CalculateKPIDetails_Assessment(int Content_Assessment_ID, int id_user, double KPIRawScore, int RightAns, int AttemptNo, string CompletionDate, int TotalQuestions, string AssessmentPlatform = "M2OST")
        {
            List<tbl_user_kpi_data_log> lstUserKPIData = new List<tbl_user_kpi_data_log>();

            try
            {
                ////UserSession content = (UserSession)HttpContext.Current.Session.Contents["UserSession"];
                ////int num1 = 0;
                ////int num2 = 0;
                ////string GameID = "0";
                string UserID = "", SqlQuery = "";
                ////string SqlQuery = "";
                ////tbl_profile tblProfile = new tbl_profile();
                ////tbl_organization tblOrganization = new tbl_organization();
                ////if (content != null)
                ////{
                ////    num1 = Convert.ToInt32(content.id_ORGANIZATION);

                ////    id_user = id_user > 0 ? id_user : Convert.ToInt32(content.ID_USER);
                ////    ////num2 = Convert.ToInt32(content.ID_USER);
                ////    UserID = content.USERID;
                ////    EmpID = content.EMPLOYEEID;

                ////    tblProfile = new UtilityModel().getUserProfileDetails(num2.ToString());
                ////    //////tblOrganization = new UtilityModel().getOrganizationDetails(num1);
                ////}
                ////else
                ////{
                ////    id_user = 11150;
                ////}
                if (id_user > 0)
                {
                    UserID = new UtilityModel().getUserName(id_user);
                }

                SqlQuery = "select DISTINCT l.ID_Scoring_Matrix,l.ID_KPI,l.ID_Assessment_Type,l.Content_Assessment_ID," +
                    "l.ApplyMasterScoreMultipleAttempts,l.ApplyRightAnswerMultipleAttempts,t2.KPI_Name,t2.ID_Organization," +
                    "t2.game_id,kt.KPI_Type AS KPI_Type,kst.KPI_Sub_Type AS KPI_SubType,sl.Scoring_Logic AS Scoring_Logic " +
                    "FROM tbl_kpi_scoring_master_details l " +
                    "INNER JOIN tbl_kpi_master_details t2 on l.ID_KPI = t2.ID_KPI " +
                    "INNER JOIN tbl_kpi_type_details kt on t2.KPI_Type = kt.ID_KPI_Type " +
                    "INNER JOIN tbl_kpi_sub_type_details kst on t2.KPI_SubType = kst.ID_KPI_Sub_Type " +
                    "INNER JOIN tbl_kpi_scoring_logic_details sl on t2.Scoring_Logic = sl.ID_KPI_Scoring_Logic " +
                    "WHERE l.Content_Assessment_ID = '" + Content_Assessment_ID + "' AND l.IsActive='A' AND t2.IsActive='A' AND t2.KPI_Type = 1";

                var tbldata = new UtilityModel().GetKPIMasterRecordData(SqlQuery);

                ////var tbldata = (from l in DbContext.tbl_kpi_scoring_master_details
                ////               join t2 in DbContext.tbl_kpi_master_details on l.ID_KPI equals t2.ID_KPI
                ////               join kt in DbContext.tbl_kpi_type_details on t2.KPI_Type equals kt.ID_KPI_Type
                ////               join kst in DbContext.tbl_kpi_sub_type_details on t2.KPI_SubType equals kst.ID_KPI_Sub_Type
                ////               join sl in DbContext.tbl_kpi_scoring_logic_details on t2.Scoring_Logic equals sl.ID_KPI_Scoring_Logic
                ////               where l.Content_Assessment_ID == Content_Assessment_ID && t2.KPI_Type == 1
                ////               select new
                ////               {
                ////                   l.ID_Scoring_Matrix,
                ////                   l.ID_KPI,
                ////                   l.ID_Assessment_Type,
                ////                   l.Content_Assessment_ID,
                ////                   l.ApplyMasterScoreMultipleAttempts,
                ////                   l.ApplyRightAnswerMultipleAttempts,
                ////                   t2.KPI_Name,
                ////                   t2.ID_Organization,
                ////                   t2.game_id,
                ////                   KPI_Type = kt.KPI_Type,
                ////                   KPI_SubType = kst.KPI_Sub_Type,
                ////                   Scoring_Logic = sl.Scoring_Logic,
                ////               }).ToList();
                string sqlquerry = "select assessment_type,id_organization from tbl_assessment where id_assessment = '" + Content_Assessment_ID + "'";
                tbl_user Assessment_Details = new UtilityModel().getAssessment(sqlquerry);
                if (Assessment_Details.ASSESSMENT_TYPE == 3)
                {

                    string querry = "INSERT INTO tbl_rs_type_qna(id_assessment_log,id_user,id_organization,id_assessment," +
                        "Attempt_number,Total_Question,right_answer_count,wrong_answer_count,result_in_percentage,status,updated_date_time)VALUES (" + 30000000 + "," + id_user + "," + Assessment_Details.ID_ORGANIZATION + "," +
                        "" + Content_Assessment_ID + "," + AttemptNo + "," + TotalQuestions + "," + RightAns + "," + (TotalQuestions - RightAns) + "," + KPIRawScore + ",'A',NOW())";
                    new UtilityModel().InsertUserKPIData(querry);

                    //valueloop = "0";

                    ////
                    //string myData = DashboardController.GlobalVariables.MyGlobalValue;
                    //if (myData != null)
                    //{
                    //    var SqlQuery123 = "select * from tbl_rs_type_qna where id_user =" + id_user + " AND id_assessment =" + Content_Assessment_ID + " ORDER BY updated_date_time DESC LIMIT 1";

                    //    List<tbl_rs_type_qna> resultList123 = new UtilityModel().GetRecordData(SqlQuery123);
                    //    Tempoin = "0";
                    //    foreach (tbl_rs_type_qna record in resultList123)
                    //    {
                    //        new UtilityModel().InsertCoin(record, Tempoin);
                    //    }
                    //}

                    //


                    //Coine For
                    //string myData = DashboardController.GlobalVariables.MyGlobalValue;
                    //if(myData !=null)
                    //{
                    //    Tempoin = "0";
                    //    Dictionary<string, string> noOfCoinsAndAttemptNo = new Dictionary<string, string>();
                    //    var SqlQuery12 = "select * from tbl_rs_type_qna where id_user =" + id_user + " AND id_assessment =" + Content_Assessment_ID + " ORDER BY updated_date_time DESC LIMIT 1";

                    //    List<tbl_rs_type_qna> resultList12 = new UtilityModel().GetRecordData(SqlQuery12);

                    //    if (AttemptNo >= 1)
                    //    {
                    //        var SqlQuery2 = "select * from tbl_coins_master where id_assessment =" + Content_Assessment_ID + " AND Attempt_no =" + AttemptNo + " AND status ='A'";
                    //        List<tbl_coins_master> resultList1 = new UtilityModel().GetRecordDataCoinMaster(SqlQuery2);


                    //        if (resultList1.Count > 0)
                    //        {

                    //            if (resultList12.Count > 0 && (double)resultList12[0].result_in_percentage >= resultList1[0].Set_percentage)
                    //            {
                    //                if (resultList12[0].attempt_number == 1 && resultList1[0].Attempt_no == 1 && (double)resultList12[0].result_in_percentage >= resultList1[0].Set_percentage)
                    //                {
                    //                    Tempoin = Convert.ToString(resultList1[0].Set_Score);
                    //                    foreach (tbl_rs_type_qna record in resultList12)
                    //                    {
                    //                        new UtilityModel().InsertCoin(record, Tempoin);
                    //                    }
                    //                }
                    //                else if (resultList12[0].attempt_number == 2 && resultList1[0].Attempt_no == 2 && (double)resultList12[0].result_in_percentage >= resultList1[0].Set_percentage)
                    //                {
                    //                    Tempoin = Convert.ToString(resultList1[0].Set_Score);
                    //                    foreach (tbl_rs_type_qna record in resultList12)
                    //                    {
                    //                        new UtilityModel().InsertCoin(record, Tempoin);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    Tempoin = "0";
                    //                    foreach (tbl_rs_type_qna record in resultList12)
                    //                    {
                    //                        new UtilityModel().InsertCoin(record, Tempoin);
                    //                    }
                    //                }


                    //            }
                    //            else
                    //            {
                    //                Tempoin = "0";
                    //                foreach (tbl_rs_type_qna record in resultList12)
                    //                {
                    //                    new UtilityModel().InsertCoin(record, Tempoin);
                    //                }
                    //            }


                    //        }
                    //        else
                    //        {

                    //            Tempoin = "0";
                    //            foreach (tbl_rs_type_qna record in resultList12)
                    //            {
                    //                new UtilityModel().InsertCoin(record, Tempoin);
                    //            }
                    //        }

                    //        noOfCoinsAndAttemptNo["Coin"] = Tempoin;
                    //        noOfCoinsAndAttemptNo["AttemptNo"] = Convert.ToString(resultList12[0].attempt_number.Value);



                    //    }
                    //}


                }
                else if (Assessment_Details.ASSESSMENT_TYPE == 4)
                {
                    string querry = "INSERT INTO tbl_rs_type_qna(id_assessment_log,id_user,id_organization,id_assessment," +
                        "Attempt_number,Total_Question,right_answer_count,wrong_answer_count,result_in_percentage,status,updated_date_time)VALUES (" + 40000000 + "," + id_user + "," + Assessment_Details.ID_ORGANIZATION + "," +
                        "" + Content_Assessment_ID + "," + AttemptNo + "," + TotalQuestions + "," + RightAns + "," + (TotalQuestions - RightAns) + "," + KPIRawScore + ",'A',NOW())";
                    new UtilityModel().InsertUserKPIData(querry);

                    string myData = DashboardController.GlobalVariables.MyGlobalValue;

                }

                ////For the Coins
                //string myData = DashboardController.GlobalVariables.MyGlobalValue;
                //var noOfCoinsAndAttemptNo = new Dictionary<string, string>();
                //if (myData != null)
                //{

                //    // int coin = 0;
                //    int attemtno = 0;

                //    // var SqlQuery1 = "select * from tbl_rs_type_qna where id_user =" + id_user + " AND id_assessment = " + Content_Assessment_ID + "  ORDER BY attempt_number DESC LIMIT 1";

                //    var SqlQuery1 = "select * from tbl_rs_type_qna where id_user =" + id_user + " AND id_assessment = " + Content_Assessment_ID + "  ORDER BY updated_date_time DESC LIMIT 1";

                //    List<tbl_rs_type_qna> resultList = new UtilityModel().GetRecordData(SqlQuery1);

                //    if (resultList[0].attempt_number >= 1)
                //    {
                //        var SqlQuery2 = "select * from tbl_coins_master where Id_organization =" + resultList[0].id_organization + " AND id_assessment = " + Content_Assessment_ID + " AND Attempt_no =" + resultList[0].attempt_number + " AND status = 'A'";
                //        List<tbl_coins_master> resultList1 = new UtilityModel().GetRecordDataCoinMaster(SqlQuery2);


                //        if (resultList1.Count > 0)
                //        {

                //            if (resultList.Count > 0 && (double)resultList[0].result_in_percentage >= resultList1[0].Set_percentage)
                //            {
                //                if (resultList[0].attempt_number == 1 && resultList1[0].Attempt_no == 1 && (double)resultList[0].result_in_percentage >= resultList1[0].Set_percentage)
                //                {
                //                    Tempoin = Convert.ToString(resultList1[0].Set_Score);
                //                    foreach (tbl_rs_type_qna record in resultList)
                //                    {
                //                        new UtilityModel().InsertCoin(record, Tempoin);
                //                    }
                //                }
                //                else if(resultList[0].attempt_number == 2 && resultList1[0].Attempt_no == 2 && (double)resultList[0].result_in_percentage >= resultList1[0].Set_percentage)
                //                {
                //                    Tempoin = Convert.ToString(resultList1[0].Set_Score);
                //                    foreach (tbl_rs_type_qna record in resultList)
                //                    {
                //                        new UtilityModel().InsertCoin(record, Tempoin);
                //                    }
                //                }
                //                else
                //                {
                //                    Tempoin = "0";
                //                    foreach (tbl_rs_type_qna record in resultList)
                //                    {
                //                        new UtilityModel().InsertCoin(record, Tempoin);
                //                    }
                //                }


                //            }
                //            else
                //            {
                //               Tempoin = "0";
                //                foreach (tbl_rs_type_qna record in resultList)
                //                {
                //                    new UtilityModel().InsertCoin(record, Tempoin);
                //                }
                //            }


                //        }
                //        else
                //        {

                //            Tempoin = "0";
                //            foreach (tbl_rs_type_qna record in resultList)
                //            {
                //                new UtilityModel().InsertCoin(record, Tempoin);
                //            }
                //        }





                //        //

                //        //if (resultList1.Count > 0)
                //        //{

                //        //    if (resultList1[0].Attempt_no == 1 || resultList1[0].Attempt_no == 2)
                //        //    {

                //        //        if (resultList1[0].Attempt_no == 1)
                //        //        {
                //        //            Tempoin = Convert.ToString(resultList1[0].Set_Score);
                //        //            //set_value= resultList1[0].
                //        //            foreach (tbl_rs_type_qna record in resultList)
                //        //            {
                //        //                new UtilityModel().InsertCoin(record, Tempoin);
                //        //            }


                //        //        }
                //        //        else
                //        //        {
                //        //            Tempoin = Convert.ToString(resultList1[0].Set_Score);
                //        //            foreach (tbl_rs_type_qna record in resultList)
                //        //            {
                //        //                new UtilityModel().InsertCoin(record, Tempoin);
                //        //            }


                //        //        }

                //        //    }




                //        //}
                //        //coin = 3;
                //        //attemtno= 
                //        noOfCoinsAndAttemptNo["Coin"] = Tempoin;
                //        noOfCoinsAndAttemptNo["AttemptNo"] = Convert.ToString(resultList[0].attempt_number.Value);

                //        // Convert car to JSON

                //    }
                //}
                ////////



                // if (tbldata != null)
                //for the Popup
                if (tbldata.Count != 0)
                {
                    foreach (var KPIdata in tbldata)
                    {
                        double? Score = 0;
                        int? MasteryScore = 0;
                        decimal TotalPoints = 0;
                        ////int AttemptNo = 1;
                        int OrgId = KPIdata.ID_Organization;
                        int? GameId = KPIdata.game_id;
                        var ID_KPI = KPIdata.ID_KPI;
                        var KPI_Name = KPIdata.KPI_Name;
                        var ID_Assessment_Type = KPIdata.ID_Assessment_Type;
                        var ID_Scoring_Matrix = KPIdata.ID_Scoring_Matrix;
                        var KPI_Type = KPIdata.KPI_Type;
                        var KPI_SubType = KPIdata.KPI_SubType;
                        var Scoring_Logic = KPIdata.Scoring_Logic;

                        if (KPI_SubType == "Mastery Score")
                        {
                            Score = KPIRawScore;
                            if (Scoring_Logic == "Absolute")
                            {
                                //////SqlQuery = "SELECT l.* FROM tbl_assessment_mastery_score_details l where l.IsActive='A' AND l.ID_Scoring_Matrix = '" + ID_Scoring_Matrix + "' AND l.AttemptNo = '" + AttemptNo + "' AND " + KPIRawScore + " >= l.Score;";
                                SqlQuery = "SELECT l.* FROM tbl_assessment_mastery_score_details l where l.IsActive='A' AND l.ID_Scoring_Matrix = '" + ID_Scoring_Matrix + "' AND l.AttemptNo = '" + AttemptNo + "'";

                                var tblKPIData = new UtilityModel().GetAssessmentMasteryScoreData(SqlQuery);

                                ////var tblKPIData = (from l in DbContext.tbl_assessment_mastery_score_details
                                ////                  where l.ID_Scoring_Matrix == ID_Scoring_Matrix && l.AttemptNo == AttemptNo && Score >= l.Score
                                ////                  select l).FirstOrDefault();

                                if (tblKPIData != null && tblKPIData.Count > 0)
                                {
                                    MasteryScore = tblKPIData.FirstOrDefault().Score;

                                    if (KPIRawScore >= MasteryScore)
                                    {
                                        TotalPoints = tblKPIData.FirstOrDefault().Points;
                                    }
                                }
                            }
                        }

                        if (KPI_SubType == "Right Answer")
                        {
                            Score = RightAns;
                            if (Scoring_Logic == "Absolute")
                            {
                                SqlQuery = "SELECT l.* FROM tbl_assessment_right_answer_details l where l.IsActive='A' AND l.ID_Scoring_Matrix = '" + ID_Scoring_Matrix + "' AND l.AttemptNo = '" + AttemptNo + "';";

                                var tblKPIData = new UtilityModel().GetAssessmentRightAnsData(SqlQuery);

                                //////var tblKPIData = (from l in DbContext.tbl_assessment_right_answer_details
                                //////                  where l.ID_Scoring_Matrix == ID_Scoring_Matrix && l.AttemptNo == AttemptNo
                                //////                  select l).FirstOrDefault();

                                if (tblKPIData != null && tblKPIData.Count > 0)
                                {
                                    TotalPoints = (RightAns * tblKPIData.FirstOrDefault().Points);
                                }
                            }
                        }

                        if (KPI_SubType == "Overall Score Grid")
                        {
                            if (Scoring_Logic == "Grid")
                            {
                                SqlQuery = "SELECT l.* FROM tbl_assessment_grid_details l where l.IsActive='A' AND l.ID_Scoring_Matrix = '" + ID_Scoring_Matrix + "' AND " + KPIRawScore + ">=l.FromScore AND " + KPIRawScore + " <= l.ToScore;";

                                var tblKPIData = new UtilityModel().GetAssessmentGridData(SqlQuery);

                                ////var tblKPIData = (from l in DbContext.tbl_assessment_grid_details
                                ////                  where l.ID_Scoring_Matrix == ID_Scoring_Matrix && Score >= l.FromScore && Score <= l.ToScore
                                ////                  select l).FirstOrDefault();

                                if (tblKPIData != null && tblKPIData.Count > 0)
                                {
                                    TotalPoints = tblKPIData.FirstOrDefault().Points;
                                    Score = KPIRawScore;
                                }
                            }
                        }

                        if (KPI_SubType == "Ontime Completion")
                        {
                            if (AttemptNo == 1)
                            {
                                if (Scoring_Logic == "Absolute")
                                {
                                    SqlQuery = "SELECT l.* FROM tbl_content_asessment_completion_timeframe_details l where l.IsActive='A' AND l.ID_Scoring_Matrix = '" + ID_Scoring_Matrix + "';";

                                    var tblKPIData = new UtilityModel().GetAssessmentCompletionTimeFrameData(SqlQuery);

                                    ////var tblKPIData = (from l in DbContext.tbl_content_asessment_completion_timeframe_details
                                    ////                  where l.ID_Scoring_Matrix == ID_Scoring_Matrix
                                    ////                  select l).FirstOrDefault();

                                    if (tblKPIData != null && tblKPIData.Count > 0)
                                    {
                                        DateTime? StartDate = null, KPICompletionDate = Convert.ToDateTime(CompletionDate), EndDate = null;
                                        int Category = tblKPIData.FirstOrDefault().Category;
                                        var Hrs = Convert.ToDouble(tblKPIData.FirstOrDefault().TimePeriod);
                                        var Points = tblKPIData.FirstOrDefault().Points;

                                        if (Category == 0)
                                        {
                                            TotalPoints = Points;
                                            Score = 1;
                                        }
                                        else
                                        {
                                            SqlQuery = "SELECT l.* FROM tbl_assessment_user_assignment l where l.id_user = '" + id_user + "' AND l.id_organization = '" + OrgId + "' AND l.id_assessment = '" + Content_Assessment_ID + "' ORDER BY id_assessment_user_assignment DESC;";

                                            var tblUserAssignmentAssessmentData = new UtilityModel().GetUserAssignmentAssessmentData(SqlQuery);

                                            //////var tblUserAssignmentAssessmentData = (from l in DbContext.tbl_assessment_user_assignment
                                            //////                                       where l.id_user == id_user && l.id_organization == OrgId && l.id_assessment == Content_Assessment_ID
                                            //////                                       select l).ToList().OrderByDescending(x => x.id_assessment_user_assignment);

                                            if (tblUserAssignmentAssessmentData != null && tblUserAssignmentAssessmentData.Count > 0)
                                            {
                                                var UserAssignmentData = tblUserAssignmentAssessmentData.FirstOrDefault();

                                                StartDate = UserAssignmentData.start_date;
                                                EndDate = UserAssignmentData.expire_date;
                                            }
                                            else
                                            {
                                                SqlQuery = "SELECT l.* FROM tbl_assessment l where l.id_assessment = '" + Content_Assessment_ID + "'";

                                                var tblAssessmentData = new UtilityModel().GetAssessmentData(SqlQuery);

                                                //////var tblAssessmentData = (from l in DbContext.tbl_assessment
                                                //////                         where l.id_assessment == Content_Assessment_ID
                                                //////                         select l).ToList();

                                                if (tblAssessmentData != null && tblAssessmentData.Count > 0)
                                                {
                                                    StartDate = tblAssessmentData.FirstOrDefault().assess_start;
                                                    EndDate = tblAssessmentData.FirstOrDefault().assess_ended;
                                                }
                                            }

                                            if (Category == 1)
                                            {
                                                if (Convert.ToDateTime(StartDate).AddHours(Hrs) > KPICompletionDate)
                                                {
                                                    TotalPoints = Points;
                                                    Score = 1;
                                                }
                                            }

                                            if (Category == 2)
                                            {
                                                if (StartDate != null && KPICompletionDate < EndDate)
                                                {
                                                    TotalPoints = Points;
                                                    Score = 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //////////if (TotalPoints > 0)
                        //////////{

                        int isSaved = 0;
                        var CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
                        SqlQuery = "INSERT INTO tbl_user_kpi_data_log(id_user,ID_KPI,KPI_Name,game_id,assessment_platform," +
                            "ID_Scoring_Matrix,ID_Assessment_Type,Content_Assessment_ID,AttemptNo,mastery_score," +
                            "scoring_value,points_scored,IsActive,created_by,created_date,TotalQuestions,UserID) " +
                            "VALUES ('" + id_user + "','" + ID_KPI + "','" + KPI_Name + "','" + GameId + "','" + AssessmentPlatform + "'," +
                            "'" + ID_Scoring_Matrix + "','" + ID_Assessment_Type + "','" + Content_Assessment_ID + "','" + AttemptNo + "'," +
                            "'" + MasteryScore + "','" + Score + "','" + TotalPoints + "','A','" + id_user + "','" + CreatedDate + "','" + TotalQuestions + "','" + UserID + "');";

                        isSaved = new UtilityModel().InsertUserKPIData(SqlQuery);

                        tbl_user_kpi_data_log tblUserKPIData = new tbl_user_kpi_data_log();

                        tblUserKPIData.id_user = id_user;
                        tblUserKPIData.ID_KPI = ID_KPI;
                        tblUserKPIData.KPI_Name = KPI_Name;
                        tblUserKPIData.game_id = GameId;
                        tblUserKPIData.assessment_platform = AssessmentPlatform;
                        tblUserKPIData.ID_Scoring_Matrix = ID_Scoring_Matrix;
                        tblUserKPIData.ID_Assessment_Type = ID_Assessment_Type;
                        tblUserKPIData.Content_Assessment_ID = Content_Assessment_ID;
                        tblUserKPIData.AttemptNo = AttemptNo;

                        tblUserKPIData.mastery_score = MasteryScore;

                        tblUserKPIData.scoring_value = Score;
                        tblUserKPIData.points_scored = TotalPoints;
                        tblUserKPIData.IsActive = "A";
                        tblUserKPIData.UserID = UserID;
                        tblUserKPIData.created_by = id_user;
                        tblUserKPIData.created_date = Convert.ToDateTime(CreatedDate);
                        tblUserKPIData.KPI_SubType = KPI_SubType;
                        tblUserKPIData.TotalQuestions = TotalQuestions;

                        if (AssessmentPlatform == "M2OST")
                        {
                            tblUserKPIData.IsRecordProcessed = true;
                        }

                        lstUserKPIData.Add(tblUserKPIData);

                        if (isSaved == 1)
                        {
                            int GameID = new UtilityModel().getGameId(id_user);
                            int RoleID = new UtilityModel().getRoleID(id_user);
                            string Kpi_Role_Name = new UtilityModel().getRoleName(RoleID, KPI_Name);
                            AddPointsAPI(Kpi_Role_Name, TotalPoints.ToString(), GameID.ToString(), UserID);
                            //AddPointsAPI(KPI_Name, TotalPoints.ToString(), GameId.ToString(), UserID);
                        }
                        //////}
                    }
                }
                //else
                //{

                //    Tempoin = "0";
                //    return Json(noOfCoinsAndAttemptNo);

                //}
            }
            catch (Exception ex)
            {

            }

            return Json(lstUserKPIData);
        }


        [Route("CalculateInduction")]
        [HttpPost]
        public IHttpActionResult CalculateInduction(int AssessmentID, int UserID, string contact)
        {
            myData = DashboardController.GlobalVariables.MyGlobalValue;
            Dictionary<string, string> noOfCoinsAndAttemptNo = new Dictionary<string, string>();

            if (myData != null)
            {

                // noOfCoinsAndAttemptNo["Coin"] = Tempoin;
                noOfCoinsAndAttemptNo["AttemptNo"] = "0";
                var a = 0;
                string b = contact;
                if (DateTimecal == null)
                {
                    DateTimecal = contact;
                    if (DateTimecal != null)
                    {
                        DateTimecal = contact;

                    }

                }

                if (valueloop != null)
                {
                    var s1 = "SELECT * FROM tbl_rs_type_qna WHERE id_user='" + UserID + "' AND id_assessment='" + AssessmentID + "' AND updated_date_time >='" + DateTimecal + "'";
                    List<tbl_rs_type_qna> resultList123 = new UtilityModel().GetRecordDataID(s1);

                    if (resultList123.Count > 0)
                    {

                        var SqlQuery2 = "select * from tbl_coins_master where Id_organization =" + resultList123[0].id_organization + " AND id_assessment =" + AssessmentID + " AND Attempt_no =" + resultList123[0].attempt_number + " AND status ='A'";
                        List<tbl_coins_master> resultList1 = new UtilityModel().GetRecordDataCoinMaster(SqlQuery2);

                        if (resultList1.Count > 0)
                        {

                            if (resultList123.Count > 0 && (double)resultList123[0].result_in_percentage >= resultList1[0].Set_percentage)
                            {
                                if (resultList123[0].attempt_number == 1 && resultList1[0].Attempt_no == 1 && (double)resultList123[0].result_in_percentage >= resultList1[0].Set_percentage)
                                {
                                    Tempoin = Convert.ToString(resultList1[0].Set_Score);
                                    foreach (tbl_rs_type_qna record in resultList123)
                                    {
                                        new UtilityModel().InsertCoin(record, Tempoin);
                                    }
                                }
                                else if (resultList123[0].attempt_number == 2 && resultList1[0].Attempt_no == 2 && (double)resultList123[0].result_in_percentage >= resultList1[0].Set_percentage)
                                {
                                    Tempoin = Convert.ToString(resultList1[0].Set_Score);
                                    foreach (tbl_rs_type_qna record in resultList123)
                                    {
                                        new UtilityModel().InsertCoin(record, Tempoin);
                                    }
                                }
                                else
                                {
                                    Tempoin = "0";
                                    foreach (tbl_rs_type_qna record in resultList123)
                                    {
                                        new UtilityModel().InsertCoin(record, Tempoin);
                                    }
                                }
                            }
                            else
                            {
                                Tempoin = "0";
                                foreach (tbl_rs_type_qna record in resultList123)
                                {
                                    new UtilityModel().InsertCoin(record, Tempoin);
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
                                Tempoin = null;
                                DateTimecal = null;
                                valueloop = null;
                                GlobalVariables.MyGlobalValue = "Induction Training";
                            }
                            // Tempoin = null;

                            // Return the dictionary data as JSON
                            return Json(noOfCoinsAndAttemptNo);

                        }
                        else
                        {
                            Tempoin = "0";
                            foreach (tbl_rs_type_qna record in resultList123)
                            {
                                new UtilityModel().InsertCoin(record, Tempoin);
                            }
                            var SqlQuery1 = "SELECT * FROM tbl_coins_details WHERE id_user =" + UserID + " AND id_assessment =" + AssessmentID + " ORDER BY Update_date DESC LIMIT 1";

                            List<tbl_coins_details> resultList = new UtilityModel().GetRecordDataCoin(SqlQuery1);
                            if (resultList.Count > 0)
                            {
                                noOfCoinsAndAttemptNo["Coin"] = Convert.ToString(resultList[0].coins_scored);
                                noOfCoinsAndAttemptNo["AttemptNo"] = Convert.ToString(resultList[0].attempt_number);
                                Tempoin = null;
                                DateTimecal = null;
                                valueloop = null;
                                GlobalVariables.MyGlobalValue = "Induction Training";
                            }
                            // Tempoin = null;

                            // Return the dictionary data as JSON
                            return Json(noOfCoinsAndAttemptNo);


                        }
                    }
                }



            }


            //var SqlQuery123 = "select * from tbl_rs_type_qna where id_user =" + UserID + " AND id_assessment =" + AssessmentID + " ORDER BY updated_date_time DESC LIMIT 1";

            // List<tbl_rs_type_qna> resultList123 = new UtilityModel().GetRecordData(SqlQuery123);
            //if (resultList123[0].updated_date_time >= formattedDate + formattedTime)
            //{

            //}


            //try
            //{
            //    if (Tempoin != null)
            //    {
            //        var SqlQuery1 = "SELECT * FROM tbl_coins_details WHERE id_user =" + UserID + " AND id_assessment =" + AssessmentID + " ORDER BY Update_date DESC LIMIT 1";

            //        List<tbl_coins_details> resultList = new UtilityModel().GetRecordDataCoin(SqlQuery1);
            //        if (resultList.Count > 0)
            //        {
            //            noOfCoinsAndAttemptNo["Coin"] = Convert.ToString(resultList[0].coins_scored);
            //            noOfCoinsAndAttemptNo["AttemptNo"] = Convert.ToString(resultList[0].attempt_number);
            //            Tempoin = null;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    // Handle exceptions appropriately, at least log them
            //    Console.WriteLine(ex.Message);
            //}

            // Tempoin = null;

            // Return the dictionary data as JSON
            return Json(noOfCoinsAndAttemptNo);
        }






        [Route("CalculateKPIDetails_Content")]
        public IHttpActionResult CalculateKPIDetails_Content(int Content_Assessment_ID, int id_user, int KPIRawScore)
        {
            var tbldata = (from l in DbContext.tbl_kpi_scoring_master_details
                           join t2 in DbContext.tbl_kpi_master_details on l.ID_KPI equals t2.ID_KPI
                           join kt in DbContext.tbl_kpi_type_details on t2.KPI_Type equals kt.ID_KPI_Type
                           join kst in DbContext.tbl_kpi_sub_type_details on t2.KPI_SubType equals kst.ID_KPI_Sub_Type
                           join sl in DbContext.tbl_kpi_scoring_logic_details on t2.Scoring_Logic equals sl.ID_KPI_Scoring_Logic
                           where l.Content_Assessment_ID == Content_Assessment_ID && t2.KPI_Type == 2
                           select new
                           {
                               l.ID_Scoring_Matrix,
                               l.ID_KPI,
                               l.ID_Assessment_Type,
                               l.Content_Assessment_ID,
                               l.ApplyMasterScoreMultipleAttempts,
                               l.ApplyRightAnswerMultipleAttempts,
                               t2.KPI_Name,
                               t2.ID_Organization,
                               KPI_Type = kt.KPI_Type,
                               KPI_SubType = kst.KPI_Sub_Type,
                               Scoring_Logic = sl.Scoring_Logic
                           }).ToList();

            if (tbldata != null)
            {
                foreach (var KPIdata in tbldata)
                {
                    int Score = KPIRawScore;
                    decimal TotalPoints = 0;
                    int AttemptNo = 1;
                    var ID_KPI = KPIdata.ID_KPI;
                    var KPI_Name = KPIdata.KPI_Name;
                    var ID_Assessment_Type = KPIdata.ID_Assessment_Type;
                    var ID_Scoring_Matrix = KPIdata.ID_Scoring_Matrix;
                    var KPI_Type = KPIdata.KPI_Type;
                    var KPI_SubType = KPIdata.KPI_SubType;
                    var Scoring_Logic = KPIdata.Scoring_Logic;

                    if (KPI_SubType == "Completion")
                    {
                        if (Scoring_Logic == "Absolute")
                        {
                            var tblKPIData = (from l in DbContext.tbl_content_completion_notime_details
                                              where l.ID_Scoring_Matrix == ID_Scoring_Matrix
                                              select l).FirstOrDefault();

                            TotalPoints = tblKPIData.Points;
                        }
                    }

                    if (KPI_SubType == "Ontime Completion")
                    {
                        if (Scoring_Logic == "Absolute")
                        {
                            var tblKPIData = (from l in DbContext.tbl_content_asessment_completion_timeframe_details
                                              where l.ID_Scoring_Matrix == ID_Scoring_Matrix
                                              select l).FirstOrDefault();

                            //Hit APi only when the content is 100% complete
                        }
                    }

                    if (TotalPoints > 0)
                    {
                        int isSaved = 0;
                        tbl_user_kpi_data_log tblUserKPIData = new tbl_user_kpi_data_log();

                        tblUserKPIData.id_user = id_user;
                        tblUserKPIData.ID_KPI = ID_KPI;
                        tblUserKPIData.KPI_Name = KPI_Name;
                        tblUserKPIData.game_id = 1;
                        tblUserKPIData.assessment_platform = "M2OST";
                        tblUserKPIData.ID_Scoring_Matrix = ID_Scoring_Matrix;
                        tblUserKPIData.ID_Assessment_Type = ID_Assessment_Type;
                        tblUserKPIData.Content_Assessment_ID = Content_Assessment_ID;
                        tblUserKPIData.AttemptNo = AttemptNo;
                        tblUserKPIData.mastery_score = 0;
                        tblUserKPIData.scoring_value = 0;
                        tblUserKPIData.points_scored = TotalPoints;
                        tblUserKPIData.IsActive = "A";
                        tblUserKPIData.created_by = 1;
                        tblUserKPIData.created_date = DateTime.UtcNow;
                        DbContext.tbl_user_kpi_data_log.Add(tblUserKPIData);
                        isSaved = DbContext.SaveChanges();

                        if (isSaved == 1)
                        {

                        }
                    }
                }
            }

            return Json("value");
        }

        public void AddPointsAPI(string KPIName, string Score, string GameID, string UserID)
        {
            //////////UserSession content = (UserSession)HttpContext.Current.Session.Contents["UserSession"];
            //////////int num1 = 0;
            //////////int num2 = 0;
            //////////////string GameID = "0";
            //////////string UserID = "", EmpID = "";
            //////////tbl_profile tblProfile = new tbl_profile();
            //////////tbl_organization tblOrganization = new tbl_organization();
            //////////if (content != null)
            //////////{
            //////////    num1 = Convert.ToInt32(content.id_ORGANIZATION);
            //////////    num2 = Convert.ToInt32(content.ID_USER);
            //////////    UserID = content.USERID;
            //////////    EmpID = content.EMPLOYEEID;

            //////////    tblProfile = new UtilityModel().getUserProfileDetails(num2.ToString());
            //////////    tblOrganization = new UtilityModel().getOrganizationDetails(num1);
            //////////}

            //////if (tblOrganization != null)
            //////{
            //////    GameID = tblOrganization.CONTACTNUMBER;
            //////}

            //////UserID = "GOP_IN501";
            //////GameID = "183";

            ////GetPostAPIDetails("https://coroebus.in/coroebus-tgc-api-levels/dashboard/produce_1", UserID, GameID, "1", "1", "W", KPIName, Score);
            //Adithya 13-01-2024  //GetPostAPIDetails(CorebusAPIUrl + "dashboard/produce_1", UserID, GameID, "1", "1", "W", KPIName, Score);
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
                    ////AddPointsAPIDetails("https://www.coroebus.in/coroebus-tgc-api-levels//KpiPointsProcessing/add_points", RootobjectResponse, KPIName, Score);
                    AddPointsAPIDetails(CorebusAPIUrl + "KpiPointsProcessing/add_points", RootobjectResponse, KPIName, Score);
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

        //////public List<tbl_user_kpi_data_log> GetUserKPILogDetails(int AssessmentID, int UserID, int GameID, int TotalQuestions, int AttemptNo, string AssessmentPlatform)
        //////{
        //////    List<tbl_user_kpi_data_log> UserKPIData = new List<tbl_user_kpi_data_log>();
        //////    try
        //////    {
        //////        string SqlQuery = "SELECT * FROM tbl_user_kpi_data_log where IsActive='A' AND Content_Assessment_ID='" + AssessmentID + "' AND id_user='" + UserID + "' AND game_id='" + GameID + "' AND AttemptNo='" + AttemptNo + "';";

        //////        UserKPIData = new UtilityModel().GetUserKPILogDetailsData(SqlQuery);
        //////    }
        //////    catch (Exception ex)
        //////    {

        //////    }

        //////    return UserKPIData;
        //////}

        [Route("GetUserKPILogDetails")]
        [HttpPost]
        public List<tbl_user_kpi_data_log> GetUserKPILogDetails(int AssessmentID, int UserID)
        {
            List<tbl_user_kpi_data_log> UserKPIData = new List<tbl_user_kpi_data_log>();
            try
            {
                string SqlQuery = "select kp.*,kst.KPI_Sub_Type AS KPI_SubType FROM tbl_user_kpi_data_log kp INNER JOIN tbl_kpi_scoring_master_details l on kp.ID_Scoring_Matrix=l.ID_Scoring_Matrix INNER JOIN tbl_kpi_master_details t2 on l.ID_KPI = t2.ID_KPI INNER JOIN tbl_kpi_sub_type_details kst on t2.KPI_SubType = kst.ID_KPI_Sub_Type where kp.IsActive='A' AND kp.Content_Assessment_ID='" + AssessmentID + "' AND kp.id_user='" + UserID + "' AND kp.IsRecordProcessed='0';";

                UserKPIData = new UtilityModel().GetUserKPILogDetailsData(SqlQuery);

                SqlQuery = "SET SQL_SAFE_UPDATES = 0;UPDATE tbl_user_kpi_data_log set IsRecordProcessed=1 WHERE IsActive='A' AND Content_Assessment_ID='" + AssessmentID + "' AND id_user='" + UserID + "' AND IsRecordProcessed=0;";

                int isSaved = new UtilityModel().InsertUserKPIData(SqlQuery);
            }
            catch (Exception ex)
            {

            }

            return UserKPIData;
        }



        public List<tbl_assessment> AssessmentDetails_Content(int CategoryID)
        {
            try
            {
                int OrgID = 0, id_user = 0;
                UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

                if (userData != null)
                {
                    OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                    id_user = Convert.ToInt32(userData.ID_USER);
                }

                ////var tblAssessmentData = (from a in DbContext.tbl_assessment_categoty_mapping
                ////                         join b in DbContext.tbl_category on a.id_category equals b.ID_CATEGORY
                ////                         join c in DbContext.tbl_assessment on a.id_assessment equals c.id_assessment
                ////                         where b.ID_ORGANIZATION == OrgID && b.STATUS == "A" && b.ID_CATEGORY == CategoryID
                ////                         select c).ToList();

                //string SqlQuery = "SELECT c.* FROM tbl_assessment_categoty_mapping a JOIN tbl_category b on a.id_category=b.ID_CATEGORY " +
                //    "JOIN tbl_assessment c on a.id_assessment=c.id_assessment " +
                //    "WHERE b.id_organization = '" + OrgID + "' and b.status = 'A'  and b.id_category = '" + CategoryID + "'";

                string SqlQuery = "SELECT a.* FROM tbl_assessment a ,tbl_assessment_user_assignment b WHERE id_category = " + CategoryID + " AND id_user = " + userData.ID_USER + " AND expire_date >=current_date() AND a.id_organization = " + userData.id_ORGANIZATION + " " +
                    "AND a.id_assessment = b.id_assessment " +
                    "UNION " +
                    "SELECT c.* FROM tbl_assessment_categoty_mapping a , tbl_category b , tbl_assessment c WHERE a.id_category = b.id_category  AND a.id_assessment = c.id_assessment  " +
                    "AND b.id_organization = " + userData.id_ORGANIZATION + " AND b.status = 'A'  AND b.id_category = " + CategoryID + "";



                var tblAssessmentData = new UtilityModel().AssessmentDetails_ContentData(SqlQuery);

                return tblAssessmentData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<tbl_category> ContentDetailsByCategory(int CategoryId)
        {
            try
            {
                int OrgID = 0;
                UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

                if (userData != null)
                {
                    OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                }

                //////var tblData = (from a in DbContext.tbl_category
                //////               join b in DbContext.tbl_content_organization_mapping on a.ID_CATEGORY equals b.id_category
                //////               join c in DbContext.tbl_content on b.id_content equals c.ID_CONTENT
                //////               where a.ID_ORGANIZATION == OrgID && c.status == "A" && b.id_category == CategoryId
                //////               select a).ToList();

                string SqlQuery = "SELECT a.CATEGORYNAME,0 AS ID_PARENT,COUNT(*) AS COUNT_REQUIRED FROM tbl_category a " +
                    "JOIN tbl_content_organization_mapping b on a.id_category=b.ID_CATEGORY " +
                "JOIN tbl_content c on b.id_content=c.id_content " +
                "WHERE a.id_organization = '" + OrgID + "' and c.status = 'A'  and a.id_category = '" + CategoryId + "' GROUP BY a.CATEGORYNAME";

                var tblData = new UtilityModel().GetContentDetailsByCategoryData(SqlQuery);

                return tblData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<tbl_category> ContentReadDetailsByCategory(int CategoryId)
        {
            try
            {
                int OrgID = 0, id_user = 0;
                UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

                if (userData != null)
                {
                    OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                    id_user = Convert.ToInt32(userData.ID_USER);
                }

                ////var tblData = (from a in DbContext.tbl_content_counters
                ////               join c in DbContext.tbl_content_organization_mapping on a.id_content equals c.id_content
                ////               join b in DbContext.tbl_category on c.id_category equals b.ID_CATEGORY
                ////               where b.ID_ORGANIZATION == OrgID && a.id_user == id_user && b.ID_CATEGORY == CategoryId
                ////               select b).ToList();

                string SqlQuery = "SELECT b.CATEGORYNAME,a.id_user AS ID_PARENT,COUNT(distinct a.id_content) AS COUNT_REQUIRED FROM tbl_content_counters a " +
                    "JOIN tbl_content_organization_mapping c on a.id_content = c.id_content " +
                "JOIN tbl_category b on b.ID_CATEGORY=c.id_category " +
                "WHERE b.id_organization = '" + OrgID + "' and a.id_user = '" + id_user + "' and b.ID_CATEGORY = '" + CategoryId + "' GROUP BY b.CATEGORYNAME,a.id_user";

                var tblData = new UtilityModel().GetContentDetailsByCategoryData(SqlQuery);

                return tblData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public double CategoryHeadersCount(string CategoryHeadingId)
        {
            try
            {
                int OrgID = 0, id_user = 0;
                UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

                if (userData != null)
                {
                    OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                    id_user = Convert.ToInt32(userData.ID_USER);
                }

                ////////var tblData = (from a in DbContext.tbl_category_heading
                ////////               join c in DbContext.tbl_category_associantion on a.id_category_heading equals c.id_category_heading
                ////////               join b in DbContext.tbl_category on c.id_category equals b.ID_CATEGORY
                ////////               where b.ID_ORGANIZATION == OrgID && a.id_category_heading == CategoryHeadingId
                ////////               select new
                ////////               {
                ////////                   a.id_category_heading,
                ////////                   a.Heading_title,
                ////////                   b.ID_CATEGORY,
                ////////                   b.CATEGORYNAME
                ////////               }).OrderBy(x => x.id_category_heading).ToList();

                //var SqlQuery = "SELECT distinct 0 as id_category, a.id_category_heading , a.heading_title, b.id_category AS id_category_tiles ,b.categoryname AS status FROM tbl_category_heading a, " +
                //    "  tbl_content_program_mapping c,tbl_category b where a.id_category_heading = c.id_category_heading " +
                //    "  and b.id_category = c.ID_CATEGORY and " +
                //    " b.ID_ORGANIZATION ='" + OrgID + "'  AND a.id_category_heading ='" + CategoryHeadingId + "' AND id_user = " + id_user + " " +
                //    "UNION " +
                //    "SELECT distinct 0 as id_category, a.id_category_heading , a.heading_title, b.id_category AS id_category_tiles ,b.categoryname AS status FROM tbl_category_heading a, " +
                //    " tbl_category_associantion c,tbl_category b where a.id_category_heading = c.id_category_heading and " +
                //    " c.id_category = b.ID_CATEGORY and " +
                //    " b.ID_ORGANIZATION ='" + OrgID + "'  AND a.id_category_heading ='" + CategoryHeadingId + "' ";

                //
                var SqlQuery = " SELECT DISTINCT 0 AS id_category, a.id_category_heading, a.heading_title, b.id_category AS id_category_tiles, b.categoryname AS status FROM tbl_category_heading a, tbl_content_program_mapping c, tbl_category b WHERE a.id_category_heading = c.id_category_heading AND b.id_category = c.ID_CATEGORY AND b.ID_ORGANIZATION = '" + OrgID + "'  AND a.id_category_heading = '" + CategoryHeadingId + "'  AND id_user = " + id_user + "  AND DATE(c.expiry_date) >= CURDATE() UNION SELECT DISTINCT 0 AS id_category, a.id_category_heading, a.heading_title, b.id_category AS id_category_tiles, b.categoryname AS status FROM tbl_category_heading a, tbl_category_associantion c, tbl_category b WHERE a.id_category_heading = c.id_category_heading AND c.id_category = b.ID_CATEGORY AND b.ID_ORGANIZATION = '" + OrgID + "'  AND a.id_category_heading = '" + CategoryHeadingId + "' ";



                var tblData = new UtilityModel().GetContentCategoryHeadingData(SqlQuery).ToList();

                double AvgPer = 0;
                double TotalPer = 0;
                if (tblData.Count > 0)
                {

                    foreach (var Data in tblData)
                    {
                        int Category = Convert.ToInt32(Data.id_category_tiles);

                        var CategoryCompletionStatusData = CalculateCategoryCompletionStatus(Category);

                        if (CategoryCompletionStatusData != null)
                        {
                            TotalPer = TotalPer + CategoryCompletionStatusData.TotalCompletionPer;
                        }
                    }
                    AvgPer = Math.Round((TotalPer / tblData.Count), 2);
                }

                return AvgPer;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public CourseCompletion CalculateCategoryCompletionStatus(int CategoryId)
        {
            int OrgID = 0, id_user = 0;
            double CourseCompletedPer = 0, AssessmentCompletedPer = 0, CompletionCalcPer = 1;
            string StartDate = "", ExpiryDate = "";
            UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];
            CourseCompletion cc = new CourseCompletion();

            if (userData != null)
            {
                OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                id_user = Convert.ToInt32(userData.ID_USER);
            }

            var AssessmentData = AssessmentDetails_Content(CategoryId);

            // string SqlQuery = "select * from tbl_content_program_mapping where id_organization='" + OrgID + "' AND id_user = '" + id_user + "' AND id_category = '" + CategoryId + "' AND status = 'A'";
            string SqlQuery = "select * from tbl_content_program_mapping where id_organization = '" + OrgID + "' AND id_user = '" + id_user + "' AND id_category = '" + CategoryId + "'AND status = 'A' and expiry_date >= current_date()";

            var tblContentData = new UtilityModel().GetContentProgramMappingDatesData(SqlQuery);

            if (tblContentData != null && tblContentData.Count > 0)
            {
                StartDate = tblContentData.FirstOrDefault().start_date.ToString();
                ExpiryDate = tblContentData.FirstOrDefault().expiry_date.ToString();
            }

            if (AssessmentData != null && AssessmentData.Count > 0)
            {
                int AssessmentID = AssessmentData.FirstOrDefault().id_assessment;

                //////var tblContentData = (from a in DbContext.tbl_content_program_mapping
                //////                      where a.id_organization == OrgID && a.id_user == id_user && a.id_category == CategoryId
                //////                      select a).FirstOrDefault();

                //Assessment Completion Per
                if (AssessmentID > 0)
                {
                    //SqlQuery = "SELECT l.* FROM tbl_assessment_user_assignment l where l.id_user = '" + id_user + "' AND l.id_organization = '" + OrgID + "' AND l.id_assessment = '" + AssessmentID + "' ORDER BY id_assessment_user_assignment DESC;";

                    SqlQuery = " SELECT l.*FROM tbl_assessment_user_assignment l where l.id_user = '" + id_user + "' AND l.id_organization = '" + OrgID + "' AND l.id_assessment =  '" + AssessmentID + "' and expire_date >= current_date() ORDER BY id_assessment_user_assignment DESC";



                    var tblUserAssignmentAssessmentData = new UtilityModel().GetUserAssignmentAssessmentData(SqlQuery);

                    if (tblUserAssignmentAssessmentData != null && tblUserAssignmentAssessmentData.Count > 0)
                    {
                        var UserAssignmentData = tblUserAssignmentAssessmentData.FirstOrDefault();

                        StartDate = StartDate == "" ? UserAssignmentData.start_date.ToString() : StartDate;
                        ExpiryDate = UserAssignmentData.expire_date.ToString();
                    }
                    else
                    {
                        SqlQuery = "SELECT l.* FROM tbl_assessment l where l.id_assessment = '" + AssessmentID + "'";

                        // SqlQuery = "select * from tbl_content_program_mapping where id_organization = '" + OrgID + "' AND id_user = '" + id_user + "' AND id_category = '" + CategoryId + "'AND status = 'A' and expiry_date >= current_date()";


                        var tblAssessmentData = new UtilityModel().GetAssessmentData(SqlQuery);

                        if (tblAssessmentData != null && tblAssessmentData.Count > 0)
                        {
                            StartDate = StartDate == "" ? tblAssessmentData.FirstOrDefault().assess_start.ToString() : StartDate;
                            ExpiryDate = tblAssessmentData.FirstOrDefault().assess_ended.ToString();
                        }
                    }

                    CompletionCalcPer = 0.5;
                    //////int AssessmentCompleted = (from a in DbContext.tbl_assessment_audit
                    //////                           where a.id_assessment == AssessmentID
                    //////                           select a).ToList().Count();

                    //SqlQuery = "select COUNT(*) AS count FROM tbl_assessment_audit WHERE id_assessment='" + AssessmentID + "' AND id_user='" + id_user + "'";
                    //Change for the Non Scroing Assesment
                    SqlQuery = "select count(*) AS count from tbl_rs_type_qna where  id_assessment='" + AssessmentID + "' AND id_user='" + id_user + "'";

                    int AssessmentCompleted = new UtilityModel().getRecordCount(SqlQuery);

                    if (AssessmentCompleted > 0)
                    {
                        //100 % completed;
                        AssessmentCompletedPer = 100;
                    }
                    else
                    {
                        SqlQuery = "select COUNT(*) AS count FROM tbl_user_kpi_data_log WHERE Content_Assessment_ID='" + AssessmentID + "' AND id_user='" + id_user + "'";

                        AssessmentCompleted = new UtilityModel().getRecordCount(SqlQuery);

                        if (AssessmentCompleted > 0)
                        {
                            //100 % completed;
                            AssessmentCompletedPer = 100;
                        }
                    }
                }
            }

            //Course Completion Per
            var ContentReadDetailsData = ContentReadDetailsByCategory(CategoryId);
            var ContentDetailsByCategoryData = ContentDetailsByCategory(CategoryId);

            if (ContentReadDetailsData != null && ContentDetailsByCategoryData != null && ContentReadDetailsData.Count > 0 && ContentDetailsByCategoryData.Count > 0)
            {
                int TotalContentLength = Convert.ToInt32(ContentDetailsByCategoryData.FirstOrDefault().COUNT_REQUIRED);
                int ReadContentLength = Convert.ToInt32(ContentReadDetailsData.FirstOrDefault().COUNT_REQUIRED);

                CourseCompletedPer = (ReadContentLength / TotalContentLength) * 100;
            }

            //get difference of Start Date and End Date for Duration
            var Duration = ExpiryDate == "" ? 0 : (Convert.ToDateTime(ExpiryDate).Date - Convert.ToDateTime(StartDate).Date).Days;

            //get difference of Expiry Date and End Date for Days to Complete
            var DaysToComplete = ExpiryDate == "" ? 0 : (Convert.ToDateTime(ExpiryDate).Date - DateTime.UtcNow.Date).Days;

            var TotalCompletion = (CourseCompletedPer * CompletionCalcPer) + (AssessmentCompletedPer * 0.5);

            cc.StartDate = StartDate == "" ? null : Convert.ToDateTime(StartDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            cc.ExpiryDate = ExpiryDate == "" ? null : Convert.ToDateTime(ExpiryDate).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            cc.DaysToComplete = DaysToComplete;
            cc.TotalCompletionPer = TotalCompletion;
            cc.Duration = Duration;

            return cc;
        }


        public List<MoodleAssessmentStatus> GetMoodleAssessmentStatus(int categoryId)
        {
            List<MoodleAssessmentStatus> moodleAssessmentStatusList = new List<MoodleAssessmentStatus>();

            using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT id_user, id_organization, attempt_number, moodle_assessment_status FROM tbl_rs_type_qna WHERE id_assessment = @CategoryId";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())  // Iterate through all rows
                            {
                                MoodleAssessmentStatus status = new MoodleAssessmentStatus
                                {
                                    IdUser = reader.GetInt32("id_user"),
                                    IdOrganization = reader.GetInt32("id_organization"),
                                    AttemptNumber = reader.GetInt32("attempt_number"),
                                    Status = reader.GetString("moodle_assessment_status")
                                };
                                moodleAssessmentStatusList.Add(status);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return moodleAssessmentStatusList;
        }



        [Route("GetUserScheduledEventDetails")]
        [HttpGet]
        public APIRESPONSE GetUserScheduledEventDetails()
        {
            APIRESPONSE apiResponse = new APIRESPONSE();

            try
            {
                int OrgID = 0, id_user = 0;
                UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

                if (userData != null)
                {
                    OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                    id_user = Convert.ToInt32(userData.ID_USER);
                }

                string SqlQuery = "select DISTINCT se.id_scheduled_event,se.event_title,event_description,program_start_date,program_end_date,event_online_url,date_format(program_start_date,'%e %M %Y') AS event_duration,DAYNAME(program_start_date) AS program_day, DATE_FORMAT(program_start_date,'%d/%m/%Y %h:%i %p') AS Start_Time, DATE_FORMAT(program_end_date,'%d/%m/%Y %h:%i %p') AS End_Time,TIMESTAMPDIFF(MINUTE, CURDATE(), program_start_date) AS program_duration,id_event_creator from tbl_scheduled_event_subscription_log es INNER JOIN tbl_scheduled_event se on es.id_scheduled_event=se.id_scheduled_event WHERE se.Status='A' AND (es.id_user='" + id_user + "' OR se.id_event_creator='" + id_user + "') AND se.program_start_date>CURDATE()";

                var ScheduledEventData = new UtilityModel().getUserScheduledEventDetails(SqlQuery);

                apiResponse.KEY = "SUCCESS";
                apiResponse.MESSAGE = JsonConvert.SerializeObject(ScheduledEventData);
            }
            catch (Exception ex)
            {
                apiResponse.KEY = "FAILURE";
                apiResponse.MESSAGE = "Code Error.";
            }

            return apiResponse;
        }

        [Route("SubmitEventDetails")]
        [HttpPost]
        public APIRESPONSE SubmitEventDetails(tbl_scheduled_event EventDetails)
        {
            APIRESPONSE apiResponse = new APIRESPONSE();
            int isSaved = 0;

            string FacilitatorName = "";
            int OrgID = 0, id_user = 0;
            UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

            if (userData != null)
            {
                FacilitatorName = userData.fullname;
                OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                id_user = Convert.ToInt32(userData.ID_USER);
            }

            EventDetails.event_group_type = 1;
            EventDetails.event_type = 2;
            EventDetails.facilitator_name = FacilitatorName;
            //////string ProgramName = EventDetails.program_name;
            //////string ProgramDescription = EventDetails.Subject;
            //////string ProgramObjective = EventDetails.Subject;
            EventDetails.facilitator_organization = "Bata";
            if (EventDetails.participants != "")
            {
                EventDetails.no_of_participants = EventDetails.participants.Split(',').Length;
            }
            EventDetails.program_venue = "Online";
            EventDetails.program_location = "Online";
            EventDetails.attachment_type = 1;
            EventDetails.id_program = Convert.ToInt32(EventDetails.id_program);
            EventDetails.id_category_tile = 0;
            EventDetails.id_category_heading = 0;
            EventDetails.id_category = 0;
            EventDetails.is_approval = "2";
            EventDetails.is_response = "2";
            EventDetails.is_unsubscribe = "2";
            ////EventDetails.program_start_date = Convert.ToDateTime(EventDetails.program_start_date);
            var ProgramStartDate = Convert.ToDateTime(EventDetails.program_start_date).ToString("yyyy-MM-dd HH:mm:ss");
            var ProgramEndDate = Convert.ToDateTime(EventDetails.program_end_date).ToString("yyyy-MM-dd HH:mm:ss");
            var RegistrationStartDate = Convert.ToDateTime(EventDetails.program_start_date).AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss");
            EventDetails.program_duration_type = 2;
            EventDetails.program_duration = Convert.ToInt32((TimeSpan.ParseExact(EventDetails.End_Time, @"hh\:mm", CultureInfo.InvariantCulture).Subtract(TimeSpan.ParseExact(EventDetails.Start_Time, @"hh\:mm", CultureInfo.InvariantCulture))).TotalMinutes);
            EventDetails.program_duration_unit = "H";
            ////DateTime program_end_date = Convert.ToDateTime(EventDetails.program_end_date);
            ////EventDetails.updated_date_time = DateTime.UtcNow;
            var UpdatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string SqlQuery = "INSERT INTO tbl_scheduled_event(id_organization,event_group_type,event_title,event_description,program_name,program_description,program_objective," +
                "facilitator_name,facilitator_organization,no_of_participants,program_venue,program_location,attachment_type,id_program," +
                "id_category_tile,id_category_heading,id_category,is_approval,is_response,is_unsubscribe,program_start_date," +
                "program_duration_type,program_duration,program_duration_unit,program_end_date,event_online_url,updated_date_time,Status," +
                "registration_start_date,registration_end_date,event_start_datetime,id_event_creator,event_type,event_duration) VALUES ('" + OrgID + "','" + EventDetails.event_group_type + "','" + EventDetails.event_title + "','" + EventDetails.event_description + "','" + EventDetails.program_name + "','" + EventDetails.program_description + "','" + EventDetails.program_objective + "','" + EventDetails.facilitator_name + "','" + EventDetails.facilitator_organization + "','" + EventDetails.no_of_participants + "','" + EventDetails.program_venue + "','" + EventDetails.program_location + "','" + EventDetails.attachment_type + "','" + EventDetails.id_program + "','" + EventDetails.id_category_tile + "','" + EventDetails.id_category_heading + "','" + EventDetails.id_category + "','" + EventDetails.is_approval + "','" + EventDetails.is_response + "','" + EventDetails.is_unsubscribe + "','" + ProgramStartDate + "','" + EventDetails.program_duration_type + "','" + EventDetails.program_duration + "','" + EventDetails.program_duration_unit + "','" + ProgramEndDate + "','" + EventDetails.event_online_url + "','" + UpdatedDate + "','A','" + RegistrationStartDate + "','" + ProgramEndDate + "','" + ProgramStartDate + "','" + id_user + "','" + EventDetails.event_type + "','" + EventDetails.program_duration + "');";

            isSaved = new UtilityModel().InsertDataToDB(SqlQuery);

            if (isSaved > 0)
            {
                if (EventDetails.no_of_participants > 0)
                {
                    var ParticipantData = EventDetails.participants.Split(',');

                    for (int i = 0; i < ParticipantData.Length; i++)
                    {
                        tbl_scheduled_event_subscription_log event_Subscription_Log = new tbl_scheduled_event_subscription_log();
                        var UserData = new UtilityModel().getUserDetails(ParticipantData[i]);

                        event_Subscription_Log.id_scheduled_event = isSaved;

                        if (UserData != null)
                        {
                            event_Subscription_Log.id_user = UserData.ID_USER;
                            event_Subscription_Log.id_organization = UserData.ID_ORGANIZATION;
                        }

                        event_Subscription_Log.event_sent_timestamp = Convert.ToDateTime(UpdatedDate);

                        AddUserToEvent(event_Subscription_Log);
                    }
                }
                apiResponse.KEY = "SUCCESS";
                apiResponse.MESSAGE = "Event created successfully.";
            }
            else
            {
                apiResponse.KEY = "FAILURE";
                apiResponse.MESSAGE = "EVent creation unsuccessful.";
            }

            return apiResponse;
        }

        [Route("AddUserToEvent")]
        [HttpPost]
        public APIRESPONSE AddUserToEvent(tbl_scheduled_event_subscription_log EventAttendanceData)
        {
            APIRESPONSE apiResponse = new APIRESPONSE();
            int isSaved = 0;

            try
            {
                var UpdatedDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string SqlQuery = "INSERT INTO tbl_scheduled_event_subscription_log(id_scheduled_event,id_user,id_organization," +
                    "event_sent_timestamp,event_user_response,subscription_status,Status,id_cms_user,updated_date_time) " +
                            "VALUES ('" + EventAttendanceData.id_scheduled_event + "','" + EventAttendanceData.id_user + "'," +
                            "'" + EventAttendanceData.id_organization + "','" + Convert.ToDateTime(EventAttendanceData.event_sent_timestamp).ToString("yyyy-MM-dd HH:mm:ss") + "',0,'A','A','" + EventAttendanceData.id_user + "','" + UpdatedDateTime + "');";

                isSaved = new UtilityModel().InsertDataToDB(SqlQuery);

                if (isSaved > 0)
                {
                    apiResponse.KEY = "SUCCESS";
                    apiResponse.MESSAGE = "User added to Event successfully.";
                }
                else
                {
                    apiResponse.KEY = "FAILURE";
                    apiResponse.MESSAGE = "User not added to Event.";
                }
            }
            catch (Exception ex)
            {
                apiResponse.KEY = "FAILURE";
                apiResponse.MESSAGE = "Code Error";
            }

            return apiResponse;
        }

        [Route("SaveEventAttendanceData")]
        [HttpGet]
        public APIRESPONSE SaveEventAttendanceData(int id_scheduled_event)
        {
            APIRESPONSE apiResponse = new APIRESPONSE();
            int isSaved = 0;
            int OrgID = 0, id_user = 0;
            string UserID = "";
            try
            {
                UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

                if (userData != null)
                {
                    UserID = userData.USERID;
                    OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                    id_user = Convert.ToInt32(userData.ID_USER);
                }

                var CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string SqlQuery = "INSERT INTO tbl_event_attendence(id_scheduled_event,id_user,userid,id_organization,attended_date," +
                            "Status,created_by,created_date) " +
                            "VALUES ('" + id_scheduled_event + "','" + id_user + "','" + UserID + "'," +
                            "'" + OrgID + "','" + CreatedDate + "','A','" + id_user + "','" + CreatedDate + "');";

                isSaved = new UtilityModel().InsertDataToDB(SqlQuery);

                if (isSaved > 0)
                {
                    apiResponse.KEY = "SUCCESS";
                    apiResponse.MESSAGE = "Attendance Event marked successfully.";
                }
                else
                {
                    apiResponse.KEY = "FAILURE";
                    apiResponse.MESSAGE = "Attendance Event not marked.";
                }
            }
            catch (Exception ex)
            {
                apiResponse.KEY = "FAILURE";
                apiResponse.MESSAGE = "Code Error";
            }

            return apiResponse;
        }

        [Route("UpdateUserEventStatus")]
        [HttpGet]
        public APIRESPONSE UpdateUserEventStatus(int id_scheduled_event)
        {
            APIRESPONSE apiResponse = new APIRESPONSE();
            int isSaved = 0;

            try
            {
                var CreatedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                string SqlQuery = "SET SQL_SAFE_UPDATES = 0;UPDATE tbl_scheduled_event set Status='D' WHERE id_scheduled_event='" + id_scheduled_event + "'";

                isSaved = new UtilityModel().UpdateDataToDB(SqlQuery);

                if (isSaved > 0)
                {
                    apiResponse.KEY = "SUCCESS";
                    apiResponse.MESSAGE = "Event deleted successfully.";
                }
                else
                {
                    apiResponse.KEY = "FAILURE";
                    apiResponse.MESSAGE = "Event not deleted.";
                }
            }
            catch (Exception ex)
            {
                apiResponse.KEY = "FAILURE";
                apiResponse.MESSAGE = "Code Error";
            }

            return apiResponse;
        }

        [Route("GetCourseDetails")]
        [HttpGet]
        public APIRESPONSE GetCourseDetails()
        {
            APIRESPONSE apiResponse = new APIRESPONSE();
            try
            {
                int OrgID = 0, id_user = 0;
                UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

                if (userData != null)
                {
                    OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                    id_user = Convert.ToInt32(userData.ID_USER);
                }

                //List<tbl_category> CourseDetails = new UtilityModel().getprg("select * from tbl_category where id_organization='" + (object)OrgID + "' AND STATUS='A'");
                List<tbl_category> CourseDetails = new UtilityModel().getprg("select * from tbl_category where id_organization='" + (object)OrgID + "' AND status='A' AND category_type = 0");

                apiResponse.KEY = "SUCCESS";
                apiResponse.MESSAGE = JsonConvert.SerializeObject(CourseDetails);
            }
            catch (Exception ex)
            {
                apiResponse.KEY = "FAILURE";
                apiResponse.MESSAGE = "Code Error";
            }

            return apiResponse;
        }

        [Route("GetStoreLocationDetails")]
        [HttpGet]
        public APIRESPONSE GetStoreLocationDetails()
        {
            APIRESPONSE apiResponse = new APIRESPONSE();
            try
            {
                int OrgID = 0, id_user = 0;
                UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

                if (userData != null)
                {
                    OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                    id_user = Convert.ToInt32(userData.ID_USER);
                }

                //////var StoreLocationDetails = new UtilityModel().GetStoredLocation("SELECT DISTINCT LOCATION FROM tbl_profile tp INNER JOIN tbl_user tu on tu.ID_USER=tp.ID_USER WHERE tu.ID_ORGANIZATION='" + OrgID + "' AND tu.STATUS='A' AND tp.LOCATION IS NOT NULL AND tp.LOCATION<>'' ORDER BY LOCATION");
                var StoreLocationDetails = new UtilityModel().GetStoredLocation("SELECT DISTINCT user_grade AS LOCATION FROM tbl_user WHERE ID_ORGANIZATION='" + OrgID + "' AND STATUS='A' AND user_grade IS NOT NULL AND user_grade<>'' ORDER BY CAST(user_grade AS UNSIGNED)");

                apiResponse.KEY = "SUCCESS";
                apiResponse.MESSAGE = JsonConvert.SerializeObject(StoreLocationDetails);
            }
            catch (Exception ex)
            {
                apiResponse.KEY = "FAILURE";
                apiResponse.MESSAGE = "Code Error";
            }

            return apiResponse;
        }

        [Route("GetStoredLocationUserDetails")]
        [HttpPost]
        public APIRESPONSE GetStoredLocationUserDetails(string Location)
        {
            APIRESPONSE apiResponse = new APIRESPONSE();
            try
            {
                int OrgID = 0, id_user = 0;
                UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

                if (userData != null)
                {
                    OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                    id_user = Convert.ToInt32(userData.ID_USER);
                }

                ////var StoreLocationUserDetails = new UtilityModel().getStoredLocationUserDetails(" SELECT tu.ID_USER,tu.USERID from tbl_user tu INNER JOIN tbl_profile tp on tu.ID_USER=tp.ID_USER WHERE tp.LOCATION='" + Location + "' AND tu.STATUS='A'");
                var StoreLocationUserDetails = new UtilityModel().getStoredLocationUserDetails(" SELECT tu.ID_USER,tu.USERID from tbl_user tu INNER JOIN tbl_profile tp on tu.ID_USER=tp.ID_USER WHERE tu.user_grade='" + Location + "' AND  ID_ORGANIZATION='" + OrgID + "' AND tu.STATUS='A'");

                apiResponse.KEY = "SUCCESS";
                apiResponse.MESSAGE = JsonConvert.SerializeObject(StoreLocationUserDetails);
            }
            catch (Exception ex)
            {
                apiResponse.KEY = "FAILURE";
                apiResponse.MESSAGE = "Code Error";
            }

            return apiResponse;
        }

        [Route("UserSearchAPI")]
        [HttpPost]
        public APIRESPONSE UserSearchAPI(string SearchString)
        {
            APIRESPONSE apiResponse = new APIRESPONSE();
            try
            {
                string SqlQuery = "select * from tbl_user tu INNER JOIN tbl_profile tp on tu.ID_USER=tp.ID_USER where Email like '%" + SearchString + "%' OR FIRSTNAME like '%" + SearchString + "%' OR LASTNAME like '%" + SearchString + "%';";

                var UserData = new UtilityModel().getSearchedAPIData(SqlQuery);

                apiResponse.KEY = "SUCCESS";
                apiResponse.MESSAGE = JsonConvert.SerializeObject(UserData);
            }
            catch (Exception ex)
            {
                apiResponse.KEY = "FAILURE";
                apiResponse.MESSAGE = "Code Error";
            }

            return apiResponse;
        }

        [Route("GetUserMaxAttemptDetails")]
        [HttpGet]
        public tbl_rs_type_qna_Attempt GetUserMaxAttemptDetails(int AssessmentID)
        {
            try
            {
                int OrgID = 0, id_user = 0;
                UserSession userData = (UserSession)HttpContext.Current.Session.Contents["UserSession"];

                if (userData != null)
                {
                    OrgID = Convert.ToInt32(userData.id_ORGANIZATION);
                    id_user = Convert.ToInt32(userData.ID_USER);
                }

                string SqlQuery = "SELECT attempt_number as attempt_no,updated_date_time as recorded_timestamp FROM tbl_rs_type_qna WHERE id_assessment='" + AssessmentID + "' AND id_user='" + id_user + "' order by updated_date_time desc limit 1";

                var AttemptData = new UtilityModel().GetUserMaxAttemptDetails(SqlQuery);

                if (AttemptData.Attemp_No == 0)
                {
                    AttemptData = null;
                }

                return AttemptData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //////// GET: api/KPI
        //////public IEnumerable<string> Get()
        //////{
        //////    return new string[] { "value1", "value2" };
        //////}

        //////// GET: api/KPI/5
        //////public string Get(int id)
        //////{
        //////    return "value";
        //////}

        //////// POST: api/KPI
        //////public void Post([FromBody]string value)
        //////{
        //////}

        //////// PUT: api/KPI/5
        //////public void Put(int id, [FromBody]string value)
        //////{
        //////}

        //////// DELETE: api/KPI/5
        //////public void Delete(int id)
        //////{
        //////}
    }
}
