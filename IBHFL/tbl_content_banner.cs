// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_content_banner
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_content_banner
  {
    public int id_content_banner { get; set; }

    public int id_content { get; set; }

    public int id_banner { get; set; }

    public int id_organization { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public DateTime? date_assigned { get; set; }

    public DateTime? date_removed { get; set; }

    public virtual tbl_banner tbl_banner { get; set; }

    public virtual tbl_content tbl_content { get; set; }
  }
}
