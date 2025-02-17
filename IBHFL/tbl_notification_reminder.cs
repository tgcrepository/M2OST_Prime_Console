// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_notification_reminder
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_notification_reminder
  {
    public int id_notification_reminder { get; set; }

    public int? id_notification { get; set; }

    public int? id_reminder_notification { get; set; }

    public int? reminder_timeout { get; set; }

    public int? reminder_frequency { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
