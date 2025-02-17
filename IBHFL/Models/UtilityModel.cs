// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.UtilityModel
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities.Net;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using IPAddress = System.Net.IPAddress;

namespace IBHFL.Models
{
    public class UtilityModel
    {
        private MySqlConnection conn;

        public UtilityModel() => this.conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString);

        public string getApiResponseString(string api)
        {
            byte[] bytes = (byte[])null;
            WebClient webClient1 = new WebClient();
            using (WebClient webClient2 = new WebClient())
                bytes = webClient2.DownloadData(api);
            return Encoding.UTF8.GetString(bytes);
        }

        public string getApiPost(string uri, NameValueCollection pairs)
        {
            byte[] bytes = (byte[])null;
            using (WebClient webClient = new WebClient())
                bytes = webClient.UploadValues(uri, pairs);
            return Encoding.UTF8.GetString(bytes);
        }

        public async Task<string> GetApiPostAsync(string uri, NameValueCollection pairs)
        {
            byte[] bytes;

            using (HttpClient httpClient = new HttpClient())
            {
                var content = new FormUrlEncodedContent(pairs.AllKeys.Select(key => new KeyValuePair<string, string>(key, pairs[key])));
                var response = await httpClient.PostAsync(uri, content);

                if (!response.IsSuccessStatusCode)
                {
                    // Handle non-success status code if needed
                    throw new HttpRequestException($"Error: {response.StatusCode}");
                }

                bytes = await response.Content.ReadAsByteArrayAsync();
            }

            return Encoding.UTF8.GetString(bytes);
        }

