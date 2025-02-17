// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_content_program_mapping
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_content_program_mapping
  {
    public int id_content_program_mapping { get; set; }

    public int? id_organization { get; set; }

    public int? id_category { get; set; }

    public int? id_assessment_sheet { get; set; }

    public int? id_user { get; set; }

    public int? id_role { get; set; }

    public int? map_type { get; set; }

    public int? option_type { get; set; }

    public DateTime? start_date { get; set; }

    public DateTime? expiry_date { get; set; }

    public int? id_category_tile { get; set; }

    public int? id_category_heading { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
