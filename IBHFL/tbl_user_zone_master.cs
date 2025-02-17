// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_user_zone_master
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL
{
  public class tbl_user_zone_master
  {
    public int id_user_zone_master { get; set; }

    public int? id_organization { get; set; }

    public string zone_title { get; set; }

    public string trainer_name { get; set; }

    public int? id_user_trainer { get; set; }

    public string employee_id { get; set; }

    public string status { get; set; }

    public string updated_date_time { get; set; }
  }
}