        public List<GameElements> getMyGames(int orgid, int userid)
        {
            List<GameElements> myGames = new List<GameElements>();
            try
            {
                string str = "SELECT  * FROM tbl_game_creation WHERE id_game IN (SELECT DISTINCT id_game FROM  tbl_game_group_association    WHERE id_user =" + (object)userid + " AND status = 'A'    AND association_type = '1') and status='A' OR id_game IN (SELECT DISTINCT   id_game FROM  tbl_game_group_association  WHERE  id_game_group IN (SELECT DISTINCT   id_game_group  FROM   tbl_game_group_participatant  WHERE  id_user =" + (object)userid + ")) AND status = 'A' AND id_organisation =" + (object)orgid;
                List<GameElements> gameElementsList = new List<GameElements>();
                this.conn.Open();
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                while (mySqlDataReader.Read())
                    myGames.Add(new GameElements()
                    {
                        game_title = mySqlDataReader["game_title"].ToString(),
                        game_description = mySqlDataReader["game_description"].ToString(),
                        gameid = mySqlDataReader["gameid"].ToString(),
                        id_game = Convert.ToInt32(mySqlDataReader["id_game"].ToString())
                    });
                this.conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return myGames;
        }

        public string getUserName(int usid)
        {
            string str = "SELECT USERID FROM tbl_user where ID_USER=" + (object)usid;
            string userName = "";
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            while (mySqlDataReader.Read())
                userName = mySqlDataReader["USERID"].ToString();
            this.conn.Close();
            return userName;
        }
        public tbl_user getAssessment(string querry)
        {
            this.conn.Open();
            tbl_user UserDetails = new tbl_user();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = querry;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            while (mySqlDataReader.Read()) {
                UserDetails.ASSESSMENT_TYPE = Convert.ToInt32(mySqlDataReader["assessment_type"]);
                UserDetails.ID_ORGANIZATION = Convert.ToInt32(mySqlDataReader["ID_ORGANIZATION"]);
            }
            this.conn.Close();
            return UserDetails;
        }
        public string getRoleName(int RoleId,string KPINAME)
        {
            string str = "SELECT Kpi_coroebus_name FROM Tbl_coroebus_role_mapping where Id_m2ost_role= "+RoleId+" and kpi_m2ost_name = '"+ KPINAME +"'";
            string RoleName = "";
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            while (mySqlDataReader.Read())
                RoleName = mySqlDataReader["Kpi_coroebus_name"].ToString();
            this.conn.Close();
            return RoleName;
        }
        public string getRoleName_Lb(int RoleId,int OrgId)
        {
            string str = "SELECT substr(csst_role,1,2) as RoleName_LB FROM Tbl_csst_role where Id_csst_role= " + RoleId + " and id_organization = "+OrgId+" and status = 'A'";
            string RoleName_LB = "";
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            while (mySqlDataReader.Read())
                RoleName_LB = mySqlDataReader["RoleName_LB"].ToString();
            this.conn.Close();
            return RoleName_LB;
        }
        //public tbl_user getRMNames(string UserId, int? OrgId)
        //{
        //    string str = "SELECT  a.userid,a.user_designation,CONCAT(g.firstname,'-', g.lastname) AS myName, a.user_grade AS Store_code, c.csst_role," +
        //        "  a.reporting_manager,b.userid AS rm1UserID,b.user_designation AS rm1UserDesignation,CONCAT(h.firstname, '-', h.lastname) AS rm1Name," +
        //        " d.userid AS rm2UserID,d.user_designation AS rm2UserDesignation,CONCAT(i.firstname, '-', i.lastname) AS rm2Name,e.userid AS RM3UserID," +
        //        "e.user_designation AS RM3UserDesignation, CONCAT(j.firstname, '-', j.lastname) AS RM3Name,f.userid AS rm4UserID,f.user_designation AS rm4UserDesignation,CONCAT(k.firstname, '-', k.lastname) AS rm4Name FROM " +
        //        " tbl_user a LEFT JOIN tbl_user b ON a.reporting_manager = b.id_user LEFT JOIN tbl_csst_role c ON a.id_role = c.id_csst_role LEFT JOIN tbl_profile g ON a.id_user = g.id_user LEFT JOIN tbl_user d ON b.reporting_manager = d.id_user " +
        //        "LEFT JOIN tbl_profile h ON b.id_user = h.id_user LEFT JOIN tbl_user e ON d.reporting_manager = e.id_user LEFT JOIN tbl_profile i ON d.id_user = i.id_user LEFT JOIN tbl_user f ON e.reporting_manager = f.id_user " +
        //        "LEFT JOIN tbl_profile j ON e.id_user = j.id_user LEFT JOIN tbl_profile k ON f.id_user = k.id_user WHERE a.id_organization = "+ OrgId + " AND a.status = 'A' AND a.userid = '"+ UserId + "'";
        //    tbl_user RoleName_LB = new tbl_user();
        //    this.conn.Open();
        //    MySqlCommand command = this.conn.CreateCommand();
        //    command.CommandText = str;
        //    MySqlDataReader mySqlDataReader = command.ExecuteReader();
        //    while (mySqlDataReader.Read())
        //    RoleName_LB.SM = mySqlDataReader["rm1Name"].ToString();
        //    RoleName_LB.DM = mySqlDataReader["rm2Name"].ToString();
        //    RoleName_LB.RM = mySqlDataReader["rm3Name"].ToString();
        //    RoleName_LB.Role = mySqlDataReader["csst_role"].ToString();
        //    this.conn.Close();
        //    return RoleName_LB;
        //}

        // sujit change on 02/02/2024 optimize login

        
        public tbl_user getRMNames(string UserId, int? OrgId)
        {
            tbl_user roleNameLB = null;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    conn.Open();

                    string query = @"SELECT a.userid, a.user_designation, CONCAT(g.firstname, '-', g.lastname) AS myName, a.user_grade AS Store_code, 
                             c.csst_role, a.reporting_manager, b.userid AS rm1UserID, b.user_designation AS rm1UserDesignation, 
                             CONCAT(h.firstname, '-', h.lastname) AS rm1Name, d.userid AS rm2UserID, d.user_designation AS rm2UserDesignation, 
                             CONCAT(i.firstname, '-', i.lastname) AS rm2Name, e.userid AS RM3UserID, e.user_designation AS RM3UserDesignation, 
                             CONCAT(j.firstname, '-', j.lastname) AS RM3Name, f.userid AS rm4UserID, f.user_designation AS rm4UserDesignation, 
                             CONCAT(k.firstname, '-', k.lastname) AS rm4Name 
                             FROM tbl_user a 
                             LEFT JOIN tbl_user b ON a.reporting_manager = b.id_user 
                             LEFT JOIN tbl_csst_role c ON a.id_role = c.id_csst_role 
                             LEFT JOIN tbl_profile g ON a.id_user = g.id_user 
                             LEFT JOIN tbl_user d ON b.reporting_manager = d.id_user 
                             LEFT JOIN tbl_profile h ON b.id_user = h.id_user 
                             LEFT JOIN tbl_user e ON d.reporting_manager = e.id_user 
                             LEFT JOIN tbl_profile i ON d.id_user = i.id_user 
                             LEFT JOIN tbl_user f ON e.reporting_manager = f.id_user 
                             LEFT JOIN tbl_profile j ON e.id_user = j.id_user 
                             LEFT JOIN tbl_profile k ON f.id_user = k.id_user 
                             WHERE a.id_organization = @OrgId AND a.status = 'A' AND a.userid = @UserId";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        // Safely parameterize the query to prevent SQL injection
                        command.Parameters.AddWithValue("@OrgId", OrgId);
                        command.Parameters.AddWithValue("@UserId", UserId);

                        using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
                        {
                            if (mySqlDataReader.Read())
                            {
                                // Populate the tbl_user object with data from the reader
                                roleNameLB = new tbl_user
                                {
                                    SM = mySqlDataReader["rm1Name"].ToString(),
                                    DM = mySqlDataReader["rm2Name"].ToString(),
                                    RM = mySqlDataReader["rm3Name"].ToString(),
                                    Role = mySqlDataReader["csst_role"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Log MySQL related exceptions
                Log.Error($"MySQL error in getRMNames: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general exceptions
                Log.Error($"An error occurred in getRMNames: {ex.Message}", ex);
            }

            return roleNameLB;
        }
        public int getRoleID(int userid)
        {
            string str = "SELECT ID_ROLE FROM tbl_user where ID_USER=" + (object)userid;
            int roleId = 0;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            while (mySqlDataReader.Read())
                roleId =Convert.ToInt32( mySqlDataReader["ID_ROLE"]);
            this.conn.Close();
            return roleId;
        }
        public string getCatName(string query)
        {
            string categoryName = string.Empty;

            try
            {
                this.conn.Open();
                using (MySqlCommand command = this.conn.CreateCommand())
                {
                    command.CommandText = query;
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read()) // Read only the first row
                        {
                            categoryName = reader["categoryname"]?.ToString(); // Use null-conditional operator
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Ensure the connection is closed even if an exception occurs
                if (this.conn.State == System.Data.ConnectionState.Open)
                {
                    this.conn.Close();
                }
            }

            return categoryName;
        }

        public int getGameId(int Id)
        {
            string str = "SELECT Id_coroebus_game FROM Tbl_User_Game_mapping where Id_m2ost_user=" + (object)Id;
            int GameId = 0;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            while (mySqlDataReader.Read())
                GameId = Convert.ToInt32(mySqlDataReader["Id_coroebus_game"]);
            this.conn.Close();
            return GameId;
        }
        //public tbl_user getUserDetails(string usid)
        //{
        //     string str = "SELECT * FROM tbl_user us left join tbl_profile pf on us.id_user = pf.id_user   where us.USERID='" + (object)usid + "' AND us.STATUS='A' ";            
        //    this.conn.Open();
        //    MySqlCommand command = this.conn.CreateCommand();
        //    command.CommandText = str;
        //    MySqlDataReader mySqlDataReader = command.ExecuteReader();
        //    tbl_user UserDetails = new tbl_user();
        //    while (mySqlDataReader.Read())
        //    {
        //        UserDetails.ID_USER = Convert.ToInt32(mySqlDataReader["ID_USER"].ToString());
        //        UserDetails.USERID = mySqlDataReader["USERID"].ToString();
        //        UserDetails.PASSWORD = mySqlDataReader["PASSWORD"].ToString();
        //        UserDetails.ID_ORGANIZATION = Convert.ToInt32(mySqlDataReader["ID_ORGANIZATION"]);
        //        UserDetails.EMPLOYEEID = mySqlDataReader["EMPLOYEEID"].ToString();
        //        UserDetails.user_designation = mySqlDataReader["USER_DESIGNATION"].ToString();
        //        UserDetails.ID_ROLE =Convert.ToInt32(mySqlDataReader["Id_Role"]);
        //        UserDetails.user_grade = mySqlDataReader["USER_GRADE"].ToString();
        //        UserDetails.user_function = mySqlDataReader["USER_FUNCTION"].ToString();
        //        UserDetails.OFFICE_ADDRESS = mySqlDataReader["OFFICE_ADDRESS"].ToString();
        //        UserDetails.GENDER = mySqlDataReader["GENDER"].ToString();
        //        UserDetails.FullName = string.Concat(mySqlDataReader["firstname"].ToString(), " ", mySqlDataReader["Lastname"].ToString());
        //    }
        //    this.conn.Close();
        //    return UserDetails;
        //}

        // sujit change on 02/02/2024 optimize login
        public tbl_user getUserDetails(string usid)
        {

            tbl_user UserDetails = new tbl_user();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    conn.Open();

                    string query = "SELECT * FROM tbl_user us LEFT JOIN tbl_profile pf ON us.id_user = pf.id_user WHERE us.USERID=@UserId AND us.STATUS='A'";
                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        command.Parameters.AddWithValue("@UserId", usid);

                        using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
                        {
                            if (mySqlDataReader.Read())
                            {
                                UserDetails.ID_USER = mySqlDataReader.GetInt32("ID_USER");
                                UserDetails.USERID = mySqlDataReader.GetString("USERID");
                                UserDetails.PASSWORD = mySqlDataReader.GetString("PASSWORD");
                                UserDetails.ID_ORGANIZATION = mySqlDataReader.GetInt32("ID_ORGANIZATION");
                                UserDetails.EMPLOYEEID = mySqlDataReader.GetString("EMPLOYEEID");
                                UserDetails.user_designation = mySqlDataReader.GetString("USER_DESIGNATION");
                                UserDetails.ID_ROLE = mySqlDataReader.GetInt32("Id_Role");
                                UserDetails.user_grade = mySqlDataReader.GetString("USER_GRADE");
                                UserDetails.user_function = mySqlDataReader.GetString("USER_FUNCTION");
                                UserDetails.OFFICE_ADDRESS = mySqlDataReader.GetString("OFFICE_ADDRESS");
                                UserDetails.GENDER = mySqlDataReader.GetString("GENDER");
                                UserDetails.FullName = string.Concat(mySqlDataReader["firstname"].ToString(), " ", mySqlDataReader["Lastname"].ToString());
                            }
                        }
                    }
                }
            }
            catch (MySqlException sqlEx)
            {
                // Log database-related errors
                Log.Error($"MySQL error in getUserDetails: {sqlEx.Message}", sqlEx);
            }
            catch (Exception ex)
            {
                // Log general errors
                Log.Error($"An error occurred in getUserDetails: {ex.Message}", ex);
            }

            return UserDetails;
         }
        public List<tbl_user> getUserDesignationDetails(string RoleName_LB,int OrgId)
        {
            string str = "SELECT a.userid,a.ID_USER,d.firstname,d.Lastname,SUM(points_scored) AS points " +
                "FROM tbl_user a,tbl_csst_role b,tbl_user_kpi_data_log c,tbl_profile d " +
                "WHERE a.id_role = b.id_csst_role AND a.id_user = c.id_user AND a.id_user = d.id_user AND b.id_organization = "+OrgId+" AND c.isactive = 'A'" +
                " AND a.STATUS = 'A' AND b.STATUS = 'A' AND BINARY SUBSTR(b.csst_role, 1, 2) = '"+ RoleName_LB + "' GROUP BY a.userid HAVING points > 0 ORDER BY points DESC;";           
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_user> UserDetails = new List <tbl_user>();
            int rank = 0;
            double previousPoints = double.MaxValue;
            while (mySqlDataReader.Read())
            {
                if (Convert.ToDouble(mySqlDataReader["points"]) < previousPoints)
                {
                    rank++;
                }
                previousPoints = Convert.ToDouble(mySqlDataReader["points"]);
                UserDetails.Add(new tbl_user()
                {
                    ID_USER = Convert.ToInt32(mySqlDataReader["ID_USER"].ToString()),
                    USERID = mySqlDataReader["USERID"].ToString(),
                    FullName = string.Concat(mySqlDataReader["firstname"].ToString()," ", mySqlDataReader["Lastname"].ToString()),
                    PointsScored =Convert.ToDouble( mySqlDataReader["points"]),
                    Dense_Rank =  rank,                    
                    
                });

            }
            this.conn.Close();
            return UserDetails;
        }
        public List<tbl_user_kpi_data_log> getCoinsCount(string querry)
        {
           List<tbl_user_kpi_data_log> DataScore = new List<tbl_user_kpi_data_log>();
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = querry;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            while (mySqlDataReader.Read())
                DataScore.Add(new tbl_user_kpi_data_log()
                {
                    id_user = Convert.ToInt32(mySqlDataReader["id_user"]),
                    AttemptNo = Convert.ToInt32(mySqlDataReader["attempt_number"]),
                    points_scored = Convert.ToInt32(mySqlDataReader["result_in_percentage"])
                });
            this.conn.Close();
            return DataScore;
        }

        public List<int> getTotalCoin(string SqlQuery)
        {
            List<int> resultList = new List<int>();

            try
            {
                this.conn.Open();
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = SqlQuery;
                using (MySqlDataReader mySqlDataReader = command.ExecuteReader())
                {
                    if (mySqlDataReader.Read())
                    {
                        int avgCoinsScored = mySqlDataReader.GetInt32(0); // Assuming the result is an integer
                        Console.WriteLine("Average coins scored: " + avgCoinsScored);
                        resultList.Add(avgCoinsScored);
                    }
                    else
                    {
                        Console.WriteLine("No results found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                this.conn.Close();
            }

            return resultList;
        }


        public tbl_profile getUserProfileDetails(string usid)
        {
            string str = "SELECT * FROM tbl_profile where ID_USER='" + (object)usid + "'";
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            tbl_profile UserDetails = new tbl_profile();
            while (mySqlDataReader.Read())
            {
                UserDetails.FIRSTNAME = mySqlDataReader["FIRSTNAME"].ToString();
                UserDetails.LASTNAME = mySqlDataReader["LASTNAME"].ToString();
                UserDetails.FullName = string.Concat(UserDetails.FIRSTNAME," ",UserDetails.LASTNAME);
            }
            this.conn.Close();
            return UserDetails;
        }

        public tbl_organization getOrganizationDetails(int? OrgID)
        {
            string str = "SELECT * FROM tbl_organization where ID_ORGANIZATION='" + (object)OrgID + "'";
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            tbl_organization Details = new tbl_organization();
            while (mySqlDataReader.Read())
            {
                Details.ID_ORGANIZATION = Convert.ToInt32(mySqlDataReader["ID_ORGANIZATION"]);
                Details.DEFAULT_EMAIL = mySqlDataReader["DEFAULT_EMAIL"].ToString();
                Details.CONTACTNUMBER = mySqlDataReader["CONTACTNUMBER"].ToString();
            }
            this.conn.Close();
            return Details;
        }

        public string getOrgLogo(int oid)
        {
            try
            {
                string str1 = "";
                string str2 = "SELECT LOGO FROM tbl_organization WHERE id_organization = @value1";
                this.conn.Open();
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str2;
                command.Parameters.AddWithValue("value1", (object)oid);
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                while (mySqlDataReader.Read())
                    str1 = mySqlDataReader["LOGO"].ToString();
                mySqlDataReader.Close();
                return !(str1 == "") ? ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "ORGLOGO/" + str1 : ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "default.png";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.conn.Close();
            }
        }

        public string getOrgBanner(int oid)
        {
            try
            {
                string orgBanner = "";
                string str = "SELECT Banner_path FROM tbl_organisation_banner WHERE id_organisation = @value1";
                this.conn.Open();
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object)oid);
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                while (mySqlDataReader.Read())
                    orgBanner = mySqlDataReader["Banner_path"].ToString();
                mySqlDataReader.Close();
                if (!(orgBanner == ""))
                    orgBanner = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "BANNERIMG/" + orgBanner;
                return orgBanner;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.conn.Close();
            }
        }

        public int getOrgBannerID(int oid)
        {
            try
            {
                int orgBannerId = 0;
                string str = "SELECT id_organisation_banner FROM tbl_organisation_banner WHERE id_organization = @value1";
                this.conn.Open();
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str;
                command.Parameters.AddWithValue("value1", (object)oid);
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                while (mySqlDataReader.Read())
                    orgBannerId = Convert.ToInt32(mySqlDataReader["id_organisation_banner"].ToString());
                mySqlDataReader.Close();
                return orgBannerId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.conn.Close();
            }
        }

        public void addTask(int org_id, int user_id, string task)
        {
            MySqlCommand command = this.conn.CreateCommand();
            string str = "INSERT INTO tbl_task(id_user, id_org, task) VALUES (@value1,@value2,@value3)";
            command.CommandText = str;
            command.Parameters.AddWithValue("value1", (object)user_id);
            command.Parameters.AddWithValue("value2", (object)org_id);
            command.Parameters.AddWithValue("value3", (object)task);
            this.conn.Open();
            command.ExecuteNonQuery();
            this.conn.Close();
        }

        public List<Task> getTask(int org_id, int user_id)
        {
            string str = "select * from tbl_task where id_user=@value0 and status=@value1 and DATE(modified_date) <= CURDATE()";
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            command.Parameters.AddWithValue("value0", (object)user_id);
            command.Parameters.AddWithValue("value1", (object)'A');
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<Task> task = new List<Task>();
            while (mySqlDataReader.Read())
                task.Add(new Task()
                {
                    id_user = Convert.ToInt32(mySqlDataReader["id_user"]),
                    id_org = Convert.ToInt32(mySqlDataReader["id_org"]),
                    id_task = Convert.ToInt32(mySqlDataReader["id_task"]),
                    task = mySqlDataReader["task"].ToString(),
                    modified_date = new DateTime?(Convert.ToDateTime(mySqlDataReader["modified_date"].ToString()))
                });
            this.conn.Close();
            return task;
        }

        /// <summary> For the search
        public List<object> SearchbarList(int org_id, string id_category, string content_answer1)
        {
            var Searchresult = new List<object>(); // To store the results

            // Split `id_category` into an array and prepare a parameterized query
            var categoryIds = id_category.Split(',').Select((id, index) => $"@CategoryId{index}").ToArray();
            string categoryCondition = string.Join(",", categoryIds);

            string query = $@"
SELECT 
    a.*, 
    b.*, 
    c.*
FROM 
    tbl_content a
LEFT JOIN 
    tbl_content_organization_mapping b 
    ON a.id_content = b.id_content
LEFT JOIN 
    tbl_content_answer c 
    ON a.id_content = c.ID_CONTENT
WHERE 
    a.STATUS = 'A' 
    AND b.id_category IN ({categoryCondition}) 
    AND b.id_organization = @OrgId
    AND c.CONTENT_ANSWER1 LIKE @ContentAnswer";

            //comment for LEFT JOIN 
            //tbl_content_metadata d
             //ON c.ID_CONTENT_ANSWER = d.ID_CONTENT_ANSWER

            // Ensure your connection string is correctly configured
            string connectionString = ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add `OrgId` and `ContentAnswer` parameters
                    command.Parameters.AddWithValue("@OrgId", org_id);
                    command.Parameters.AddWithValue("@ContentAnswer", "%" + content_answer1 + "%");

                    // Add parameters for each category ID
                    var categories = id_category.Split(',');
                    for (int i = 0; i < categories.Length; i++)
                    {
                        command.Parameters.AddWithValue($"@CategoryId{i}", categories[i]);
                    }

                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Map results to a dynamic object or your desired structure
                            var result = new
                            {
                                ID_CONTENT = reader["ID_CONTENT"],
                                ID_THEME = reader["ID_THEME"],
                                ID_USER = reader["ID_USER"],
                                CONTENT_HEADER = reader["CONTENT_HEADER"],
                                CONTENT_QUESTION = reader["CONTENT_QUESTION"],
                                id_category = reader["id_category"],
                                id_content = reader["id_content"],
                                // Add other required fields here
                            };
                            Searchresult.Add(result);
                        }
                    }
                }
            }

            return Searchresult;
        }

