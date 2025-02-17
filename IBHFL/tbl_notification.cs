// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_notification
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_notification
  {
    public int id_notification { get; set; }

    public int notification_type { get; set; }

    public string notification_key { get; set; }

    public string notification_name { get; set; }

    public string notification_description { get; set; }

    public string notification_message { get; set; }

    public int reminder_flag { get; set; }

    public int id_organization { get; set; }

    public DateTime? created_date { get; set; }

    public int reminder_time { get; set; }

    public int reminder_frequency { get; set; }

    public DateTime? start_date { get; set; }

    public DateTime? end_date { get; set; }

    public string notification_action_type { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
