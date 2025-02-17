// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_report_content
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_report_content
  {
    public int ID_REPORT_CONTENT { get; set; }

    public int ID_ORGANIZATION { get; set; }

    public int ID_CONTENT { get; set; }

    public int ID_USER { get; set; }

    public int? CHOICE { get; set; }

    public string STATUS { get; set; }

    public DateTime? UPDATED_DATE_TIME { get; set; }
  }
}
