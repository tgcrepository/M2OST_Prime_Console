// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_content
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_content
  {
    public tbl_content()
    {
      this.tbl_content_banner = (ICollection<IBHFL.tbl_content_banner>) new HashSet<IBHFL.tbl_content_banner>();
      this.tbl_content_header_footer = (ICollection<IBHFL.tbl_content_header_footer>) new HashSet<IBHFL.tbl_content_header_footer>();
      this.tbl_content_right_association = (ICollection<IBHFL.tbl_content_right_association>) new HashSet<IBHFL.tbl_content_right_association>();
      this.tbl_content_organization_mapping = (ICollection<IBHFL.tbl_content_organization_mapping>) new HashSet<IBHFL.tbl_content_organization_mapping>();
      this.tbl_subscriptions = (ICollection<IBHFL.tbl_subscriptions>) new HashSet<IBHFL.tbl_subscriptions>();
    }

    public int ID_CONTENT { get; set; }

    public int ID_THEME { get; set; }

    public int ID_USER { get; set; }

    public int ID_CONTENT_LEVEL { get; set; }

    public string CONTENT_TITLE { get; set; }

    public string CONTENT_HEADER { get; set; }

    public string CONTENT_QUESTION { get; set; }

    public int CONTENT_COUNTER { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATED_DATE_TIME { get; set; }

    public int LINK_COUNT { get; set; }

    public int IS_PRIMARY { get; set; }

    public DateTime? EXPIRY_DATE { get; set; }

    public string COMMENT { get; set; }

    public string CONTENT_IDENTIFIER { get; set; }

    public int CONTENT_OWNER { get; set; }

    public virtual ICollection<IBHFL.tbl_content_banner> tbl_content_banner { get; set; }

    public virtual ICollection<IBHFL.tbl_content_header_footer> tbl_content_header_footer { get; set; }

    public virtual ICollection<IBHFL.tbl_content_right_association> tbl_content_right_association { get; set; }

    public virtual ICollection<IBHFL.tbl_content_organization_mapping> tbl_content_organization_mapping { get; set; }

    public virtual ICollection<IBHFL.tbl_subscriptions> tbl_subscriptions { get; set; }
  }
}
