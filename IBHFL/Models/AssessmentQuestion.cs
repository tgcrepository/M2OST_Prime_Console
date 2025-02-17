// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.AssessmentQuestion
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL.Models
{
  public class AssessmentQuestion
  {
    public int id_assessment_question { get; set; }

    public int id_organization { get; set; }

    public string assessment_question { get; set; }

    public string question_image { get; set; }

    public string aq_answer { get; set; }
  }
}
