// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_survey
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_survey
  {
    public tbl_survey()
    {
      this.tbl_survey_bank_link = (ICollection<IBHFL.tbl_survey_bank_link>) new HashSet<IBHFL.tbl_survey_bank_link>();
      this.tbl_survey_data = (ICollection<IBHFL.tbl_survey_data>) new HashSet<IBHFL.tbl_survey_data>();
    }

    public int ID_SURVEY { get; set; }

    public int? ID_CONTENT_ANSWER { get; set; }

    public int? ID_ANSWER_STEP { get; set; }

    public string SURVEY_NAME { get; set; }

    public string DESCRIPTION { get; set; }

    public string SURVEY_IMAGE { get; set; }

    public DateTime START_DATE { get; set; }

    public DateTime END_DATE { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATED_DATE_TIME { get; set; }

    public virtual tbl_content_answer tbl_content_answer { get; set; }

    public virtual tbl_content_answer_steps tbl_content_answer_steps { get; set; }

    public virtual ICollection<IBHFL.tbl_survey_bank_link> tbl_survey_bank_link { get; set; }

    public virtual ICollection<IBHFL.tbl_survey_data> tbl_survey_data { get; set; }
  }
}