        /// <param></param>

        public void deleteTask(int id)
        {
            MySqlCommand command = this.conn.CreateCommand();
            string str = "UPDATE tbl_task SET status='D' WHERE id_task=@value1";
            command.CommandText = str;
            command.Parameters.AddWithValue("value1", (object)id);
            this.conn.Open();
            command.ExecuteNonQuery();
            this.conn.Close();
        }

        public int getPerformance(int user_id, int org_id)
        {
            string str = "CALL `percentilescore`(@value0,@value1);";
            int performance = 0;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            command.Parameters.AddWithValue("value0", (object)user_id);
            command.Parameters.AddWithValue("value1", (object)org_id);
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            while (mySqlDataReader.Read())
                performance = Convert.ToInt32(mySqlDataReader["Percentile"]);
            this.conn.Close();
            return performance;
        }

        public int OverallScore(int user_id, int org_id)
        {
            string str = "CALL `percentilescore`(@value0,@value1);";
            int num = 0;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            command.Parameters.AddWithValue("value0", (object)user_id);
            command.Parameters.AddWithValue("value1", (object)org_id);
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            while (mySqlDataReader.Read())
                num = Convert.ToInt32(mySqlDataReader[nameof(OverallScore)]);
            this.conn.Close();
            return num;
        }

        public Ticker getTicker(int org_id)
        {
            string str = "select * from tbl_news_ticker where Id_org=@value0 and status=@value1";
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            command.Parameters.AddWithValue("value0", (object)org_id);
            command.Parameters.AddWithValue("value1", (object)'A');
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            Ticker ticker = new Ticker();
            while (mySqlDataReader.Read())
            {
                ticker.ticker_news = mySqlDataReader["ticker_news"].ToString();
                ticker.background_color = mySqlDataReader["background_color"].ToString();
                ticker.font_color = mySqlDataReader["font_color"].ToString();
            }
            this.conn.Close();
            return ticker;
        }

