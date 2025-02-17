using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class tbl_event_attendence
    {
        public int id_event_attendence { get; set; }

        public int id_scheduled_event { get; set; }

        public int id_user { get; set; }

        public string userid { get; set; }

        public int id_organization { get; set; }

        public DateTime attended_date { get; set; }

        public char Status { get; set; }
    }
}