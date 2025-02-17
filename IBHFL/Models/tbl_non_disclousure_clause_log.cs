// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.tbl_non_disclousure_clause_log
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL.Models
{
  public class tbl_non_disclousure_clause_log
  {
    public int id_clause_log { get; set; }

    public int id_user { get; set; }

    public int id_org { get; set; }

    public string log_status { get; set; }

    public DateTime updated_date_time { get; set; }
  }
}
