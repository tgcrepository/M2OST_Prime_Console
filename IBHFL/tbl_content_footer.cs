// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_content_footer
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_content_footer
  {
    public tbl_content_footer() => this.tbl_content_header_footer = (ICollection<IBHFL.tbl_content_header_footer>) new HashSet<IBHFL.tbl_content_header_footer>();

    public int id_content_footer { get; set; }

    public int id_organization { get; set; }

    public string content_footer_name { get; set; }

    public string content_footer_image { get; set; }

    public string content_footer_action_url { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public virtual ICollection<IBHFL.tbl_content_header_footer> tbl_content_header_footer { get; set; }
  }
}
