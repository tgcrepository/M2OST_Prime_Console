// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_kpi_grid
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_kpi_grid
  {
    public int id_kpi_grid { get; set; }

    public int? id_kpi_master { get; set; }

    public double? start_range { get; set; }

    public double? end_range { get; set; }

    public int? kpi_value { get; set; }

    public string kpi_text { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
