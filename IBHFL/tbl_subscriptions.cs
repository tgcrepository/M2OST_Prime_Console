// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_subscriptions
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_subscriptions
  {
    public int ID_SUBSCRIPTION { get; set; }

    public int ID_USER { get; set; }

    public int ID_CONTENT { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATEDTIME { get; set; }

    public DateTime? EXPIRY_DATE { get; set; }

    public virtual tbl_content tbl_content { get; set; }

    public virtual tbl_user tbl_user { get; set; }
  }
}
