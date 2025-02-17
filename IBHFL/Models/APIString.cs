// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.APIString
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System.Configuration;

namespace IBHFL.Models
{
  public static class APIString
  {
    public static string API = ConfigurationManager.AppSettings["api_full"].ToString();
    public static string RAW = ConfigurationManager.AppSettings["api_raw"].ToString();
  }
}
