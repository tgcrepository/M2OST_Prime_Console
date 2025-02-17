// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_reminder_notification_config
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_reminder_notification_config
  {
    public int id_reminder_notification_config { get; set; }

    public int? id_notification { get; set; }

    public int? reminder_type { get; set; }

    public int? DH { get; set; }

    public int? YH { get; set; }

    public int? YM { get; set; }

    public int? TM { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
