// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.PROGRAMCOMPLETE
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL.Models
{
  public class PROGRAMCOMPLETE
  {
    public string ID_USER { get; set; }

    public int ID_CATEGORY { get; set; }

    public string USERID { get; set; }

    public string UNAME { get; set; }

    public string EMPLOYEEID { get; set; }

    public string ID_ORGANIZATION { get; set; }

    public string ORGANIZATION_NAME { get; set; }

    public string CATEGORYNAME { get; set; }

    public int TOTALCOUNT { get; set; }

    public int CHECKCOUNT { get; set; }

    public double PERCENTAGE { get; set; }

    public DateTime assigned_date { get; set; }

    public DateTime start_date { get; set; }

    public DateTime end_date { get; set; }

    public string LOCATION { get; set; }

    public string DESIGNATION { get; set; }

    public string RMUSER { get; set; }

    public string USTATUS { get; set; }
  }
}
