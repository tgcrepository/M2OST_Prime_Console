﻿// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_kpi_program_scoring
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_kpi_program_scoring
  {
    public int id_kpi_program_scoring { get; set; }

    public int? id_organization { get; set; }

    public int? kpi_type { get; set; }

    public int? id_category { get; set; }

    public int? id_assessment { get; set; }

    public int? id_kpi_master { get; set; }

    public double? ps_weightage { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
