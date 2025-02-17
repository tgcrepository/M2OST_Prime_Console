// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_game_path
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL
{
  public class tbl_game_path
  {
    public int id_game_path { get; set; }

    public int? id_category_tile { get; set; }

    public int? id_category_heading { get; set; }

    public int? id_category { get; set; }

    public int? id_program { get; set; }

    public int? Program_sequence_order { get; set; }

    public string is_program_mandatory { get; set; }

    public string program_weightage { get; set; }

    public string program_select_flag { get; set; }

    public int? id_assessment { get; set; }

    public int? assessment_sequence_order { get; set; }

    public string is_assessment_mandatory { get; set; }

    public int? assessment_weightage { get; set; }

    public string assessment_select_flag { get; set; }

    public int id_game { get; set; }
  }
}
