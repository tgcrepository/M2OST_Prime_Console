using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IBHFL.Models
{
    public class Certificate_log
    {
        public int? id_assessment { get; set; }

        public int? id_user { get; set; }

        public double score { get; set; }

        public string certificateFileName { get; set; }

        public DateTime addedDate { get; set; }

        public int? attempt_no { get; set; }

        public string pdfURL { get; set; }
        public int idheading { get; set; }
    }

    public class CertificateAssignmentTheme
    {
        public int IdCertificate { get; set; }
        public int Id_organization { get; set; }

        public string SelectTheme { get; set; }
        public string HeaderThemeFirst { get; set; }
        public string SubText1ThemeFirst { get; set; }
        public string SubText2ThemeFirst { get; set; }
        public string SubText3ThemeFirst { get; set; }
        public string TextColorHeaderThemeFirst { get; set; }
        public string TextColorSubTextThemeFirst { get; set; }
        public string LogoImageLeftThemeFirst { get; set; }

        public HttpPostedFileBase LogoImageLeftThemeFirstUploadDir { get; set; }
        public string LogoImageRightThemeFirst { get; set; }


        public HttpPostedFileBase LogoImageRightThemeFirstUploadDir { get; set; }
        public string BackgroundImageThemeFirst { get; set; }

        public HttpPostedFileBase BackgroundImageThemeFirstUploadDir { get; set; }





        public string HeaderThemeSecond { get; set; }
        public string SubTextFirstThemeSecond { get; set; }
        public string RightNameThemeSecond { get; set; }
        public string LeftDesignationThemeSecond { get; set; }
        public string RightDepartmentThemeSecond { get; set; }
        public string LeftRegionThemeSecond { get; set; }
        public string SubText2ThemeSecond { get; set; }
        public string TextColorHeaderThemeSecond { get; set; }
        public string TextColorThemeSecond { get; set; }
        public string BackgroundImageThemeSecond { get; set; }
        public string LogoImageLeftThemeSecond { get; set; }
        public string LogoImageRightThemeSecond { get; set; }
    }


}