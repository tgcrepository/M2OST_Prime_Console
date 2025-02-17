// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_csst_role
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_csst_role
  {
    public int id_csst_role { get; set; }

    public string csst_role { get; set; }

    public int? id_organization { get; set; }

    public string status { get; set; }

    public DateTime? updated_dated_time { get; set; }

    public virtual tbl_organization tbl_organization { get; set; }
  }
}
