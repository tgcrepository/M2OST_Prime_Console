// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_assessment_answer
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_assessment_answer
  {
    public tbl_assessment_answer() => this.tbl_assessment_header = (ICollection<IBHFL.tbl_assessment_header>) new HashSet<IBHFL.tbl_assessment_header>();

    public int id_assessment_answer { get; set; }

    public int? id_assessment { get; set; }

    public int? id_assessment_question { get; set; }

    public int? id_assessment_scoring_key { get; set; }

    public string answer_description { get; set; }

    public int? position { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public virtual ICollection<IBHFL.tbl_assessment_header> tbl_assessment_header { get; set; }
  }
}
