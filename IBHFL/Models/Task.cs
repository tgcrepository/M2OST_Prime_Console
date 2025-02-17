// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.Task
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL.Models
{
  public class Task
  {
    public int id_task { get; set; }

    public int id_user { get; set; }

    public int id_org { get; set; }

    public string task { get; set; }

    public DateTime? modified_date { get; set; }

    public string status { get; set; }
  }
}
