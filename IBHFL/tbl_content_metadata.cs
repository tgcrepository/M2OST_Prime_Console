// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_content_metadata
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_content_metadata
  {
    public int ID_CONTENT_METADATA { get; set; }

    public string CONTENT_METADATA { get; set; }

    public int CONTENT_METADATA_COUNTER { get; set; }

    public int ID_CONTENT_ANSWER { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATEDTIME { get; set; }

    public virtual tbl_content_answer tbl_content_answer { get; set; }
  }
}
