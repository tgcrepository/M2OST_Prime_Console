using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class VideoModel
    {
    }
    public class tbl_video_configuration
    {

        public int Id_video { get; set; }
        public int Id_organization { get; set; }

        public string Header_text { get; set; }

        public string Video_name_web { get; set; }
        public HttpPostedFileBase Video_name_webFile { get; set; }

        public string Video_name_mobile { get; set; }
        public HttpPostedFileBase Video_name_mobileFile { get; set; }

        public string Organization_name { get; set; }

    }
    public class addVideoModel
    {

        public List<tbl_video_configuration> GetVideoData(int num)
        {


            List<tbl_video_configuration> videoList = new List<tbl_video_configuration>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["dbconnectionstring"].ConnectionString))
                {
                    // SQL query to fetch feedback data
                    string query = "SELECT * FROM tbl_video_configuration WHERE Id_organization = @orgId AND Status = 'A' ORDER BY Id_video DESC LIMIT 1";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // Set the organization ID parameter
                        command.Parameters.AddWithValue("@orgId", num);

                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tbl_video_configuration video = new tbl_video_configuration();
                                video.Id_video = Convert.ToInt32(reader["Id_video"]);
                                video.Id_organization = Convert.ToInt32(reader["Id_organization"]);
                                video.Header_text = reader["Header_text"].ToString();
                                video.Video_name_web = reader["Video_name_web"].ToString();
                                video.Video_name_mobile = reader["Video_name_mobile"].ToString();
                              
                                videoList.Add(video);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                //Console.WriteLine("Error occurred: " + ex.Message);
                // You can log the exception to a file or a logging framework here
            }

            return videoList;
        }
    }
}