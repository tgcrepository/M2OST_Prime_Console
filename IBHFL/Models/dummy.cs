// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.dummy
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Net;
using System.Xml;
using System.Xml.Linq;

namespace IBHFL.Models
{
  public class dummy
  {
    public string Page_Load()
    {
      string str1 = (string) null;
      XmlDocument xmlDocument1 = new XmlDocument();
      try
      {
        string address = APIString.API + "DisplayCategory?orgID=16&cid=14&uid=103";
        string str2;
        using (WebClient webClient = new WebClient())
          str2 = webClient.DownloadString(address);
        XmlDocument xmlDocument2 = new XmlDocument();
        XDocument.Parse(str2);
        xmlDocument2.Load(str2);
      }
      catch (Exception ex)
      {
        throw ex;
      }
      return str1;
    }
  }
}
