// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.AssessmentReport
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL.Models
{
  public class AssessmentReport
  {
    public int id_assessment_log { get; set; }

    public int id_assessment_sheet { get; set; }

    public int id_assessment { get; set; }

    public string assessment_name { get; set; }

    public string assessment_description { get; set; }

    public string attempt { get; set; }

    public string LogDate { get; set; }
  }
}
