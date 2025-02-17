// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_industry
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_industry
  {
    public tbl_industry() => this.tbl_organization = (ICollection<IBHFL.tbl_organization>) new HashSet<IBHFL.tbl_organization>();

    public int ID_INDUSTRY { get; set; }

    public string INDUSTRYNAME { get; set; }

    public string DESCRIPTION { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATED_DATE_TIME { get; set; }

    public virtual ICollection<IBHFL.tbl_organization> tbl_organization { get; set; }
  }
}
