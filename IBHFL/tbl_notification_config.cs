// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_notification_config
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_notification_config
  {
    public int id_notification_config { get; set; }

    public int? id_notification { get; set; }

    public int? id_creater { get; set; }

    public string notification_key { get; set; }

    public string notification_action_type { get; set; }

    public int? id_user { get; set; }

    public int? id_content { get; set; }

    public int? id_category { get; set; }

    public int? id_assessment { get; set; }

    public string user_type { get; set; }

    public DateTime? read_timestamp { get; set; }

    public DateTime? start_date { get; set; }

    public DateTime? end_date { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
