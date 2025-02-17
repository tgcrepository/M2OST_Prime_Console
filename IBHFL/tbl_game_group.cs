// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_game_group
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_game_group
  {
    public int id_game_group { get; set; }

    public string group_name { get; set; }

    public string group_image_path { get; set; }

    public int? id_creator { get; set; }

    public int? game_group_type { get; set; }

    public int? id_user { get; set; }

    public int? id_organization { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
