// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_scheduled_event_subscription_log
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_scheduled_event_subscription_log
  {
    public int id_scheduled_event_subscription_log { get; set; }

    public int? id_scheduled_event { get; set; }

    public int? id_user { get; set; }

    public int? id_cms_user { get; set; }

    public int? id_organization { get; set; }

    public DateTime? event_sent_timestamp { get; set; }

    public int? event_user_response { get; set; }

    public DateTime? event_user_response_timestamp { get; set; }

    public string event_user_comment { get; set; }

    public int? apporoved_reporting_manager { get; set; }

    public DateTime? approved_date { get; set; }

    public string subscription_status { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
