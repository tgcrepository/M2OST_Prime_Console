// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_assessmnt_log
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_assessmnt_log
  {
    public int id_assessmnt_log { get; set; }

    public int id_user { get; set; }

    public int? id_organization { get; set; }

    public int id_assessment_sheet { get; set; }

    public string json_response { get; set; }

    public int attempt_no { get; set; }

    public string status { get; set; }

    public DateTime? updated_date_time { get; set; }
  }
}
