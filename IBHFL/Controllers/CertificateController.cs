using DinkToPdf;
using DinkToPdf.Contracts;
using IBHFL.Models;
using Microsoft.AspNet.Identity;
using Microsoft.IdentityModel.Tokens;
using Rotativa;
using RusticiSoftware.HostedEngine.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using iText.Html2pdf;
using iText.Kernel.Pdf;
using iText.Html2pdf;
using iText.Html2pdf.Resolver.Font;
using iText.Layout.Font;
using iText.Layout;
using iText.Kernel.Geom;
using Org.BouncyCastle.Utilities.Collections;
using iText.Layout.Element;


namespace IBHFL.Controllers
{
    //[SessionExpireFilter]
    public class CertificateController : Controller
    {
        private db_m2ostEntities db = new db_m2ostEntities();
    

        // GET: Certificate
        [Route("download/certificate")]
        [Obsolete]
        public ActionResult CreateCertificate(int? assessmentID, int? id_user, string _attemptNo,string orgid,int headingid)
        {
            var tbl_profile = getData(1, id_user, assessmentID);

            var tbl_user = getData(2, id_user, assessmentID);

            //var assessmentTitle = getData(3, id_user, assessmentID);

            var created_date = getData(4, id_user, assessmentID);

            var _fileName = id_user + "_" + assessmentID + "_" + _attemptNo + "_Certificate.pdf";


            //string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Certificates/");
            string path = @"D:\\Certificates\\Content\\";

            var _existQuery = "Select *  from tbl_certificate_log where id_user = " + id_user + " and certificateFileName = '" + _fileName + "' and id_assessment = " + assessmentID + " and id_heading = " + headingid;
            var _exist = new UtilityModel().checkCertificateExistOrNot(_existQuery);

            //var _pdf = new ActionAsPdf("Index", new { _userName = tbl_profile._userName, _region = tbl_profile.region, designation = tbl_user.designation, _date = created_date.date, _department = tbl_user.department }) { FileName = _fileName };

            var _pdf = new ActionAsPdf("Index", new { _userName = tbl_profile._userName, _region = tbl_profile.region, designation = tbl_user.designation, _date = created_date.date, _department = tbl_user.department, orgid })
            {
                FileName = _fileName,
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4
            };

            if (!_exist)
            {

                var byteArray = _pdf.BuildPdf(ControllerContext);
                var fileStream = new FileStream(path + _fileName, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();

                var _updateTable = new Certificate_log()
                {
                    id_assessment = assessmentID,
                    id_user = id_user,
                    score = Convert.ToDouble(created_date.scoring_value),
                    certificateFileName = _fileName,
                    addedDate = DateTime.Now,
                    attempt_no = Convert.ToInt32(_attemptNo),
                    pdfURL = @"https://www.m2ost.in/certificates/" + _fileName,
                    idheading = headingid
                };

                new UtilityModel().addCertificateIntoTable(_updateTable);
            }

            return _pdf;
        }

        public string DownloadeCreateCertificate1(int? assessmentID, int? id_user, string _attemptNo, string orgid, int headingid)
        {
            string value = "1";
          

            if (assessmentID == null || id_user == null || string.IsNullOrEmpty(_attemptNo))
            {

            }
            else
            {

            

            var tbl_profile = getData(1, id_user, assessmentID);
            var tbl_user = getData(2, id_user, assessmentID);
            var created_date = getData(4, id_user, assessmentID); // Ensure this method is implemented correctly.

            var _fileName = $"{id_user}_{assessmentID}_{_attemptNo}_Certificate.pdf";
            var _existQuery = "Select *  from tbl_certificate_log where id_user = " + id_user + " and certificateFileName = '" + _fileName + "' and id_assessment = " + assessmentID + " and id_heading = " + headingid;
            var _exist = new UtilityModel().checkCertificateExistOrNot(_existQuery);
                var _pdf = new ActionAsPdf("Index", new { _userName = tbl_profile._userName, _region = tbl_profile.region, designation = tbl_user.designation, _date = created_date.date, _department = tbl_user.department, orgid })
                {
                    FileName = _fileName,
                    PageOrientation = Rotativa.Options.Orientation.Landscape,
                    PageSize = Rotativa.Options.Size.A4
                };

                if (!_exist)
                {

                    var _updateTable = new Certificate_log()
                    {
                        id_assessment = assessmentID,
                        id_user = id_user,
                        score = Convert.ToDouble(created_date?.scoring_value ?? 0),
                        certificateFileName = _fileName,
                        addedDate = DateTime.Now,
                        attempt_no = Convert.ToInt32(_attemptNo),
                        pdfURL = $"https://www.m2ost.in/certificates/{_fileName}",
                        idheading = headingid
                    };

                    // Add the certificate log to the database
                    var utilityModel = new UtilityModel();
                    utilityModel.addCertificateIntoTable(_updateTable);
                }
            }

            return value;


        }


        // test 
        public string DownloadeCreateCertificate(int? assessmentID, int? id_user, string _attemptNo, string orgid, int headingid)
        {
          

            if (assessmentID == null || id_user == null || string.IsNullOrEmpty(_attemptNo))
            {
                return "Invalid parameters!";
            }

            // Fetch user details
            var tbl_profile = getData(1, id_user, assessmentID);
            var tbl_user = getData(2, id_user, assessmentID);
            var created_date = getData(4, id_user, assessmentID);

            // Generate the certificate HTML
            string certificateHtml = GenerateCertificateHtml(
                tbl_profile._userName,
                tbl_profile.region,
                tbl_user.designation,
                created_date.date,
                tbl_user.department,
                orgid
            );

            // Define PDF file path
            string folderPath = @"D:\Certificates\Content\Content\";
            string _fileName = $"{id_user}_{assessmentID}_{_attemptNo}_Certificate.pdf";
            string fullPath = System.IO.Path.Combine(folderPath, _fileName);

            //// Ensure directory exists
            



            // Ensure the output folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
       
            // Convert HTML to PDF
            try
            {
                try
                {
                    using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                    {
                        Console.WriteLine("File stream created successfully.");

                        using (PdfWriter writer = new PdfWriter(stream))
                        {
                            Console.WriteLine("PdfWriter created successfully.");

                            using (PdfDocument pdfDoc = new PdfDocument(writer))
                            {
                                Console.WriteLine("PdfDocument created successfully.");

                                PageSize pageSize = PageSize.A4.Rotate();

                                using (Document document = new Document(pdfDoc, pageSize))
                                {
                                    Console.WriteLine("Document created successfully.");

                                    HtmlConverter.ConvertToPdf(certificateHtml, pdfDoc, new ConverterProperties());
                                    Console.WriteLine("HTML converted to PDF successfully.");
                                   
                                } // Document disposed here
                                int totalPages = pdfDoc.GetNumberOfPages();
                                Console.WriteLine($"Total Pages Before Removal: {totalPages}");

                                // **Remove the last page if more than one page exists**
                                if (totalPages > 1)
                                {
                                    pdfDoc.RemovePage(totalPages);
                                    Console.WriteLine("Last page removed successfully.");
                                }


                                Console.WriteLine("Document closed successfully.");
                            } // PdfDocument disposed here
                        }

                        GC.Collect(); // Ensure resources are fully released
                        GC.WaitForPendingFinalizers();// PdfWriter disposed here
                    }
                }// FileStream disposed here

                catch (Exception ex)
                {
                    Console.WriteLine("Error generating PDF: " + ex.Message);
                    Console.WriteLine("Stack Trace: " + ex.StackTrace);
                }

            
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating PDF: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }


            try
            {
                var _existQuery = "Select *  from tbl_certificate_log where id_user = " + id_user + " and certificateFileName = '" + _fileName + "' and id_assessment = " + assessmentID + " and id_heading = " + headingid;
                var _exist = new UtilityModel().checkCertificateExistOrNot(_existQuery);

                if (!_exist)
                {

                    //// Log the certificate in the database
                    var _updateTable = new Certificate_log()
                    {
                        id_assessment = assessmentID,
                        id_user = id_user,
                        score = Convert.ToDouble(created_date?.scoring_value ?? 0),
                        certificateFileName = _fileName,
                        addedDate = DateTime.Now,
                        attempt_no = Convert.ToInt32(_attemptNo),
                        pdfURL = $"https://www.m2ost.in/certificates/{_fileName}",
                        idheading = headingid
                    };


                    new UtilityModel().addCertificateIntoTable(_updateTable);
                    return "Certificate generated successfully!";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("PDF Generation Error: " + ex.Message);
                return "Error generating PDF!";
            }
            return "Certificate generated successfully!";
        }


        
        public string GenerateCertificateHtml(string _userName, string _region, string designation, string _date, string _department, string orgid)
        {
            // Fetch certificate theme data from database
            UtilityModel certificateModel = new UtilityModel();
            string Select_theme = "2";
            List<CertificateAssignmentTheme> certificateList = certificateModel.GetCertificateDataListThem(Select_theme, orgid);
            string str1 = certificateList[0].BackgroundImageThemeSecond;
            string str2 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/CertificationAssessment/" + str1;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.BackgroundImageThemeSecond = str2;
            }
            string str3 = certificateList[0].LogoImageLeftThemeSecond;
            string str4 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/CertificationAssessment/" + str3;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.LogoImageLeftThemeSecond = str4;
            }
            string str5 = certificateList[0].LogoImageRightThemeSecond;
            string str6 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/CertificationAssessment/" + str5;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.LogoImageRightThemeSecond = str6;
            }

