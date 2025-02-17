// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_user_device_link
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_user_device_link
  {
    public int ID_USER_DEVICE_LINK { get; set; }

    public int ID_USER { get; set; }

    public int ID_DEVICE_TYPE { get; set; }

    public string DEVICE_ID { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATED_DATE_TIME { get; set; }
  }
}
