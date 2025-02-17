// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_assessment_sheet
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_assessment_sheet
  {
    public tbl_assessment_sheet() => this.tbl_assessment_general = (ICollection<IBHFL.tbl_assessment_general>) new HashSet<IBHFL.tbl_assessment_general>();

    public int id_assessment_sheet { get; set; }

    public int? id_organization { get; set; }

    public int id_assesment { get; set; }

    public int? id_assessment_theme { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public virtual tbl_assessment tbl_assessment { get; set; }

    public virtual ICollection<IBHFL.tbl_assessment_general> tbl_assessment_general { get; set; }

    public virtual tbl_organization tbl_organization { get; set; }
  }
}
