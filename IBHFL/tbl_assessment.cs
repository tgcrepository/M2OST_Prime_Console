// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_assessment
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_assessment
  {
    public tbl_assessment() => this.tbl_assessment_sheet = (ICollection<IBHFL.tbl_assessment_sheet>) new HashSet<IBHFL.tbl_assessment_sheet>();

    public int id_assessment { get; set; }

    public string assessment_title { get; set; }

    public string assesment_description { get; set; }

    public int? id_organization { get; set; }

    public int? assessment_type { get; set; }

    public DateTime? assess_created { get; set; }

    public DateTime? assess_start { get; set; }

    public DateTime? assess_ended { get; set; }

    public string assess_type { get; set; }

    public int? assess_group { get; set; }

    public string lower_title { get; set; }

    public string high_title { get; set; }

    public string lower_value { get; set; }

    public string high_value { get; set; }

    public int? total_attempt { get; set; }

    public int? ans_requiered { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public string answer_description { get; set; }

    public virtual ICollection<IBHFL.tbl_assessment_sheet> tbl_assessment_sheet { get; set; }

    public virtual tbl_organization tbl_organization { get; set; }
  }
}
