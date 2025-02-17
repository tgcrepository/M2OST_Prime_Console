// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.tbl_non_disclousure_clause_content
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL.Models
{
  public class tbl_non_disclousure_clause_content
  {
    public int id_clause_content { get; set; }

    public int id_org { get; set; }

    public string content_title { get; set; }

    public string content { get; set; }

    public DateTime updated_date_time { get; set; }

    public int id_creator { get; set; }

    public string status { get; set; }
  }
}
