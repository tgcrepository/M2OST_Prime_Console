// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_category
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_category
  {
    public tbl_category()
    {
      this.tbl_category_associantion = (ICollection<IBHFL.tbl_category_associantion>) new HashSet<IBHFL.tbl_category_associantion>();
      this.tbl_content_organization_mapping = (ICollection<IBHFL.tbl_content_organization_mapping>) new HashSet<IBHFL.tbl_content_organization_mapping>();
    }

    public int ID_CATEGORY { get; set; }

    public int ID_ORGANIZATION { get; set; }

    public string CATEGORYNAME { get; set; }

    public string DESCRIPTION { get; set; }

    public string IMAGE_PATH { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATED_DATE_TIME { get; set; }

    public string SUB_HEADING { get; set; }

    public int? ORDERID { get; set; }

    public int? IS_PRIMARY { get; set; }

    public int? ID_PARENT { get; set; }

    public int? SEARCH_MAX_COUNT { get; set; }

    public int? COUNT_REQUIRED { get; set; }

    public int? CATEGORY_TYPE { get; set; }

    public string IMAGE_URL { get; set; }

    public virtual ICollection<IBHFL.tbl_category_associantion> tbl_category_associantion { get; set; }

    public virtual ICollection<IBHFL.tbl_content_organization_mapping> tbl_content_organization_mapping { get; set; }

    public virtual tbl_organization tbl_organization { get; set; }
  }
}