            if (certificateList == null || certificateList.Count == 0)
            {
                return "<h2>Certificate Data Not Found</h2>";
            }

            var certificate = certificateList[0];

            // Generate the HTML
            string htmlContent = $@"
    <!DOCTYPE html>
    <html lang='en'>
    <head>
        <meta charset='UTF-8' />
        <meta name='viewport' content='width=device-width, initial-scale=1.0' />
        <title>Certificate</title>
        <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css'>
        <style>
            body {{
                font-family: 'Times New Roman', Times, serif;
            }}
            .bg-container {{
    background:url('{certificate.BackgroundImageThemeSecond}');
    background-size: contain;
    background-repeat: no-repeat;
    background-position: unset;
    width: 100%;
    height: 11.69in;  /* A4 size - adjust as needed */
    margin: 10px;
}}
.bg-container {{
    page-break-before: always;  /* Ensures that the container starts a new page */
    page-break-after: avoid;    /* Avoids a page break after the container */
}}
            .container3 {{
                text-align: center;
            }}
            .container3-p {{
                font-family: Cambria, Cochin, Georgia, Times, 'Times New Roman', serif;
                font-size: 20px;
            }}
            .fontWidth {{
                font-weight: 600;
                color: red;
                font-size: 20px;
            }}
        </style>
    </head>
    <body class='container'>
        <div class='bg-container'>
            <div class='text-center' style='padding: 20px;'>
                <img src='{certificate.LogoImageLeftThemeSecond}' height='60' width='180' style='float:left;'>
                <img src='{certificate.LogoImageRightThemeSecond}' height='66' width='200' style='float:right; margin-right:120px;'>
            </div>

