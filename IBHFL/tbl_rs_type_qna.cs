// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_rs_type_qna
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_rs_type_qna
  {
  
    public int? id_assessment_log { get; set; }

    public int? id_user { get; set; }

    public int? id_organization { get; set; }

    public int? id_assessment_sheet { get; set; }

    public int? id_assessment { get; set; }

    public int? attempt_number { get; set; }

    public int? total_question { get; set; }

    public int? right_answer_count { get; set; }

    public int? wrong_answer_count { get; set; }

    public double? result_in_percentage { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
