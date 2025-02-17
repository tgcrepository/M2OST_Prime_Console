using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class UserDataForCertificate
    {
        public string _userName { get; set; }
        public string region { get; set; }
        public string designation { get; set; }
        public string date { get; set; }
        public string department { get; set; }
        public string assessment_title { get; set; }
        public int? attempt_no { get; set; }
        public double? scoring_value { get; set; }
        public string CertificateFileName { get; set; }
        public string pdfURL { get; set; }

        public string orgid1 { get; set; }
        public int id_heading { get; set; }
        public string Heading_title { get; set; }
      


    }
}