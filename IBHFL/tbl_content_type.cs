// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_content_type
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_content_type
  {
    public tbl_content_type() => this.tbl_content_type_link = (ICollection<IBHFL.tbl_content_type_link>) new HashSet<IBHFL.tbl_content_type_link>();

    public int ID_CONTENT_TYPE { get; set; }

    public string TYPE_NAME { get; set; }

    public string TYPE_DESCRIPTION { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATED_DATE_TIME { get; set; }

    public virtual ICollection<IBHFL.tbl_content_type_link> tbl_content_type_link { get; set; }
  }
}
