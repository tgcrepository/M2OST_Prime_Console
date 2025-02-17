// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.NotificationAlert
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL.Models
{
  public class NotificationAlert
  {
    public int NOTIFICATION_ID { get; set; }

    public string NOTIFICATION_TITLE { get; set; }

    public string NOTIFICATION_MESSAGE { get; set; }

    public string NOTIFICATION_KEY { get; set; }

    public string NOTIFICATION_DESCRIPTION { get; set; }

    public string START_DATE { get; set; }

    public string END_DATE { get; set; }

    public string NOTIFICATION_TYPE { get; set; }

    public string ACTION_TYPE { get; set; }

    public string REDIRECTION_URL { get; set; }

    public int ID_USER { get; set; }
  }
}
