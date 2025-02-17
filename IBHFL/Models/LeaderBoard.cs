// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.LeaderBoard
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL.Models
{
  public class LeaderBoard
  {
    public int id_user { get; set; }

    public double OverallScore { get; set; }

    public int Percentile { get; set; }

    public string Ordinal { get; set; }

    public string username { get; set; }

    public string status { get; set; }
  }
}
