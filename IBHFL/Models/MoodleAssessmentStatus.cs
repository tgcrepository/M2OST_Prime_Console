using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class MoodleAssessmentStatus
    {
        public int IdUser { get; set; }
        public int IdOrganization { get; set; }
        public int AttemptNumber { get; set; }
        public string Status { get; set; }
    }
}