            <h1 style='text-align:center; color:{certificate.TextColorHeaderThemeSecond}; font-size:80px; font-family: Great Vibes, cursive;'>
                {certificate.HeaderThemeSecond}
            </h1>

            <div style='padding: 0 15%; text-align: justify; color:{certificate.TextColorThemeSecond};'>
                <ol>";

            // Convert SubText into a bullet list
            foreach (var line in certificate.SubTextFirstThemeSecond.Split('\n'))
            {
                htmlContent += $"<li><b>{line}</b></li>";
            }

            htmlContent += @"
                </ol>
            </div>

            <div style='padding: 20px; text-align:center;'>
                <table style='margin: auto;'>
                    <tr>
                        <td><h4>" + certificate.RightNameThemeSecond + $@" <b class='fontWidth'> {_userName}</b></h4></td>
                        <td><h4>" + certificate.LeftDesignationThemeSecond + $@" <b class='fontWidth'>{designation}</b></h4></td>
                    </tr>
                    <tr>
                        <td><h4>" + certificate.RightDepartmentThemeSecond + $@" <b class='fontWidth'>{_department}</b></h4></td>
                        <td><h4>" + certificate.LeftRegionThemeSecond + $@" <b class='fontWidth'>{_region}</b></h4></td>
                    </tr>
                </table>
            </div>

