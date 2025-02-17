using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class KPIMasterRecord
    {
        public int ID_Scoring_Matrix { get; set; }
        public int ID_KPI { get; set; }
        public int ID_Assessment_Type { get; set; }
        public int Content_Assessment_ID { get; set; }
        public bool ApplyMasterScoreMultipleAttempts { get; set; }
        public bool ApplyRightAnswerMultipleAttempts { get; set; }
        public string KPI_Name { get; set; }
        public int ID_Organization { get; set; }
        public int game_id { get; set; }
        public string KPI_Type { get; set; }
        public string KPI_SubType { get; set; }
        public string Scoring_Logic { get; set; }
    }
}