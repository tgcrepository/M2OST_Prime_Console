// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_report_login_log
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_report_login_log
  {
    public int id_report_login_log { get; set; }

    public int? id_user { get; set; }

    public int? id_organization { get; set; }

    public DateTime? LOG_DATETIME { get; set; }

    public string IMEI { get; set; }
  }
}