            <div style='text-align:center; font-family: Great Vibes, cursive; font-size: 2.5rem;'>
                {certificate.SubText2ThemeSecond}
            </div>

            <div style='text-align:center; margin-top:20px; font-size: 12px; font-weight: 700;'>
                THIS IS A SYSTEM-GENERATED CERTIFICATE, NO SIGNATURE REQUIRED
            </div>

            <div style='text-align:center; margin-top: 50px; font-size: 1rem; font-weight: 700;'>
                DATE: {DateTime.UtcNow.AddHours(5).AddMinutes(30).ToString("dd/MM/yyyy")}
            </div>
        </div>
    </body>
    </html>";



            return htmlContent;
        }

        public string DownloadeCreateCertificatenormal(int? assessmentID, int? id_user, string _attemptNo, string categoryname, string orgid, int headingid)
        {


            if (assessmentID == null || id_user == null || string.IsNullOrEmpty(_attemptNo))
            {
                return "Invalid parameters!";
            }

            // Fetch user details
            var tbl_profile = getData(1, id_user, assessmentID);

            var tbl_user = getData(2, id_user, assessmentID);

            var assessmentTitle = getData(3, id_user, assessmentID);

            var created_date = getData(4, id_user, assessmentID);

            var _fileName = id_user + "_" + assessmentID + "_" + _attemptNo + "_Certificate.pdf";
            // Generate the certificate HTML
            string certificateHtml = GenerateCertificateHtmlnormal(
               tbl_profile._userName,
                tbl_profile.region,
                tbl_user.designation,
                created_date.date,
                tbl_user.department,
                created_date.scoring_value,
                categoryname,
                orgid
            );

            // Define PDF file path
            string folderPath = @"D:\Certificates\Content\Content\";
            string _fileName1 = $"{id_user}_{assessmentID}_{_attemptNo}_Certificate.pdf";
            string fullPath = System.IO.Path.Combine(folderPath, _fileName1);

            //// Ensure directory exists




            // Ensure the output folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Convert HTML to PDF
            try
            {
                try
                {
                    using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                    {
                        Console.WriteLine("File stream created successfully.");

                        using (PdfWriter writer = new PdfWriter(stream))
                        {
                            Console.WriteLine("PdfWriter created successfully.");

                            using (PdfDocument pdfDoc = new PdfDocument(writer))
                            {
                                Console.WriteLine("PdfDocument created successfully.");

                                PageSize pageSize = PageSize.A4.Rotate();

                                using (Document document = new Document(pdfDoc, pageSize))
                                {
                                    Console.WriteLine("Document created successfully.");

                                    HtmlConverter.ConvertToPdf(certificateHtml, pdfDoc, new ConverterProperties());
                                    Console.WriteLine("HTML converted to PDF successfully.");

                                } // Document disposed here
                                int totalPages = pdfDoc.GetNumberOfPages();
                                Console.WriteLine($"Total Pages Before Removal: {totalPages}");

                                // **Remove the last page if more than one page exists**
                                if (totalPages > 1)
                                {
                                    pdfDoc.RemovePage(totalPages);
                                    Console.WriteLine("Last page removed successfully.");
                                }


                                Console.WriteLine("Document closed successfully.");
                            } // PdfDocument disposed here
                        }

                        GC.Collect(); // Ensure resources are fully released
                        GC.WaitForPendingFinalizers();// PdfWriter disposed here
                    }
                }// FileStream disposed here

                catch (Exception ex)
                {
                    Console.WriteLine("Error generating PDF: " + ex.Message);
                    Console.WriteLine("Stack Trace: " + ex.StackTrace);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("Error generating PDF: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }


            try
            {
                var _existQuery = "Select *  from tbl_certificate_log where id_user = " + id_user + " and certificateFileName = '" + _fileName + "' and id_assessment = " + assessmentID + " and id_heading = " + headingid;
                var _exist = new UtilityModel().checkCertificateExistOrNot(_existQuery);

                if (!_exist)
                {

                    //// Log the certificate in the database
                    var _updateTable = new Certificate_log()
                    {
                        id_assessment = assessmentID,
                        id_user = id_user,
                        score = Convert.ToDouble(created_date?.scoring_value ?? 0),
                        certificateFileName = _fileName,
                        addedDate = DateTime.Now,
                        attempt_no = Convert.ToInt32(_attemptNo),
                        pdfURL = $"https://www.m2ost.in/certificates/{_fileName}",
                        idheading = headingid
                    };


                    new UtilityModel().addCertificateIntoTable(_updateTable);
                    return "Certificate generated successfully!";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("PDF Generation Error: " + ex.Message);
                return "Error generating PDF!";
            }
            return "Certificate generated successfully!";
        }


        public string GenerateCertificateHtmlnormal(string _userName, string _region, string designation, string _date, string _department, double? _score, string _title, string orgid)
        {
            // Fetch certificate theme data from the database
            UtilityModel certificateModel = new UtilityModel();
            string Select_theme = "1";
            List<CertificateAssignmentTheme> certificateList = certificateModel.GetCertificateDataListThem(Select_theme, orgid);

            if (certificateList == null || certificateList.Count == 0)
            {
                return "<h2>Certificate Data Not Found</h2>";
            }

            string str1 = certificateList[0].LogoImageLeftThemeFirst;
            string str2 = ConfigurationManager.AppSettings["SERVERPATH"] + "CATEGORY_IMAGE/CertificationAssessment/" + str1;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.LogoImageLeftThemeFirst = str2;
            }

            string str3 = certificateList[0].LogoImageRightThemeFirst;
            string str4 = ConfigurationManager.AppSettings["SERVERPATH"] + "CATEGORY_IMAGE/CertificationAssessment/" + str3;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.LogoImageRightThemeFirst = str4;
            }

            string str5 = certificateList[0].BackgroundImageThemeFirst;
            string str6 = ConfigurationManager.AppSettings["SERVERPATH"] + "CATEGORY_IMAGE/CertificationAssessment/" + str5;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.BackgroundImageThemeFirst = str6;
            }

            var certificate = certificateList[0];
            string scoreDisplay = _score.HasValue ? _score.Value.ToString() : "N/A";
            // Generate the HTML content using string interpolation
            string htmlContent = $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <title>Certificate</title>
    <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css'>
    <link href='https://fonts.googleapis.com/css2?family=Great+Vibes&family=Lato:wght@700&display=swap' rel='stylesheet'>
    <script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js'></script>
    <style>
        @page {{
            size: A4 landscape; /* Set the page size to A4 landscape */
            margin: 0; /* Adjust margins as needed */
        }}

        body {{
            display: grid;
            place-items: center;
            height: 8.27in; /* A4 landscape height */
            width: 11.69in; /* A4 landscape width */
            margin: 0;
        }}

        .mobile-view {{
            background: url('{certificate.BackgroundImageThemeFirst}');
            background-size: contain;
            background-repeat: no-repeat;
            background-position: unset;
            height: 8.27in; /* A4 landscape height */
            width: 100%; /* Adjust width */
            margin: 10px;
            page-break-before: always; /* Ensure this container starts on a new page */
            page-break-after: avoid; /* Avoid page break after the certificate */
        }}

        * {{
            margin: 0px;
            padding: 0px;
            overflow: hidden;
        }}

        .web-view {{
            display: none;
        }}

        body {{
            display: grid;
            place-items: center;
        }}

        .Batalogo {{
            height: 5vh;
            width: 30vw;
        }}

        .textCenter {{
            text-align: center;
            margin: 0 25% 0 25%;
        }}

        .CertificateHeaderGold {{
            display: flex;
            justify-content: center;
            color: gold;
            font-size: 1.5rem;
            font-weight: 400;
            font-family: 'Great Vibes', cursive;
            margin-top: 5%;
        }}

        .CertificateHeaderRed {{
            display: flex;
            justify-content: center;
            color: red;
            font-size: 1rem;
            font-weight: 700;
        }}

        .CertificateSubHeaderRed {{
            display: flex;
            justify-content: center;
            color: red;
            font-size: 0.7rem;
            font-weight: 700;
        }}

        .content_container {{
            display: flex;
            justify-content: center;
            align-items: center;
            flex-direction: column;
            gap: 0.3em;
        }}

        .userName {{
            display: flex;
            justify-content: center;
            color: #000;
            font-size: 1.1rem;
            font-weight: 600;
            border-bottom: 1px solid #000;
            width: 70%;
        }}

        .Paragraph_scored {{
            display: flex;
            justify-content: center;
            color: #000;
            font-size: 1rem;
            font-weight: 400;
            font-family: 'Great Vibes', cursive;
        }}

        .Score_description {{
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            color: #000;
            font-size: 1.5rem;
            font-weight: 400;
            font-family: 'Great Vibes', cursive;
        }}

        .percentage {{
            display: flex;
            justify-content: center;
            color: #000;
            font-size: 1rem;
            font-weight: 400;
            font-family: sans-serif;
            border-bottom: 1px solid #000;
            width: 15vw;
        }}

        .training_name {{
            display: flex;
            justify-content: center;
            color: #000;
            font-size: 1rem;
            font-weight: 700;
            border-bottom: 1px solid #000;
            font-family: sans-serif;
            width: 100%;
        }}

        .date {{
            display: flex;
            justify-content: center;
            align-items: end;
            color: #000;
            font-size: 0.6rem;
            font-weight: 600;
            font-family: sans-serif;
            margin-top: 2vh;
            width: 100%;
        }}

        .conducted_name {{
            font-size: 20px !important;
        }}

        .logo {{
            max-width: 20%;
            margin-top: 20px;
        }}
    </style>
</head>
<body>
    <div class='mobile-view'>
        <div class='row' style='margin-top:10%; margin-left:3%;'>
            <div>
                <div class='Batalogo' style='margin-left:20px;margin-top:5%;max-width:40%;'>
                    <img src='{certificate.LogoImageLeftThemeFirst}' style='max-width:50%;margin-top:10%;' alt=''>
                </div>
                <div class='Batalogo' style='float: right; margin-top: -5%; margin-right: 10%;'>
                    <img src='{certificate.LogoImageRightThemeFirst}' style='max-width:100%;' alt=''>
                </div>
            </div>
        </div>

        <div class='textCenter'>
            <div class='CertificateHeaderGold'>
                <span style='color:{certificate.TextColorHeaderThemeFirst}; font-size: 80px; font-weight: 400; font-family: 'Great Vibes', cursive; margin-top: 5%;'>
                    {certificate.HeaderThemeFirst}
                </span>
            </div>

            <div class='CertificateHeaderRed'>
                <span style='color:{certificate.TextColorSubTextThemeFirst}; font-size: 30px; font-weight: 700;'>
                    {certificate.SubText1ThemeFirst}
                </span>
            </div>

            <div class='CertificateSubHeaderRed'>
                <span style='color:{certificate.TextColorSubTextThemeFirst}; font-size: 20px; font-weight: 700;'>
                    {certificate.SubText2ThemeFirst}
                </span>
            </div>

            <div class='content_container'>
                <div class='userName'>
                    {_userName}
                </div>
                
                <div class='Score_description'>
                    <p class='Paragraph_scored'>
                        Scored {scoreDisplay}% and has successfully completed
                    </p>
                    <p class='training_name'>{_title}</p>
                    <p class='conducted_name'>{certificate.SubText3ThemeFirst}</p>
                </div>

                <div class='date'>
                    DATE: {DateTime.UtcNow.AddHours(5).AddMinutes(30).ToString("dd/MM/yyyy")}
                </div>
            </div>
        </div>
    </div>
</body>
</html>";



            return htmlContent;
        }






        public ActionResult Index(string _userName, string _region, string designation, string _date, string _department ,string orgid)
        {

           

            UtilityModel certificateModel = new UtilityModel();
            string Select_theme = "2";
            List<CertificateAssignmentTheme> certificateList = certificateModel.GetCertificateDataListThem(Select_theme, orgid);
            string str1 = certificateList[0].BackgroundImageThemeSecond;
            string str2 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/CertificationAssessment/" + str1;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.BackgroundImageThemeSecond = str2;
            }
            string str3 = certificateList[0].LogoImageLeftThemeSecond;
            string str4 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/CertificationAssessment/" + str3;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.LogoImageLeftThemeSecond = str4;
            }
            string str5 = certificateList[0].LogoImageRightThemeSecond;
            string str6 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/CertificationAssessment/" + str5;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.LogoImageRightThemeSecond = str6;
            }
            ViewData["certificateList"] = certificateList;

            TempData["_userName"] = _userName;
            TempData["region"] = _region;
            TempData["designation"] = designation;
            TempData["date"] = _date;
            TempData["department"] = _department;

            return View();

        }       

        public ActionResult normal_certificate(string _userName, string _region, string designation, string _date, string _department, double? _score, string _title,string orgid)
        {
            
            //

            UtilityModel certificateModel = new UtilityModel();
            string Select_theme = "1";
            List<CertificateAssignmentTheme> certificateList = certificateModel.GetCertificateDataListThem(Select_theme, orgid);
            string str1 = certificateList[0].LogoImageLeftThemeFirst;
            string str2 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/CertificationAssessment/" + str1;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.LogoImageLeftThemeFirst = str2;
            }
            string str3 = certificateList[0].LogoImageRightThemeFirst;
            string str4 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/CertificationAssessment/" + str3;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.LogoImageRightThemeFirst = str4;
            }
            string str5 = certificateList[0].BackgroundImageThemeFirst;
            string str6 = ConfigurationManager.AppSettings["SERVERPATH"].ToString() + "CATEGORY_IMAGE/CertificationAssessment/" + str5;
            foreach (var certifacateItem in certificateList)
            {
                certifacateItem.BackgroundImageThemeFirst = str6;
            }
            ViewData["certificateList"] = certificateList;

            TempData["_userName"] = _userName;
            TempData["score"] = _score;
            TempData["title"] = _title;

            return View();

        }

        [Obsolete]
        [Route("download/certificate/download")]
        public ActionResult certificate(int? assessmentID, int? id_user, string _attemptNo,string categoryname,string orgid, int headingid)
        {
            var tbl_profile = getData(1, id_user, assessmentID);

            var tbl_user = getData(2, id_user, assessmentID);

            var assessmentTitle = getData(3, id_user, assessmentID);

            var created_date = getData(4, id_user, assessmentID);

            var _fileName = id_user + "_" + assessmentID + "_" + _attemptNo + "_Certificate.pdf";


            //string path = System.Web.HttpContext.Current.Server.MapPath("~/Content/Certificates/");
            string path = @"D:\\Certificates\\Content\\";


            var _existQuery = "Select *  from tbl_certificate_log where id_user = " + id_user + " and certificateFileName = '" + _fileName + "' and id_assessment = " + assessmentID + " and id_heading = " + headingid;
            var _exist = new UtilityModel().checkCertificateExistOrNot(_existQuery);




            //var _pdf = new ActionAsPdf("COE", new { _userName = tbl_profile._userName, _region = tbl_profile.region, designation = tbl_profile.designation, _date = created_date.date, _department = tbl_user.department, _score = created_date.scoring_value, _title = assessmentTitle.assessment_title }) { FileName = _fileName };
            var _pdf = new ActionAsPdf("normal_certificate", new { _userName = tbl_profile._userName, _region = tbl_profile.region, designation = tbl_user.designation, _date = created_date.date, _department = tbl_user.department, _score = created_date.scoring_value, _title = categoryname , orgid })
            {
                FileName = _fileName,
                PageOrientation = Rotativa.Options.Orientation.Landscape,
                PageSize = Rotativa.Options.Size.A4
            };

            if (!_exist)
            {

                var byteArray = _pdf.BuildPdf(ControllerContext);
                var fileStream = new FileStream(path + _fileName, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();

                var _updateTable = new Certificate_log()
                {
                    id_assessment = assessmentID,
                    id_user = id_user,
                    score = Convert.ToDouble(created_date.scoring_value),
                    certificateFileName = _fileName,
                    addedDate = DateTime.Now,
                    attempt_no = Convert.ToInt32(_attemptNo),
                    pdfURL = @"https://www.m2ost.in/certificates/" + _fileName,
                    idheading = headingid
                };

                new UtilityModel().addCertificateIntoTable(_updateTable);
            }

            return _pdf;
        }


        public UserDataForCertificate getData(int _caseCount, int? id_user, int? assessmentID)
        {
            UserDataForCertificate userDataFor = new UserDataForCertificate();
            switch (_caseCount)
            {
                case 1:
                    string sql1 = "SELECT firstname, lastname, OFFICE_ADDRESS as region FROM tbl_profile WHERE id_user = " + id_user;
                    userDataFor = new UtilityModel().getUserCertificateData(sql1, _caseCount);
                    break;

                case 2:
                    string sql2 = "Select user_designation , user_department from tbl_user where id_user= " + id_user;
                    userDataFor = new UtilityModel().getUserCertificateData(sql2, 2);
                    break;

                case 3:
                    string sql3 = "Select assessment_title from tbl_assessment where id_assessment =" + assessmentID;
                    userDataFor = new UtilityModel().getUserCertificateData(sql3, _caseCount);
                    break;

                case 4:
                    string sql4 = "Select *  from tbl_user_kpi_data_log where id_user = " + id_user + " and Content_Assessment_ID =" + assessmentID + " and KPI_Name like '%Mastery Score%'  order by scoring_value DESC limit 1;";
                    userDataFor = new UtilityModel().getUserCertificateData(sql4, 4);

                    if (string.IsNullOrEmpty(userDataFor.date))
                    {
                        sql4 = string.Empty;
                        sql4 = "Select (result_in_percentage) scoring_value, (attempt_number) AttemptNo, (updated_date_time) created_date  from tbl_rs_type_qna where id_user = " + id_user + " and id_assessment =" + assessmentID + " and attempt_number >= 1  order by  result_in_percentage DESC limit 1;";
                        userDataFor = new UtilityModel().getUserCertificateData(sql4, 4);
                    }

                    break;
            }
            return userDataFor;
        }

    }
}