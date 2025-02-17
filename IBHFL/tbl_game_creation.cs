// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_game_creation
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_game_creation
  {
    public int id_game { get; set; }

    public int? id_organisation { get; set; }

    public int? id_game_creator { get; set; }

    public string gameid { get; set; }

    public string game_title { get; set; }

    public string game_description { get; set; }

    public string game_creator_name { get; set; }

    public DateTime? game_expiry_date { get; set; }

    public DateTime? game_start_date { get; set; }

    public string game_mode { get; set; }

    public string game_type { get; set; }

    public string id_game_path { get; set; }

    public string player_type { get; set; }

    public int? id_game_group { get; set; }

    public string game_comment { get; set; }

    public string game_phase { get; set; }

    public DateTime? game_creation_datetime { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
