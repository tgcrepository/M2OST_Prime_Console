// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_category_associantion
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_category_associantion
  {
    public int id_category_association { get; set; }

    public int? id_category_tile { get; set; }

    public int? id_category_heading { get; set; }

    public int? id_category { get; set; }

    public int? category_order { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public virtual tbl_category tbl_category { get; set; }

    public virtual tbl_category_tiles tbl_category_tiles { get; set; }

    public virtual tbl_category_heading tbl_category_heading { get; set; }
  }
}
