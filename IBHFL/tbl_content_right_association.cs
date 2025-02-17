// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_content_right_association
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_content_right_association
  {
    public int id_content_right_association { get; set; }

    public int id_content { get; set; }

    public int id_organization { get; set; }

    public DateTime? content_expiry_date { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }

    public int? real_content_id { get; set; }

    public int? real_organization_id { get; set; }

    public DateTime? transfered_date { get; set; }

    public virtual tbl_content tbl_content { get; set; }

    public virtual tbl_organization tbl_organization { get; set; }
  }
}