        public List<PROGRAMCOMPLETE> getProgramData(string STR)
        {
            List<PROGRAMCOMPLETE> programData = new List<PROGRAMCOMPLETE>();
            try
            {
                string str = STR;
                this.conn.Open();
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                while (mySqlDataReader.Read())
                {
                    PROGRAMCOMPLETE programcomplete = new PROGRAMCOMPLETE()
                    {
                        USERID = Convert.ToString(mySqlDataReader["USERID"]),
                        ID_USER = Convert.ToString(mySqlDataReader["id_user"]),
                        EMPLOYEEID = Convert.ToString(mySqlDataReader["EMPLOYEEID"]),
                        UNAME = Convert.ToString(mySqlDataReader["UNAME"]),
                        CATEGORYNAME = Convert.ToString(mySqlDataReader["CATEGORYNAME"]),
                        ID_CATEGORY = Convert.ToInt32(mySqlDataReader["ID_CATEGORY"]),
                        start_date = Convert.ToDateTime(mySqlDataReader["start_date"]),
                        end_date = Convert.ToDateTime(mySqlDataReader["expiry_date"]),
                        assigned_date = Convert.ToDateTime(mySqlDataReader["assigned_date"]),
                        LOCATION = Convert.ToString(mySqlDataReader["LOCATION"]),
                        DESIGNATION = Convert.ToString(mySqlDataReader["user_designation"])
                    };
                    programcomplete.LOCATION = Convert.ToString(mySqlDataReader["LOCATION"]);
                    programcomplete.DESIGNATION = Convert.ToString(mySqlDataReader["user_designation"]);
                    programcomplete.RMUSER = Convert.ToString(mySqlDataReader["RMUSER"]);
                    programcomplete.USTATUS = Convert.ToString(mySqlDataReader["USTATUS"]);
                    programData.Add(programcomplete);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.conn.Close();
            }
            return programData;
        }

        public int getRecordCount(string str)
        {
            int recordCount = 0;
            try
            {
                this.conn.Open();
                MySqlCommand command = this.conn.CreateCommand();
                command.CommandText = str;
                MySqlDataReader mySqlDataReader = command.ExecuteReader();
                if (mySqlDataReader.HasRows)
                {
                    while (mySqlDataReader.Read())
                        recordCount = Convert.ToInt32(mySqlDataReader["count"].ToString());
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            finally
            {
                this.conn.Close();
                this.conn = (MySqlConnection)null;
            }
            return recordCount;
        }

        public List<string> GetThoughtsNames(string query)
        {
            List<string> result = new List<string>();

            try
            {
                this.conn.Open();
                using (MySqlCommand command = this.conn.CreateCommand())
                {
                    command.CommandText = query;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Retrieve the value of the 'thoughts_name' column
                            if (!reader.IsDBNull(0)) // Ensure the column value is not null
                            {
                                result.Add(reader.GetString(0)); // Index 0 because only 'thoughts_name' is selected
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Optionally, log the error or throw it further
            }
            finally
            {
                if (this.conn != null && this.conn.State == ConnectionState.Open)
                {
                    this.conn.Close();
                }
            }

            return result;
        }

        public List<string> GetgreetingsNames(string query)
        {
            List<string> result = new List<string>();

            try
            {
                this.conn.Open();
                using (MySqlCommand command = this.conn.CreateCommand())
                {
                    command.CommandText = query;

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Retrieve the value of the 'thoughts_name' column
                            if (!reader.IsDBNull(0)) // Ensure the column value is not null
                            {
                                result.Add(reader.GetString(0)); // Index 0 because only 'thoughts_name' is selected
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // Optionally, log the error or throw it further
            }
            finally
            {
                if (this.conn != null && this.conn.State == ConnectionState.Open)
                {
                    this.conn.Close();
                }
            }

            return result;
        }


        public List<LeaderBoard> getLeaderBoard(int org_id)
        {
            string str = "CALL `leaderboard`(@value0)";
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            command.Parameters.AddWithValue("value0", (object)org_id);
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<LeaderBoard> leaderBoard = new List<LeaderBoard>();
            while (mySqlDataReader.Read())
                leaderBoard.Add(new LeaderBoard()
                {
                    id_user = Convert.ToInt32(mySqlDataReader["id_user"]),
                    username = mySqlDataReader["username"].ToString(),
                    OverallScore = Convert.ToDouble(mySqlDataReader["OverallScore"]),
                    Percentile = Convert.ToInt32(mySqlDataReader["Percentile"]),
                    Ordinal = mySqlDataReader["Ordinal"].ToString()
                });
            this.conn.Close();
            return leaderBoard;
        }

        public string check_password(string password, int uid)
        {
            string str1 = "SELECT * FROM tbl_user where ID_USER=" + (object)uid;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str1;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            string str2 = "";
            while (mySqlDataReader.Read())
                str2 = mySqlDataReader["PASSWORD"].ToString();
            this.conn.Close();
            return !(str2 == password) ? "F" : "S";
        }

        public double GetVideoTimer(string SqlQuery)
        {
            string str1 = SqlQuery;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str1;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            string str2 = "";
            while (mySqlDataReader.Read())
                str2 = mySqlDataReader["video_timer"].ToString();
            this.conn.Close();
            return str2 != "" ? Convert.ToDouble(str2) : 0;
        }

        public int IsVideoCompleted(string SqlQuery)
        {
            string str1 = SqlQuery;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str1;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            string str2 = "";
            while (mySqlDataReader.Read())
                str2 = mySqlDataReader["is_completed"].ToString();
            this.conn.Close();
            return str2 != "" ? Convert.ToInt32(str2) : 0;
        }

        public void InsertVideoTimer(int ContentID, int CategoryID, int UserID, double VideoTimer, int IsCompleted, int OrgID, double CompletePer, double TotalVideoTimer)
        {
            MySqlCommand command = this.conn.CreateCommand();
            string str = "INSERT INTO tbl_prod_que_ans_video(ContentID,CategoryID,user_id,video_timer,is_completed,OrgID,complete_per,total_video_timer) VALUES (@value1,@value2,@value3,@value4,@value5,@value6,@value7,@value8)";
            command.CommandText = str;
            command.Parameters.AddWithValue("value1", (object)ContentID);
            command.Parameters.AddWithValue("value2", (object)CategoryID);
            command.Parameters.AddWithValue("value3", (object)UserID);
            command.Parameters.AddWithValue("value4", (object)VideoTimer);
            command.Parameters.AddWithValue("value5", (object)IsCompleted);
            command.Parameters.AddWithValue("value6", (object)OrgID);
            command.Parameters.AddWithValue("value7", (object)CompletePer);
            command.Parameters.AddWithValue("value8", (object)TotalVideoTimer);
            this.conn.Open();
            command.ExecuteNonQuery();
            this.conn.Close();
        }

        public void UpdateVideoTimer(int ContentID, int CategoryID, int UserID, double VideoTimer)
        {
            MySqlCommand command = this.conn.CreateCommand();
            string str = "UPDATE tbl_prod_que_ans_video SET video_timer=@value1 WHERE ContentID=@value2 AND CategoryID=@value3 AND user_id=@value4";
            command.CommandText = str;
            command.Parameters.AddWithValue("value1", (object)VideoTimer);
            command.Parameters.AddWithValue("value2", (object)ContentID);
            command.Parameters.AddWithValue("value3", (object)CategoryID);
            command.Parameters.AddWithValue("value4", (object)UserID);
            this.conn.Open();
            command.ExecuteNonQuery();
            this.conn.Close();
        }

        /// For the coin
        public void InsertCoin(tbl_rs_type_qna record,string Tempoin)
        {
            MySqlCommand command = this.conn.CreateCommand();
            string str = "INSERT INTO tbl_coins_details( id_user, id_organization, id_assessment, attempt_number, total_question, right_answer_count,set_value,result_in_percentage,coins_scored,status) VALUES (@value1, @value2, @value3, @value4, @value5, @value6, @value8,@value9,@value10,@value11)";
            command.CommandText = str;
            command.Parameters.AddWithValue("value1", record.id_user); 
            command.Parameters.AddWithValue("value2", record.id_organization); 
            command.Parameters.AddWithValue("value3", record.id_assessment);
            command.Parameters.AddWithValue("value4", record.attempt_number); 
            command.Parameters.AddWithValue("value5", record.total_question);
            command.Parameters.AddWithValue("value6", record.right_answer_count); 
            //command.Parameters.AddWithValue("value7", record.wrong_answer_count); 
            command.Parameters.AddWithValue("value8",0);
            command.Parameters.AddWithValue("value9",record.result_in_percentage); 
            command.Parameters.AddWithValue("value10", Tempoin); 
            command.Parameters.AddWithValue("value11",record.status); 
            this.conn.Open();
            command.ExecuteNonQuery();
            this.conn.Close();
        }
      
        public List<tbl_coins_details> GetRecordDataCoin(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_coins_details> tblcoins_details = new List<tbl_coins_details>();
            while (mySqlDataReader.Read())
            {
                tbl_coins_details item = new tbl_coins_details();

                item.id_user = mySqlDataReader.GetInt32("id_user");
                item.id_organization = mySqlDataReader.GetInt32("id_organization");
                item.id_assessment = mySqlDataReader.GetInt32("id_assessment");
                item.attempt_number = mySqlDataReader.GetInt32("attempt_number");
                item.total_question = mySqlDataReader.GetInt32("total_question");
                item.right_answer_count = mySqlDataReader.GetInt32("right_answer_count");
                item.set_value = mySqlDataReader.GetInt32("set_value");
                item.result_in_percentage = mySqlDataReader.GetInt32("result_in_percentage");
                item.coins_scored = mySqlDataReader.GetInt32("coins_scored");
                item.status = mySqlDataReader.GetString("status");

                tblcoins_details.Add(item);
            }
            this.conn.Close();
            return tblcoins_details;

        }

       



        /// <returns></returns>
        //For the List of the Record tbl_rs_type_qna
        public List<tbl_rs_type_qna> GetRecordData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_rs_type_qna> tbltype_qna = new List<tbl_rs_type_qna>();
            while (mySqlDataReader.Read())
            {
                tbl_rs_type_qna item = new tbl_rs_type_qna();
   
                item.id_assessment_log = mySqlDataReader.GetInt32("id_assessment_log");
                item.id_user = mySqlDataReader.GetInt32("id_user");
                item.id_organization = mySqlDataReader.GetInt32("id_organization");
              //  item.id_assessment_sheet = mySqlDataReader.GetInt32("id_assessment_sheet");
                item.id_assessment_sheet = mySqlDataReader.IsDBNull(mySqlDataReader.GetOrdinal("id_assessment_sheet")) ? (int?)null : mySqlDataReader.GetInt32("id_assessment_sheet");

                item.id_assessment = mySqlDataReader.GetInt32("id_assessment");
                item.attempt_number =mySqlDataReader.GetInt32("attempt_number");
                item.total_question =mySqlDataReader.GetInt32("total_question");
                item.right_answer_count = mySqlDataReader.GetInt32("right_answer_count");
                item.wrong_answer_count =mySqlDataReader.GetInt32("wrong_answer_count");
                item.result_in_percentage = mySqlDataReader.GetDouble("result_in_percentage");
                item.status =mySqlDataReader.GetString("status");
              
                tbltype_qna.Add(item);
            }
            this.conn.Close();
            return tbltype_qna;

        }


        public List<tbl_rs_type_qna> GetRecordDataID(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_rs_type_qna> tbltype_qna = new List<tbl_rs_type_qna>();
            while (mySqlDataReader.Read())
            {
                tbl_rs_type_qna item = new tbl_rs_type_qna();

                item.id_assessment_log = mySqlDataReader.GetInt32("id_assessment_log");
                item.id_user = mySqlDataReader.GetInt32("id_user");
                item.id_organization = mySqlDataReader.GetInt32("id_organization");
                //  item.id_assessment_sheet = mySqlDataReader.GetInt32("id_assessment_sheet");
                item.id_assessment_sheet = mySqlDataReader.IsDBNull(mySqlDataReader.GetOrdinal("id_assessment_sheet")) ? (int?)null : mySqlDataReader.GetInt32("id_assessment_sheet");

                item.id_assessment = mySqlDataReader.GetInt32("id_assessment");
                item.attempt_number = mySqlDataReader.GetInt32("attempt_number");
                item.total_question = mySqlDataReader.GetInt32("total_question");
                item.right_answer_count = mySqlDataReader.GetInt32("right_answer_count");
                item.wrong_answer_count = mySqlDataReader.GetInt32("wrong_answer_count");
                item.result_in_percentage = mySqlDataReader.GetDouble("result_in_percentage");
                item.status = mySqlDataReader.GetString("status");

                tbltype_qna.Add(item);
            }
            this.conn.Close();
            return tbltype_qna;

        }


        //For the List of the Record tbl_coins_master

        public List<tbl_coins_master> GetRecordDataCoinMaster(string qry)
        {

            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_coins_master> tblcoinsmaster = new List<tbl_coins_master>();
            while (mySqlDataReader.Read())
            {
                tbl_coins_master item = new tbl_coins_master();

                item.Attempt_no = mySqlDataReader.GetInt32("Attempt_no");
                item.Set_percentage = mySqlDataReader.GetInt32("Set_percentage");
                item.Set_Score = mySqlDataReader.GetInt32("Set_Score");
                item.status = mySqlDataReader.GetString("status");
                item.Id_organization = mySqlDataReader.GetInt32("id_organization");
                item.Id_assessment = mySqlDataReader.GetInt32("id_assessment");            

                tblcoinsmaster.Add(item);
            }
            this.conn.Close();
            return tblcoinsmaster;
        }
        public List<tbl_category> getprg(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_category> tblCategoryList = new List<tbl_category>();
            while (mySqlDataReader.Read())
                tblCategoryList.Add(new tbl_category()
                {
                    ID_CATEGORY = Convert.ToInt32(mySqlDataReader["ID_CATEGORY"]),
                    CATEGORYNAME = mySqlDataReader["CATEGORYNAME"].ToString()
                });
            this.conn.Close();
            return tblCategoryList;
        }

        public List<KPIMasterRecord> GetKPIMasterRecordData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<KPIMasterRecord> tblKPIMasterRecordList = new List<KPIMasterRecord>();
            while (mySqlDataReader.Read())
                tblKPIMasterRecordList.Add(new KPIMasterRecord()
                {
                    ID_Scoring_Matrix = Convert.ToInt32(mySqlDataReader["ID_Scoring_Matrix"]),
                    ID_KPI = Convert.ToInt32(mySqlDataReader["ID_KPI"]),
                    ID_Assessment_Type = Convert.ToInt32(mySqlDataReader["ID_Assessment_Type"]),
                    Content_Assessment_ID = Convert.ToInt32(mySqlDataReader["Content_Assessment_ID"]),
                    ApplyMasterScoreMultipleAttempts = Convert.ToBoolean(mySqlDataReader["ApplyMasterScoreMultipleAttempts"]),
                    ApplyRightAnswerMultipleAttempts = Convert.ToBoolean(mySqlDataReader["ApplyRightAnswerMultipleAttempts"]),
                    KPI_Name = mySqlDataReader["KPI_Name"].ToString(),
                    ID_Organization = Convert.ToInt32(mySqlDataReader["ID_Organization"]),
                    game_id = Convert.ToInt32(mySqlDataReader["game_id"]),
                    KPI_Type = mySqlDataReader["KPI_Type"].ToString(),
                    KPI_SubType = mySqlDataReader["KPI_SubType"].ToString(),
                    Scoring_Logic = mySqlDataReader["Scoring_Logic"].ToString()
                });
            this.conn.Close();
            return tblKPIMasterRecordList;
        }

        public List<tbl_assessment_mastery_score_details> GetAssessmentMasteryScoreData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_assessment_mastery_score_details> tblKPIMasterRecordList = new List<tbl_assessment_mastery_score_details>();
            while (mySqlDataReader.Read())
                tblKPIMasterRecordList.Add(new tbl_assessment_mastery_score_details()
                {
                    Score = Convert.ToInt32(mySqlDataReader["Score"]),
                    Points = Convert.ToDecimal(mySqlDataReader["Points"])
                });
            this.conn.Close();
            return tblKPIMasterRecordList;
        }

        public List<tbl_assessment_right_answer_details> GetAssessmentRightAnsData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_assessment_right_answer_details> tblKPIMasterRecordList = new List<tbl_assessment_right_answer_details>();
            while (mySqlDataReader.Read())
                tblKPIMasterRecordList.Add(new tbl_assessment_right_answer_details()
                {
                    Points = Convert.ToDecimal(mySqlDataReader["Points"])
                });
            this.conn.Close();
            return tblKPIMasterRecordList;
        }

        public List<tbl_assessment_grid_details> GetAssessmentGridData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_assessment_grid_details> tblKPIMasterRecordList = new List<tbl_assessment_grid_details>();
            while (mySqlDataReader.Read())
                tblKPIMasterRecordList.Add(new tbl_assessment_grid_details()
                {
                    Points = Convert.ToDecimal(mySqlDataReader["Points"])
                });
            this.conn.Close();
            return tblKPIMasterRecordList;
        }

        public List<tbl_content_asessment_completion_timeframe_details> GetAssessmentCompletionTimeFrameData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_content_asessment_completion_timeframe_details> tblKPIMasterRecordList = new List<tbl_content_asessment_completion_timeframe_details>();
            while (mySqlDataReader.Read())
                tblKPIMasterRecordList.Add(new tbl_content_asessment_completion_timeframe_details()
                {
                    Category = mySqlDataReader["Category"].ToString() == "" ? 0 : Convert.ToInt32(mySqlDataReader["Category"]),
                    TimePeriod = mySqlDataReader["TimePeriod"].ToString() == "" ? 0 : Convert.ToInt32(mySqlDataReader["TimePeriod"]),
                    Points = mySqlDataReader["Points"].ToString() == "" ? 0 : Convert.ToDecimal(mySqlDataReader["Points"])
                });
            this.conn.Close();
            return tblKPIMasterRecordList;
        }

        public List<tbl_assessment_user_assignment> GetUserAssignmentAssessmentData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_assessment_user_assignment> tblKPIMasterRecordList = new List<tbl_assessment_user_assignment>();
            while (mySqlDataReader.Read())
                tblKPIMasterRecordList.Add(new tbl_assessment_user_assignment()
                {
                    start_date = Convert.ToDateTime(mySqlDataReader["start_date"]),
                    expire_date = Convert.ToDateTime(mySqlDataReader["expire_date"])
                });
            this.conn.Close();
            return tblKPIMasterRecordList;
        }

