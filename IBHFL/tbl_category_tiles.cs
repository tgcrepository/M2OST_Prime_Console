// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_category_tiles
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_category_tiles
  {
    public tbl_category_tiles()
    {
      this.tbl_category_associantion = (ICollection<IBHFL.tbl_category_associantion>) new HashSet<IBHFL.tbl_category_associantion>();
      this.tbl_category_heading = (ICollection<IBHFL.tbl_category_heading>) new HashSet<IBHFL.tbl_category_heading>();
    }

    public int id_category_tiles { get; set; }

    public string tile_heading { get; set; }

    public int? category_theme { get; set; }

    public int? id_organization { get; set; }

    public string tile_image { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public int? category_order { get; set; }

    public string image_url { get; set; }

    public virtual ICollection<IBHFL.tbl_category_associantion> tbl_category_associantion { get; set; }

    public virtual ICollection<IBHFL.tbl_category_heading> tbl_category_heading { get; set; }

    public virtual tbl_organization tbl_organization { get; set; }
  }
}
