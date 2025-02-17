// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_content_link
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_content_link
  {
    public int ID_CONTENT_LINK { get; set; }

    public int ID_CONTENT_PARENT { get; set; }

    public int ID_CONTENT_CHILD { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATED_DATE_TIME { get; set; }

    public int ID_LINK_TYPE { get; set; }
  }
}