        public List<tbl_assessment> GetAssessmentData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_assessment> tblKPIMasterRecordList = new List<tbl_assessment>();
            while (mySqlDataReader.Read())
                tblKPIMasterRecordList.Add(new tbl_assessment()
                {
                    assess_start = Convert.ToDateTime(mySqlDataReader["assess_start"]),
                    assess_ended = Convert.ToDateTime(mySqlDataReader["assess_ended"])
                });
            this.conn.Close();
            return tblKPIMasterRecordList;
        }

        public List<tbl_user_kpi_data_log> GetUserKPILogDetailsData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_user_kpi_data_log> tblKPIMasterRecordList = new List<tbl_user_kpi_data_log>();
            while (mySqlDataReader.Read())
                tblKPIMasterRecordList.Add(new tbl_user_kpi_data_log()
                {
                    id_user = Convert.ToInt32(mySqlDataReader["id_user"]),
                    ID_KPI = Convert.ToInt32(mySqlDataReader["ID_KPI"]),
                    KPI_Name = mySqlDataReader["KPI_Name"].ToString(),
                    game_id = Convert.ToInt32(mySqlDataReader["game_id"]),
                    assessment_platform = mySqlDataReader["assessment_platform"].ToString(),
                    AttemptNo = Convert.ToInt32(mySqlDataReader["AttemptNo"]),
                    mastery_score = Convert.ToInt32(mySqlDataReader["mastery_score"]),
                    scoring_value = Convert.ToInt32(mySqlDataReader["scoring_value"]),
                    points_scored = Convert.ToDecimal(mySqlDataReader["points_scored"]),
                    TotalQuestions = Convert.ToInt32(mySqlDataReader["TotalQuestions"]),
                    KPI_SubType = mySqlDataReader["KPI_SubType"].ToString(),
                });
            this.conn.Close();
            return tblKPIMasterRecordList;
        }

        public int InsertUserKPIData(string Query)
        {
            int Output = 0;
            MySqlCommand command = this.conn.CreateCommand();
            ////string str = "INSERT INTO tbl_user_kpi_data_log(ContentID,CategoryID,user_id,video_timer,is_completed,OrgID,complete_per,total_video_timer) VALUES (@value1,@value2,@value3,@value4,@value5,@value6,@value7,@value8)";
            command.CommandText = Query;
            ////command.Parameters.AddWithValue("value1", (object)ContentID);
            ////command.Parameters.AddWithValue("value2", (object)CategoryID);
            ////command.Parameters.AddWithValue("value3", (object)UserID);
            ////command.Parameters.AddWithValue("value4", (object)VideoTimer);
            ////command.Parameters.AddWithValue("value5", (object)IsCompleted);
            ////command.Parameters.AddWithValue("value6", (object)OrgID);
            ////command.Parameters.AddWithValue("value7", (object)CompletePer);
            ////command.Parameters.AddWithValue("value8", (object)TotalVideoTimer);

            this.conn.Open();
            Output = command.ExecuteNonQuery();
            this.conn.Close();

            return Output;
        }

        public int InsertDataToDB(string Query)
        {
            int Output = 0;
            MySqlCommand command = this.conn.CreateCommand();
            ////string str = "INSERT INTO tbl_user_kpi_data_log(ContentID,CategoryID,user_id,video_timer,is_completed,OrgID,complete_per,total_video_timer) VALUES (@value1,@value2,@value3,@value4,@value5,@value6,@value7,@value8)";
            command.CommandText = Query;

            this.conn.Open();
            command.ExecuteNonQuery();
            Output = Convert.ToInt32(command.LastInsertedId);
            this.conn.Close();

            return Output;
        }

        public int UpdateDataToDB(string Query)
        {
            int Output = 0;
            MySqlCommand command = this.conn.CreateCommand();
            ////string str = "INSERT INTO tbl_user_kpi_data_log(ContentID,CategoryID,user_id,video_timer,is_completed,OrgID,complete_per,total_video_timer) VALUES (@value1,@value2,@value3,@value4,@value5,@value6,@value7,@value8)";
            command.CommandText = Query;

            this.conn.Open();
            Output = command.ExecuteNonQuery();
            this.conn.Close();

            return Output;
        }

        public List<tbl_assessment> AssessmentDetails_ContentData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_assessment> tblDataList = new List<tbl_assessment>();
            while (mySqlDataReader.Read())
                tblDataList.Add(new tbl_assessment()
                {
                    id_assessment = Convert.ToInt32(mySqlDataReader["id_assessment"]),
                    assessment_title = mySqlDataReader["assessment_title"].ToString()
                });
            this.conn.Close();
            return tblDataList;
        }


        public List<tbl_category> GetContentDetailsByCategoryData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_category> tblDataList = new List<tbl_category>();
            while (mySqlDataReader.Read())
                tblDataList.Add(new tbl_category()
                {
                    COUNT_REQUIRED = Convert.ToInt32(mySqlDataReader["COUNT_REQUIRED"]),
                    ID_PARENT = Convert.ToInt32(mySqlDataReader["ID_PARENT"]),
                    CATEGORYNAME = mySqlDataReader["CATEGORYNAME"].ToString()
                });
            this.conn.Close();
            return tblDataList;
        }

        public List<tbl_content_program_mapping> GetContentProgramMappingDatesData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_content_program_mapping> tblDataList = new List<tbl_content_program_mapping>();
            while (mySqlDataReader.Read())
                tblDataList.Add(new tbl_content_program_mapping()
                {
                    start_date = Convert.ToDateTime(mySqlDataReader["start_date"]),
                    expiry_date = Convert.ToDateTime(mySqlDataReader["expiry_date"])
                });
            this.conn.Close();
            return tblDataList;
        }

        public List<tbl_category_heading> GetContentCategoryHeadingData(string qry)
        {
            string str = qry;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_category_heading> tblDataList = new List<tbl_category_heading>();
            while (mySqlDataReader.Read())
                tblDataList.Add(new tbl_category_heading()
                {
                    id_category = Convert.ToInt32(mySqlDataReader["id_category"]),
                    id_category_heading = Convert.ToInt32(mySqlDataReader["id_category_heading"]),
                    Heading_title = mySqlDataReader["heading_title"].ToString(),
                    id_category_tiles = Convert.ToInt32(mySqlDataReader["id_category_tiles"]),
                    status = mySqlDataReader["status"].ToString()
                });
            this.conn.Close();
            return tblDataList;
        }

        public List<int>IsAssessmentIdCheck(string SqlQuery)
        {
            string str1 = SqlQuery;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str1;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<int> id_assessment = new List<int>();
            while (mySqlDataReader.Read())
            {
                id_assessment.Add(Convert.ToInt32(mySqlDataReader["id_assessment"].ToString()));
            }
            this.conn.Close();
            return id_assessment;
        }

        public List<Location> GetStoredLocation(string SqlQuery)
        {
            string str1 = SqlQuery;
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str1;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            string str2 = "";
            List<Location> lstLocation = new List<Location>();
            while (mySqlDataReader.Read())
                lstLocation.Add(new Location()
                {
                    LOCATION = mySqlDataReader["LOCATION"].ToString()
                });
            this.conn.Close();
            return lstLocation;
        }

        public List<tbl_user> getStoredLocationUserDetails(string str)
        {
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_user> lstUser = new List<tbl_user>();
            while (mySqlDataReader.Read())
            {
                lstUser.Add(new tbl_user()
                {
                    ID_USER = Convert.ToInt32(mySqlDataReader["ID_USER"]),
                    USERID = mySqlDataReader["USERID"].ToString()
                });
            }
            this.conn.Close();

            return lstUser;
        }

        public List<tbl_scheduled_event> getUserScheduledEventDetails(string str)
        {
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<tbl_scheduled_event> lstUser = new List<tbl_scheduled_event>();
            while (mySqlDataReader.Read())
            {
                lstUser.Add(new tbl_scheduled_event()
                {
                    id_scheduled_event = Convert.ToInt32(mySqlDataReader["id_scheduled_event"]),
                    event_title = mySqlDataReader["event_title"].ToString(),
                    event_description = mySqlDataReader["event_description"].ToString(),
                    event_online_url = mySqlDataReader["event_online_url"].ToString(),
                    program_start_date = Convert.ToDateTime(mySqlDataReader["program_start_date"]),
                    program_end_date = Convert.ToDateTime(mySqlDataReader["program_end_date"].ToString()),
                    Start_Time = mySqlDataReader["Start_Time"].ToString(),
                    End_Time = mySqlDataReader["End_Time"].ToString(),
                    event_duration = mySqlDataReader["event_duration"].ToString(),
                    program_day = mySqlDataReader["program_day"].ToString(),
                    program_duration = Convert.ToInt32(mySqlDataReader["program_duration"]),
                    id_event_creator = Convert.ToInt32(mySqlDataReader["id_event_creator"]),
                });
            }
            this.conn.Close();

            return lstUser;
        }

        public List<SearchAPI> getSearchedAPIData(string str)
        {
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<SearchAPI> lstUser = new List<SearchAPI>();
            while (mySqlDataReader.Read())
            {
                lstUser.Add(new SearchAPI()
                {
                    ID_USER = Convert.ToInt32(mySqlDataReader["ID_USER"]),
                    USERID = mySqlDataReader["USERID"].ToString(),
                    ID_CODE = Convert.ToInt32(mySqlDataReader["ID_CODE"]),
                    ID_ORGANIZATION = Convert.ToInt32(mySqlDataReader["ID_ORGANIZATION"]),
                    ID_ROLE = Convert.ToInt32(mySqlDataReader["ID_ROLE"]),
                    EMAIL = mySqlDataReader["EMAIL"].ToString(),
                    AGE = mySqlDataReader["AGE"].ToString(),
                    EMPLOYEEID = mySqlDataReader["EMPLOYEEID"].ToString(),
                    user_department = mySqlDataReader["user_department"].ToString(),
                    user_designation = mySqlDataReader["user_designation"].ToString(),
                    user_function = mySqlDataReader["user_function"].ToString(),
                    user_grade = mySqlDataReader["user_grade"].ToString(),
                    reporting_manager = Convert.ToInt32(mySqlDataReader["reporting_manager"]),
                    FIRSTNAME = mySqlDataReader["FIRSTNAME"].ToString(),
                    LASTNAME = mySqlDataReader["LASTNAME"].ToString(),
                    LOCATION = mySqlDataReader["LOCATION"].ToString(),
                });
            }
            this.conn.Close();

            return lstUser;
        }

        public List<Assessment_ID_for_certification> getUserAssignment(string str)
        {
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<Assessment_ID_for_certification> UserDetails = new List<Assessment_ID_for_certification>();


            while (mySqlDataReader.HasRows)
            {
                //Console.WriteLine("\t{0}\t{1}", mySqlDataReader.GetName(0),
                //mySqlDataReader.GetName(1));

                while (mySqlDataReader.Read())
                {
                    Assessment_ID_for_certification tbl_Assessment_ = new Assessment_ID_for_certification();
                    tbl_Assessment_.id_assessment = Convert.ToInt32(mySqlDataReader["id_assessment"]);

                    UserDetails.Add(tbl_Assessment_);
                }
                mySqlDataReader.NextResult();
            }

            this.conn.Close();
            return UserDetails;
        }

        public List<Assessment_ID_for_certification> getAssessmentCategotyMapping(string str)
        {
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            List<Assessment_ID_for_certification> UserDetails = new List<Assessment_ID_for_certification>();


            while (mySqlDataReader.HasRows)
            {
                //Console.WriteLine("\t{0}\t{1}", mySqlDataReader.GetName(0),
                //mySqlDataReader.GetName(1));

                while (mySqlDataReader.Read())
                {
                    Assessment_ID_for_certification tbl_Assessment_ = new Assessment_ID_for_certification();

                    tbl_Assessment_.assessment_title = mySqlDataReader["assessment_title"].ToString();

                    if (mySqlDataReader.GetName(0).Equals("id_assessment"))
                    {
                        tbl_Assessment_.id_assessment = Convert.ToInt32(mySqlDataReader["id_assessment"]);
                    }
                    if (ColumnExists(mySqlDataReader, "certificateFileName"))
                    {
                        tbl_Assessment_.CertificateFileName = mySqlDataReader["certificateFileName"].ToString();
                    }

                    if (ColumnExists(mySqlDataReader, "createdDate"))
                    {
                        tbl_Assessment_.date = mySqlDataReader["createdDate"].ToString();
                    }
                    if (ColumnExists(mySqlDataReader, "pdfURL"))
                    {
                        tbl_Assessment_.pdfURL = mySqlDataReader["pdfURL"].ToString();
                    }
                    if (ColumnExists(mySqlDataReader, "id_heading") && mySqlDataReader["id_heading"] != DBNull.Value)
                    {
                        tbl_Assessment_.id_heading = Convert.ToInt32(mySqlDataReader["id_heading"]);
                    }
                    else
                    {
                        tbl_Assessment_.id_heading = 0; // Or assign a default value if applicable
                    }
                    if (ColumnExists(mySqlDataReader, "Heading_title"))
                    {
                        tbl_Assessment_.Heading_title = mySqlDataReader["Heading_title"].ToString();
                    }
                    UserDetails.Add(tbl_Assessment_);
                }
                mySqlDataReader.NextResult();
            }

            this.conn.Close();
            return UserDetails;
        }
        static bool ColumnExists(MySqlDataReader reader, string columnName)
        {
            // Get the schema table
            var schemaTable = reader.GetSchemaTable();

            // Check if the column exists in the schema table
            return schemaTable != null && schemaTable.Rows
                .OfType<System.Data.DataRow>()
                .Any(row => row["ColumnName"].ToString() == columnName);
        }
        public UserDataForCertificate getScore(string str, int _cond)
        {
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();

            UserDataForCertificate tbl_Assessment_ = new UserDataForCertificate() ;

            while (mySqlDataReader.Read())
            {
                

                if (_cond == 1)
                {
                    if (mySqlDataReader["attempt_number"] != DBNull.Value)
                    {
                        tbl_Assessment_.attempt_no = Convert.ToInt32(mySqlDataReader["attempt_number"]);
                    }
                }
                else
                {
                    if (mySqlDataReader["attempt_number"] != DBNull.Value)
                    {
                        tbl_Assessment_.attempt_no = Convert.ToInt32(mySqlDataReader["attempt_number"]);
                    }
                    if (mySqlDataReader["scoring_value"] != DBNull.Value)
                    {
                        tbl_Assessment_.scoring_value = Convert.ToInt32(mySqlDataReader["scoring_value"]);
                    }
                }
                

            }

            this.conn.Close();
            return tbl_Assessment_;
        }


        public int? getCertificatepercentage(string str) // Change return type to int?
        {
            int? value = null; // Nullable int to handle NULL values properly

            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();

            if (mySqlDataReader.Read()) // Read the first row
            {
                value = mySqlDataReader["certificate_percentage"] != DBNull.Value
                    ? Convert.ToInt32(mySqlDataReader["certificate_percentage"])
                    : (int?)null; // Assign NULL if the database value is NULL
            }

            mySqlDataReader.Close(); // Close the reader
            this.conn.Close(); // Close the connection

            return value; // Returns either a valid integer or NULL
        }

        public UserDataForCertificate getUserCertificateData(string str, int _case)
        {
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();

            UserDataForCertificate userDataFor = new UserDataForCertificate();

            while (mySqlDataReader.Read())
            {
                switch (_case)
                {
                    case 1:
                        var _fName = mySqlDataReader["firstname"].ToString();
                        var _lName = mySqlDataReader["lastname"].ToString();
                        userDataFor.region = mySqlDataReader["region"].ToString();
                        userDataFor._userName = _fName + " " + _lName;
                        break;
                    case 2:
                        userDataFor.designation = mySqlDataReader["user_designation"].ToString();
                        userDataFor.department = mySqlDataReader["user_department"].ToString();
                        break;
                    case 3:
                        userDataFor.assessment_title = mySqlDataReader["assessment_title"].ToString();
                        break;
                    case 4:
                        userDataFor.date = Convert.ToDateTime(mySqlDataReader["created_date"]).ToString("yyyy/MM/dd");
                        userDataFor.attempt_no = Convert.ToInt32(mySqlDataReader["AttemptNo"]);
                        userDataFor.scoring_value = Convert.ToInt32(mySqlDataReader["scoring_value"]);
                        break;
                }
            }

            this.conn.Close();
            return userDataFor;
        }

        public tbl_rs_type_qna_Attempt GetUserMaxAttemptDetails(string str)
        {
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            tbl_rs_type_qna_Attempt lstUser = new tbl_rs_type_qna_Attempt();

            while (mySqlDataReader.Read())
            {
                lstUser.Attemp_No = Convert.ToInt32(mySqlDataReader["attempt_no"]);
                lstUser.LastAttempt = Convert.ToDateTime(mySqlDataReader["recorded_timestamp"].ToString());
            }
            this.conn.Close();

            return lstUser;
        }



        public bool checkCertificateExistOrNot(string str)
        {
            this.conn.Open();
            MySqlCommand command = this.conn.CreateCommand();
            command.CommandText = str;
            MySqlDataReader mySqlDataReader = command.ExecuteReader();
            tbl_assessment_audit lstUser = new tbl_assessment_audit();

            while (mySqlDataReader.Read())
            {
                this.conn.Close();
                return true;
            }

            this.conn.Close();
            return false;
        }

        public void addCertificateIntoTable(Certificate_log certificate_Log)
        {
            MySqlCommand command = this.conn.CreateCommand();
            string str = "INSERT INTO tbl_certificate_log(id_assessment, id_user, score, certificateFileName, addedDate, attempt_no, pdfURL,id_heading) VALUES (@value1,@value2,@value3, @value4,@value5,@value6,@value7,@value8)";
            command.CommandText = str;
            command.Parameters.AddWithValue("value1", (object)certificate_Log.id_assessment);
            command.Parameters.AddWithValue("value2", (object)certificate_Log.id_user);
            command.Parameters.AddWithValue("value3", (object)certificate_Log.score);
            command.Parameters.AddWithValue("value4", (object)certificate_Log.certificateFileName);
            command.Parameters.AddWithValue("value5", (object)certificate_Log.addedDate);
            command.Parameters.AddWithValue("value6", (object)certificate_Log.attempt_no);
            command.Parameters.AddWithValue("value7", (object)certificate_Log.pdfURL);
            command.Parameters.AddWithValue("value8", (object)certificate_Log.idheading);
            this.conn.Open();
            command.ExecuteNonQuery();
            this.conn.Close();
        }

        public List<CertificateAssignmentTheme> GetCertificateDataListThem(string Select_theme, string org_id)
        {
            List<CertificateAssignmentTheme> certificateList = new List<CertificateAssignmentTheme>();

            using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
            {
                string query = @"SELECT * FROM tbl_certificate_assignment_theme WHERE Status = 'A' AND Select_theme ="+Select_theme+ " AND Id_organization=" +org_id+"";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    connection.Open();
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CertificateAssignmentTheme certificate = new CertificateAssignmentTheme();
                            certificate.IdCertificate = Convert.ToInt32(reader["Id_Certificate"]);
                            certificate.SelectTheme = reader["Select_theme"].ToString();
                            certificate.HeaderThemeFirst = reader["Header_themefirst"].ToString();
                            certificate.SubText1ThemeFirst = reader["Sub_text1themefirst"].ToString();
                            certificate.SubText2ThemeFirst = reader["Sub_text2themefirst"].ToString();
                            certificate.SubText3ThemeFirst = reader["Sub_text3themefirst"].ToString();
                            certificate.TextColorHeaderThemeFirst = reader["Text_colorheaderthemfirst"].ToString();
                            certificate.TextColorSubTextThemeFirst = reader["Text_colorsubtextthemfirst"].ToString();
                            certificate.LogoImageLeftThemeFirst = reader["Logo_image_leftthemefirst"].ToString();
                            certificate.LogoImageRightThemeFirst = reader["Logo_image_rightthemefirst"].ToString();
                            certificate.BackgroundImageThemeFirst = reader["Backgrount_imagethemefirst"].ToString();


                            certificate.HeaderThemeSecond = reader["Header_themesecond"].ToString();
                            certificate.SubTextFirstThemeSecond = reader["Sub_textfirstthemsecond"].ToString();
                            certificate.RightNameThemeSecond = reader["Right_namethemesecond"].ToString();
                            certificate.LeftDesignationThemeSecond = reader["Left_designationthemesecond"].ToString();
                            certificate.RightDepartmentThemeSecond = reader["Right_depermentthemesecond"].ToString();
                            certificate.LeftRegionThemeSecond = reader["Left_regionthemesecond"].ToString();
                            certificate.SubText2ThemeSecond = reader["Sub_text2themsecond"].ToString();
                            certificate.TextColorHeaderThemeSecond = reader["Text_colorheaderthemesecond"].ToString();
                            certificate.TextColorThemeSecond = reader["Text_colorthemesecond"].ToString();
                            certificate.BackgroundImageThemeSecond = reader["Background_imagethemesecond"].ToString();
                            certificate.LogoImageLeftThemeSecond = reader["Logo_imageleftthemesecond"].ToString();
                            certificate.LogoImageRightThemeSecond = reader["Logo_imagerightthemesecond"].ToString();
                      




                            certificateList.Add(certificate);
                        }
                    }
                }
            }

            return certificateList;
        }


        public void AddUserDataLog(string ID_USER, int id_ORGANIZATION,string Page1)
        {
            string host = Dns.GetHostName();
            string ipAddress = "";
             ipAddress = GetUserIPv4Address();



            string query = "INSERT INTO tbl_userdata_log (IdUser, OrgId, Ipaddress, Page,Update_date) VALUES (@IdUser, @OrgId, @Ipaddress, @Page,@Update_date)";

            using (MySqlCommand command = new MySqlCommand(query, this.conn))
            {
                command.Parameters.AddWithValue("@IdUser", ID_USER);
                command.Parameters.AddWithValue("@OrgId", id_ORGANIZATION);
                command.Parameters.AddWithValue("@Ipaddress", ipAddress); // You need to determine how to get the IP address
                command.Parameters.AddWithValue("@Page", Page1); // You need to determine how to get the page URL
                command.Parameters.AddWithValue("@Update_date", DateTime.Now); // You need to determine how to get the page URL

                this.conn.Open();
                command.ExecuteNonQuery();
                this.conn.Close(); // Close the connection after execution
            }
        }

        static string GetUserIPv4Address()
        {
            string userIPAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(userIPAddress))
            {
                userIPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                // X-Forwarded-For may return multiple IP addresses separated by comma
                // We'll use the first one, assuming it's the client's IP address
                userIPAddress = userIPAddress.Split(',')[0];
            }

            // Check if the retrieved IP address is in IPv6 format
            // If it is, we'll use the local machine's IPv4 address instead
            if (userIPAddress == "::1")
            {
                userIPAddress = GetLocalIPv4Address();
            }

            return userIPAddress;
        }

        static string GetLocalIPv4Address()
        {
            string localIPv4Address = "";
            foreach (var netInterface in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211 ||
                    netInterface.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet)
                {
                    foreach (var addrInfo in netInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (addrInfo.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            localIPv4Address = addrInfo.Address.ToString();
                            break;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(localIPv4Address))
                {
                    break;
                }
            }
            return localIPv4Address;
        }

        public bool InsertTourData(int org_id, int user_id, string page_name)
        {
            try
            {
                using (MySqlCommand command = this.conn.CreateCommand())
                {
                    // Using parameterized query to prevent SQL injection
                    string query = "INSERT INTO tbl_product_tour_details(Id_orgnization, Id_user, Page_name, Status) VALUES (@org_id, @user_id, @page_name, @status)";
                    command.CommandText = query;

                    // Adding parameters
                    command.Parameters.AddWithValue("@org_id", org_id);
                    command.Parameters.AddWithValue("@user_id", user_id);
                    command.Parameters.AddWithValue("@page_name", page_name);
                    command.Parameters.AddWithValue("@status", "A"); // Assuming Status column is present and has a default value

                    this.conn.Open();
                    command.ExecuteNonQuery();
                }
                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions appropriately (e.g., log the error)
                Console.WriteLine("Error inserting tour data: " + ex.Message);
                return false;
            }
            finally
            {
                // Make sure to close the connection
                if (this.conn.State != ConnectionState.Closed)
                    this.conn.Close();
            }
        }

        public bool GetTourDataExistOrNot(int org_id, int user_id, string page_name)
        {
            this.conn.Open();

            string query = "SELECT * FROM tbl_product_tour_details WHERE Id_orgnization = @orgId AND Id_user = @userId AND Page_name = @pageName";

            using (MySqlCommand command = new MySqlCommand(query, conn))
            {
                // Add parameters
                command.Parameters.AddWithValue("@orgId", org_id);
                command.Parameters.AddWithValue("@userId", user_id);
                command.Parameters.AddWithValue("@pageName", page_name);
                try
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                            return true;
                        else
                            return false; //
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return false;
        }
    }
}
