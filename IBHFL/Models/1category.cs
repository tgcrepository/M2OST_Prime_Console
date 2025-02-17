// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.category
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml;

namespace IBHFL.Models
{
  public class category
  {
    public List<details> category_service_call(int orgtid, int uid)
    {
      try
      {
        List<details> detailsList = new List<details>();
        XmlDocument xmlDocument = new XmlDocument();
        string address = APIString.API + "Category?orgID=" + (object) orgtid + "&uid=" + (object) uid;
        new JavaScriptSerializer().Deserialize<List<RootObject>>(new WebClient().DownloadString(address));
        using (WebClient webClient = new WebClient())
        {
          JArray source = JArray.Parse(webClient.DownloadString(address));
          int num1 = source.Count<JToken>();
          int num2 = 1;
          foreach (JToken jtoken in source)
          {
            if (num2 <= num1)
            {
              detailsList.Add(new details()
              {
                CategoryImagePath = (string) jtoken[(object) "CategoryDescription"],
                CategoryHeader = (object) (string) jtoken[(object) "CategoryHeader"],
                CategoryID = (int) jtoken[(object) "CategoryID"],
                image = (string) jtoken[(object) "CategoryImagePath"],
                CategoryName = (string) jtoken[(object) "CategoryName"],
                Is_Primary = (int) jtoken[(object) "Is_Primary"],
                ORDERID = (int) jtoken[(object) "ORDERID"],
                OrganisationId = (int) jtoken[(object) "OrganisationId"],
                SubCount = (int) jtoken[(object) "SubCount"],
                NEXTAPI = (string) jtoken[(object) "NEXTAPI"]
              });
              ++num2;
            }
          }
          return detailsList;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public List<details> category_description(string api, int c_id)
    {
      try
      {
        List<details> detailsList = new List<details>();
        XmlDocument xmlDocument = new XmlDocument();
        string str = "";
        string address = APIString.API + "&cid=" + (object) c_id + "&uid=204";
        WebClient webClient1 = new WebClient();
        using (WebClient webClient2 = new WebClient())
        {
          str = webClient2.DownloadString(address);
          JArray.Parse(str).Count<JToken>();
        }
        foreach (RootObject rootObject in new JavaScriptSerializer().Deserialize<List<RootObject>>(str))
        {
          int index = 0;
          foreach (Category123 category in rootObject.Categories)
          {
            detailsList.Add(new details()
            {
              CategoryID = rootObject.Categories[index].CategoryID,
              CategoryHeader = (object) rootObject.Categories[index].CategoryHeader,
              CategoryImagePath = rootObject.Categories[index].CategoryImagePath,
              CategoryName = rootObject.Categories[index].CategoryName,
              CategoryDescription = rootObject.Categories[index].CategoryDescription
            });
            ++index;
          }
        }
        return detailsList;
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public List<RootObject> LeaerningAssessment(int cid, int uid, int oid)
    {
      try
      {
        List<RootObject> rootObjectList = new List<RootObject>();
        XmlDocument xmlDocument = new XmlDocument();
        string address = APIString.API + "getLearningAssessment?CID=" + (object) cid + "&uid=oid=" + (object) oid;
        using (WebClient webClient = new WebClient())
        {
          JArray source = JArray.Parse(webClient.DownloadString(address));
          source.Count<JToken>();
          foreach (JToken jtoken in source)
            ;
          return rootObjectList;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public List<ColorApi> GetColorApi1(int oid)
    {
      try
      {
        List<ColorApi> colorApi1 = new List<ColorApi>();
        XmlDocument xmlDocument = new XmlDocument();
        string address = APIString.API + "getColorConfig?orgID=" + (object) oid;
        using (WebClient webClient = new WebClient())
        {
          webClient.DownloadString(address);
          List<ColorApi> colorApiList = new List<ColorApi>();
          colorApiList = JsonConvert.DeserializeObject<List<ColorApi>>(address);
          return colorApi1;
        }
      }
      catch (Exception ex)
      {
        throw ex;
      }
    }

    public string GetColorApi(string api)
    {
      byte[] bytes = (byte[]) null;
      WebClient webClient1 = new WebClient();
      using (WebClient webClient2 = new WebClient())
        bytes = webClient2.DownloadData(api);
      return Encoding.UTF8.GetString(bytes);
    }

    public string Email(mailmod admin, int uid, int oid, int cid)
    {
      string str1 = "Message Sent Successfully..!!";
      string str2 = " paathshala-learningtech@paathshala.biz";
      string to = "paathshala-learningtech@paathshala.biz";
      string subject = admin.sub + " /User id - " + (object) uid + "/Org id - " + (object) oid + " /Content Id - " + (object) cid;
      try
      {
        new SmtpClient()
        {
          Host = "smtp.gmail.com",
          Port = 587,
          EnableSsl = true,
          DeliveryMethod = SmtpDeliveryMethod.Network,
          Credentials = ((ICredentialsByHost) new NetworkCredential(str2, "Pls@210312")),
          Timeout = 30000
        }.Send(new MailMessage(str2, to, subject, admin.msg));
      }
      catch (Exception ex)
      {
        str1 = "Error sending email.!!!";
      }
      return str1;
    }
  }
}
