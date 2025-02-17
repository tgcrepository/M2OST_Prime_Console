// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_assignment_role_program
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_assignment_role_program
  {
    public int id_assignment_role_program { get; set; }

    public int? id_organization { get; set; }

    public int? id_role { get; set; }

    public int? id_program { get; set; }

    public DateTime? start_datetime { get; set; }

    public DateTime? end_datetime { get; set; }

    public int? id_category_tile { get; set; }

    public int? id_category_heading { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
