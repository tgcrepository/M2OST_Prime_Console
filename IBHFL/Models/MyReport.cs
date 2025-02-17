// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.MyReport
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System.Collections.Generic;

namespace IBHFL.Models
{
  public class MyReport
  {
    public List<AssessmentReport> LEARNING { get; set; }

    public List<AssessmentReport> PSYCHOMETRIC { get; set; }

    public int certificate_flag { get; set; }
  }
}
