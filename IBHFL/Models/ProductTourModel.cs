using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class ProductTourModel
    {
    }
    public class tbl_product_tour_details
    {
        public int Id_product { get; set; }
        public int Id_orgnization { get; set; }
        public int Id_user { get; set; } 
        public string Page_name { get; set; }
        public string Status { get; set; }
       
    }

}