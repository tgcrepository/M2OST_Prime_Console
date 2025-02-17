// Decompiled with JetBrains decompiler
// Type: IBHFL.sc_report_game_process_path
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class sc_report_game_process_path
  {
    public int id_sc_report_game_process_path { get; set; }

    public int? id_game { get; set; }

    public int? id_organization { get; set; }

    public int? process_type { get; set; }

    public int? sequence_number { get; set; }

    public double? score { get; set; }

    public double? element_weightage { get; set; }

    public int? element_type { get; set; }

    public int? id_category { get; set; }

    public int? id_assessment { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
