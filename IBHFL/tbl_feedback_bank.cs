// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_feedback_bank
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_feedback_bank
  {
    public tbl_feedback_bank()
    {
      this.tbl_feedback_bank_link = (ICollection<IBHFL.tbl_feedback_bank_link>) new HashSet<IBHFL.tbl_feedback_bank_link>();
      this.tbl_feedback_data = (ICollection<IBHFL.tbl_feedback_data>) new HashSet<IBHFL.tbl_feedback_data>();
    }

    public int ID_FEEDBACK_BANK { get; set; }

    public string FEEDBACK_NAME { get; set; }

    public int? id_organization { get; set; }

    public string FEEDBACK_QUESTION { get; set; }

    public string FEEDBACK_CHOICES { get; set; }

    public string FEEDBACK_IMAGE { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATED_DATE_TIME { get; set; }

    public virtual ICollection<IBHFL.tbl_feedback_bank_link> tbl_feedback_bank_link { get; set; }

    public virtual ICollection<IBHFL.tbl_feedback_data> tbl_feedback_data { get; set; }
  }
}
