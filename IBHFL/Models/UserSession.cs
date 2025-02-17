// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.UserSession
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL.Models
{
    public class UserSession : tbl_profile
    {
        public string Username { get; set; }

        public string Roleid { get; set; }

        public string ID_USER { get; set; }

        public int id_ORGANIZATION { get; set; }

        public string logo_path { get; set; }

        public string REURL { get; set; }

        public string USERID { get; set; }

        public string fullname { get; set; }

        public bool IsSSO { get; set; }

        public string EMPLOYEEID { get; set; }

        public string APIUrl { get; set; }
        public string ApplicationUrl { get; set; }
        public string dynamic_url { get; set; }
        public string UserFunction { get; set; }
        public string UserGrade { get; set; }
        public string DM { get; set; }
        public string RM { get; set; }
        public string GM { get; set; }
        public string SM { get; set; }
        public string Role { get; set; }

    }
}
