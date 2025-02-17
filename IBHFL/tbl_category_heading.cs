// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_category_heading
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_category_heading
  {
    public tbl_category_heading() => this.tbl_category_associantion = (ICollection<IBHFL.tbl_category_associantion>) new HashSet<IBHFL.tbl_category_associantion>();

    public int id_category_heading { get; set; }

    public string Heading_title { get; set; }

    public int? id_category_tiles { get; set; }

    public int? heading_order { get; set; }

    public string status { get; set; }
        public int id_category { get; set; }

    public DateTime? updated_date_time { get; set; }

    public virtual ICollection<IBHFL.tbl_category_associantion> tbl_category_associantion { get; set; }

    public virtual tbl_category_tiles tbl_category_tiles { get; set; }
  }
}
