// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_content_answer_steps
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_content_answer_steps
  {
    public tbl_content_answer_steps() => this.tbl_survey = (ICollection<IBHFL.tbl_survey>) new HashSet<IBHFL.tbl_survey>();

    public int ID_ANSWER_STEP { get; set; }

    public int ID_CONTENT_ANSWER { get; set; }

    public int STEPNO { get; set; }

    public string ANSWER_STEPS_PART1 { get; set; }

    public string ANSWER_STEPS_PART2 { get; set; }

    public string ANSWER_STEPS_IMG1 { get; set; }

    public string ANSWER_STEPS_IMG2 { get; set; }

    public string ANSWER_STEPS_BANNER { get; set; }

    public string REDIRECTION_URL { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATEDDATETIME { get; set; }

    public int? ID_THEME { get; set; }

    public string ANSWER_STEPS_PART3 { get; set; }

    public string ANSWER_STEPS_IMG3 { get; set; }

    public string ANSWER_STEPS_IMG10 { get; set; }

    public string ANSWER_STEPS_IMG5 { get; set; }

    public string ANSWER_STEPS_IMG6 { get; set; }

    public string ANSWER_STEPS_IMG7 { get; set; }

    public string ANSWER_STEPS_IMG8 { get; set; }

    public string ANSWER_STEPS_IMG9 { get; set; }

    public string ANSWER_STEPS_PART10 { get; set; }

    public string ANSWER_STEPS_PART4 { get; set; }

    public string ANSWER_STEPS_PART5 { get; set; }

    public string ANSWER_STEPS_PART6 { get; set; }

    public string ANSWER_STEPS_PART7 { get; set; }

    public string ANSWER_STEPS_PART8 { get; set; }

    public string ANSWER_STEPS_PART9 { get; set; }

    public string ANSWER_STEPS_IMG4 { get; set; }

    public virtual tbl_content_answer tbl_content_answer { get; set; }

    public virtual ICollection<IBHFL.tbl_survey> tbl_survey { get; set; }
  }
}
