// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_profile
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;

namespace IBHFL
{
  public class tbl_profile
  {
    public int ID_PROFILE { get; set; }

    public int ID_USER { get; set; }
    public string FullName { get; set; }
    public string FIRSTNAME { get; set; }

    public string LASTNAME { get; set; }

    public int? AGE { get; set; }

    public string LOCATION { get; set; }

    public string EMAIL { get; set; }

    public string MOBILE { get; set; }

    public string GENDER { get; set; }

    public string DESIGNATION { get; set; }

    public string CITY { get; set; }

    public string OFFICE_ADDRESS { get; set; }

    public DateTime? DATE_OF_BIRTH { get; set; }

    public DateTime? DATE_OF_JOINING { get; set; }

    public string REPORTING_MANAGER { get; set; }
        public string DM { get; set; }
        public string RM { get; set; }
        public string GM { get; set; }
        public string SM { get; set; }
    }
}
