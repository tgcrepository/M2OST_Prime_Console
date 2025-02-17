using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class CourseCompletion
    {
        public Nullable<int> Duration { get; set; }
        public Nullable<int> DaysToComplete { get; set; }
        public string StartDate { get; set; }
        public string ExpiryDate { get; set; }
        public double TotalCompletionPer { get; set; }
    }


}