// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_user
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_user : tbl_profile
  {
    public tbl_user()
    {
      this.tbl_assessment_general = (ICollection<IBHFL.tbl_assessment_general>) new HashSet<IBHFL.tbl_assessment_general>();
      this.tbl_feedback_data = (ICollection<IBHFL.tbl_feedback_data>) new HashSet<IBHFL.tbl_feedback_data>();
      this.tbl_offline_expiry = (ICollection<IBHFL.tbl_offline_expiry>) new HashSet<IBHFL.tbl_offline_expiry>();
      this.tbl_subscriptions = (ICollection<IBHFL.tbl_subscriptions>) new HashSet<IBHFL.tbl_subscriptions>();
      this.tbl_survey_data = (ICollection<IBHFL.tbl_survey_data>) new HashSet<IBHFL.tbl_survey_data>();
      this.tbl_user_data = (ICollection<IBHFL.tbl_user_data>) new HashSet<IBHFL.tbl_user_data>();
    }

    public int ID_USER { get; set; }

    public int ID_CODE { get; set; }
    public string FullName { get; set; }
        public int Dense_Rank { get; set; }

    public int? ID_ORGANIZATION { get; set; }

    public int ID_ROLE { get; set; }
        public int  ASSESSMENT_TYPE { get; set; }

        public string USERID { get; set; }

    public string PASSWORD { get; set; }

    public string FBSOCIALID { get; set; }

    public string GPSOCIALID { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATEDTIME { get; set; }

    public DateTime? EXPIRY_DATE { get; set; }

    public string EMPLOYEEID { get; set; }

    public string user_department { get; set; }

    public string user_designation { get; set; }

    public string user_function { get; set; }

    public string user_grade { get; set; }

    public string user_status { get; set; }
    public double PointsScored { get; set; }

    public int? reporting_manager { get; set; }

    public int? is_reporting { get; set; }
        public string Role { get; set; }

    public virtual ICollection<IBHFL.tbl_assessment_general> tbl_assessment_general { get; set; }

    public virtual ICollection<IBHFL.tbl_feedback_data> tbl_feedback_data { get; set; }

    public virtual ICollection<IBHFL.tbl_offline_expiry> tbl_offline_expiry { get; set; }

    public virtual ICollection<IBHFL.tbl_subscriptions> tbl_subscriptions { get; set; }

    public virtual ICollection<IBHFL.tbl_survey_data> tbl_survey_data { get; set; }

    public virtual ICollection<IBHFL.tbl_user_data> tbl_user_data { get; set; }
  }
}
