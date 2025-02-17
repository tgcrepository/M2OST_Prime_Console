// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.ColorApi
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL.Models
{
  public class ColorApi
  {
    public int id_color_config { get; set; }

    public int id_organisation { get; set; }

    public int config_type { get; set; }

    public string grid1_bk_color { get; set; }

    public string grid1_text_color { get; set; }

    public string grid2_bk_color { get; set; }

    public string grid2_text_color { get; set; }

    public object status { get; set; }

    public DateTime created_date_time { get; set; }

    public DateTime updated_date_time { get; set; }
  }
}
