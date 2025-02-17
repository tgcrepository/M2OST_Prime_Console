// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.ScheduledEvent
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL.Models
{
  public class ScheduledEvent
  {
    public int id_scheduled_event { get; set; }

    public string event_title { get; set; }

    public string event_description { get; set; }

    public string registration_start_date { get; set; }

    public string registration_end_date { get; set; }

    public string event_start_datetime { get; set; }

    public string event_duration { get; set; }

    public string event_type { get; set; }

    public string event_group_type { get; set; }

    public string program_name { get; set; }

    public string program_description { get; set; }

    public string program_objective { get; set; }

    public string facilitator_name { get; set; }

    public string facilitator_organization { get; set; }

    public string program_image { get; set; }

    public string no_of_participants { get; set; }

    public string program_location { get; set; }

    public string attachment_type { get; set; }

    public string attachment_id { get; set; }

    public string attachment_title { get; set; }

    public string program_start_date { get; set; }

    public string program_duration_type { get; set; }

    public string program_duration { get; set; }

    public string program_duration_unit { get; set; }

    public string program_end_date { get; set; }

    public string attachment_info { get; set; }

    public string STATUS { get; set; }

    public string MESSAGE { get; set; }

    public string COMMENT { get; set; }

    public string REDIRECTION_URL { get; set; }

    public string is_approval { get; set; }

    public string is_response { get; set; }

    public string is_unsubscribe { get; set; }
  }
}
