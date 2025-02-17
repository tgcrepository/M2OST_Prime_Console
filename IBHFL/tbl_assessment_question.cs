// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_assessment_question
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_assessment_question
  {
    public tbl_assessment_question() => this.tbl_assessment_header = (ICollection<IBHFL.tbl_assessment_header>) new HashSet<IBHFL.tbl_assessment_header>();

    public int id_assessment_question { get; set; }

    public int? id_organization { get; set; }

    public int? id_assessment { get; set; }

    public int? id_assessment_scoring_key { get; set; }

    public string assessment_question { get; set; }

    public string question_image { get; set; }

    public string aq_answer { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public virtual ICollection<IBHFL.tbl_assessment_header> tbl_assessment_header { get; set; }
  }
}
