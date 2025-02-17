// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_banner
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_banner
  {
    public tbl_banner() => this.tbl_content_banner = (ICollection<IBHFL.tbl_content_banner>) new HashSet<IBHFL.tbl_content_banner>();

    public int id_banner { get; set; }

    public int id_organization { get; set; }

    public string banner_name { get; set; }

    public string banner_image { get; set; }

    public string banner_action_url { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public virtual ICollection<IBHFL.tbl_content_banner> tbl_content_banner { get; set; }

    public virtual tbl_organization tbl_organization { get; set; }
  }
}
