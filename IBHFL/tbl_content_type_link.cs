// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_content_type_link
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_content_type_link
  {
    public int ID_CONTENT_TYPE_LINK { get; set; }

    public int ID_CONTENT_ANSWER { get; set; }

    public int ID_CONTENT_TYPE { get; set; }

    public string LINK_VALUE { get; set; }

    public string DESCRIPTION { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATED_DATE_TIME { get; set; }

    public virtual tbl_content_answer tbl_content_answer { get; set; }

    public virtual tbl_content_type tbl_content_type { get; set; }
  }
}
