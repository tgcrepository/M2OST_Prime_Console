// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.AssessmentResponce
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System.Collections.Generic;

namespace IBHFL.Models
{
  public class AssessmentResponce
  {
    public List<IBHFL.Models.Assessment> Assessment { get; set; }

    public string Message { get; set; }

    public List<UserInput> QuestionAnswer { get; set; }

    public string Attempt { get; set; }

    public int certificate_flag { get; set; }

    public int id_assessment_sheet { get; set; }

    public int id_assessment { get; set; }
  }
}
