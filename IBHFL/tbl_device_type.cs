// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_device_type
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_device_type
  {
    public tbl_device_type() => this.tbl_user_data = (ICollection<IBHFL.tbl_user_data>) new HashSet<IBHFL.tbl_user_data>();

    public int ID_DEVICE_TYPE { get; set; }

    public string DEVICENAME { get; set; }

    public string DESCRIPTION { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATEDDATETIME { get; set; }

    public virtual ICollection<IBHFL.tbl_user_data> tbl_user_data { get; set; }
  }
}
