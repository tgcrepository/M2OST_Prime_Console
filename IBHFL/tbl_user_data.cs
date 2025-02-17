// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_user_data
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_user_data
  {
    public int ID_USER_DATA { get; set; }

    public int ID_USER { get; set; }

    public int ID_DEVICE_TYPE { get; set; }

    public string DEVICE_ID { get; set; }

    public int ID_ACTION { get; set; }

    public DateTime UPDATEDDATETIME { get; set; }

    public virtual tbl_action tbl_action { get; set; }

    public virtual tbl_device_type tbl_device_type { get; set; }

    public virtual tbl_user tbl_user { get; set; }
  }
}
