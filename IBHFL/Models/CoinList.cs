using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class CoinList
    {


    }
    public class tbl_coins_master
    {
        public int Id_Coins { get; set; }
        public int Attempt_no { get; set; }
        public int Set_percentage { get; set; }
        public int Set_Score { get; set; }
        public string status { get; set; }
        public int Id_organization { get; set; }
        public int Id_assessment { get; set; }
        public string Created_by { get; set; }
        public DateTime Created_date { get; set; }

    }
    public class tbl_coins_details
    {
        public int id_user { get; set; }
        public int id_organization { get; set; }
        public int id_assessment { get; set; }
        public int attempt_number { get; set; }
        public int total_question { get; set; }
        public int right_answer_count { get; set; }
        public int set_value { get; set; }
        public int result_in_percentage { get; set; }
        public int coins_scored { get; set; }
        public string status { get; set; }
        public string Update_date { get; set; }
    }
}