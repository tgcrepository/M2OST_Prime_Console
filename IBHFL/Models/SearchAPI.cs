using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class SearchAPI
    {
        public int ID_USER { get; set; }

        public int ID_CODE { get; set; }

        public int? ID_ORGANIZATION { get; set; }

        public int ID_ROLE { get; set; }

        public string USERID { get; set; }

        public string EMAIL { get; set; }
        public string AGE { get; set; }

        public string PASSWORD { get; set; }

        public string FBSOCIALID { get; set; }

        public string GPSOCIALID { get; set; }

        public string STATUS { get; set; }

        public DateTime UPDATEDTIME { get; set; }

        public DateTime? EXPIRY_DATE { get; set; }

        public string EMPLOYEEID { get; set; }

        public string user_department { get; set; }

        public string user_designation { get; set; }

        public string user_function { get; set; }

        public string user_grade { get; set; }

        public string user_status { get; set; }

        public int? reporting_manager { get; set; }

        public int? is_reporting { get; set; }
        public string FIRSTNAME { get; set; }
        public string LASTNAME { get; set; }
        public string LOCATION { get; set; }
    }
}