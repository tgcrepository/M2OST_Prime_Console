// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.Assessment
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL.Models
{
  public class Assessment
  {
    public int id_assessment { get; set; }

    public string assessment_title { get; set; }

    public string assesment_description { get; set; }

    public int id_organization { get; set; }

    public string assess_type { get; set; }

    public string low_value { get; set; }

    public string low_title { get; set; }

    public string high_value { get; set; }

    public string high_title { get; set; }
  }
}
