// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_assessment_header
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_assessment_header
  {
    public int id_assessment_header { get; set; }

    public int id_assessment_scoring_key { get; set; }

    public int id_assessment_question { get; set; }

    public int? id_assessment_answer { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public virtual tbl_assessment_answer tbl_assessment_answer { get; set; }

    public virtual tbl_assessment_scoring_key tbl_assessment_scoring_key { get; set; }

    public virtual tbl_assessment_question tbl_assessment_question { get; set; }
  }
}
