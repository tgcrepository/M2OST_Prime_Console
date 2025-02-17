// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_organisation_banner
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_organisation_banner
  {
    public int id_organisation_banner { get; set; }

    public string banner_name { get; set; }

    public string Banner_path { get; set; }

    public DateTime? updated_date_time { get; set; }

    public int? id_organisation { get; set; }

    public virtual tbl_organization tbl_organization { get; set; }
  }
}
