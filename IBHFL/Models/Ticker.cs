// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.Ticker
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL.Models
{
  public class Ticker
  {
    public int Id_ticker { get; set; }

    public int Id_org { get; set; }

    public int Id_creator { get; set; }

    public string status { get; set; }

    public DateTime update_date { get; set; }

    public DateTime expiry_date { get; set; }

    public string ticker_news { get; set; }

    public string background_color { get; set; }

    public string font_color { get; set; }
  }
}
