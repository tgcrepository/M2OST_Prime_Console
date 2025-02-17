using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class LoginPageModel
    {
    }
    public class tbl_login_page
    {
       public int Id_login { get; set; }
       public int Organization { get; set; }
       public string Background_Image { get; set; }
       public HttpPostedFileBase BGImageFile { get; set; }
       public string Logo_Image { get; set; }
       public HttpPostedFileBase LogoImageFile { get; set; }
       public string Text_Button_Color { get; set; }
       public string Status { get; set; }
       public string Organization_name { get; set; }

    }

    public class addLoginPageModel
    {

        public List<tbl_login_page> GetLoginPageData(int num)
        {


            List<tbl_login_page> LoginPageList = new List<tbl_login_page>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    // SQL query to fetch feedback data
                    string query = "SELECT * FROM tbl_login_page WHERE Organization = @orgId AND Status = 'A' ORDER BY Id_login DESC LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Set the organization ID parameter
                        command.Parameters.AddWithValue("@orgId", num);

                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tbl_login_page login = new tbl_login_page();
                                login.Id_login = Convert.ToInt32(reader["Id_login"]);
                                login.Organization = Convert.ToInt32(reader["Organization"]);
                                login.Background_Image = reader["Background_Image"].ToString();
                                login.Logo_Image = reader["Logo_Image"].ToString();
                                login.Text_Button_Color = reader["Text_Button_Color"].ToString();
                                LoginPageList.Add(login);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error occurred: " + ex.Message);
                // You can log the exception to a file or a logging framework here
            }

            return LoginPageList;
        }
    }
}