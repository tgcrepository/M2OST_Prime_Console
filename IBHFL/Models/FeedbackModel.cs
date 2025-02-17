using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class FeedbackModel
    {
    }
    public class tbl_feedback_configuration
    {
        public int Id_Feedback { get; set; }
        public int Organization_id { get; set; }
        public string Image_path { get; set; } // Store the file path as a string
        public HttpPostedFileBase ImageFile { get; set; }
        public string Header_Text { get; set; }
        public string Feedback_Text { get; set; }
        public string Text_Button_Colour { get; set; }

        public string Organization_name { get; set; }
    }

    public class addFeedbackModel
    {
      
        public List<tbl_feedback_configuration> GetFeedbackData(int num)
        {


            List<tbl_feedback_configuration> feedbackList = new List<tbl_feedback_configuration>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    // SQL query to fetch feedback data
                    string query = "SELECT * FROM tbl_feedback_configuration WHERE Organization_id = @orgId AND Status = 'A' ORDER BY Id_Feedback DESC LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Set the organization ID parameter
                        command.Parameters.AddWithValue("@orgId", num);

                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tbl_feedback_configuration feedback = new tbl_feedback_configuration();
                                feedback.Id_Feedback = Convert.ToInt32(reader["Id_Feedback"]);
                                feedback.Organization_id = Convert.ToInt32(reader["Organization_id"]);
                                feedback.Image_path = reader["Image_path"].ToString();
                                feedback.Header_Text = reader["Header_Text"].ToString();
                                feedback.Feedback_Text = reader["Feedback_Text"].ToString();
                                feedback.Text_Button_Colour = reader["Text_Button_Colour"].ToString();
                                feedbackList.Add(feedback);
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

            return feedbackList;
        }
    }
    }

 