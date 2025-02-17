// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_kpi_master
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_kpi_master
  {
    public int id_kpi_master { get; set; }

    public int? id_organization { get; set; }

    public string kpi_name { get; set; }

    public string kpi_description { get; set; }

    public int? kpi_type { get; set; }

    public string KPIID { get; set; }

    public DateTime? start_date { get; set; }

    public DateTime? expiry_date { get; set; }

    public int? id_creator { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
