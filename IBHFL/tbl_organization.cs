// Decompiled with JetBrains decompiler
// Type: IBHFL.tbl_organization
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System;
using System.Collections.Generic;

namespace IBHFL
{
  public class tbl_organization
  {
    public tbl_organization()
    {
      this.tbl_assessment = (ICollection<IBHFL.tbl_assessment>) new HashSet<IBHFL.tbl_assessment>();
      this.tbl_assessment_general = (ICollection<IBHFL.tbl_assessment_general>) new HashSet<IBHFL.tbl_assessment_general>();
      this.tbl_assessment_sheet = (ICollection<IBHFL.tbl_assessment_sheet>) new HashSet<IBHFL.tbl_assessment_sheet>();
      this.tbl_banner = (ICollection<IBHFL.tbl_banner>) new HashSet<IBHFL.tbl_banner>();
      this.tbl_category = (ICollection<IBHFL.tbl_category>) new HashSet<IBHFL.tbl_category>();
      this.tbl_category_tiles = (ICollection<IBHFL.tbl_category_tiles>) new HashSet<IBHFL.tbl_category_tiles>();
      this.tbl_content_organization_mapping = (ICollection<IBHFL.tbl_content_organization_mapping>) new HashSet<IBHFL.tbl_content_organization_mapping>();
      this.tbl_content_right_association = (ICollection<IBHFL.tbl_content_right_association>) new HashSet<IBHFL.tbl_content_right_association>();
      this.tbl_csst_role = (ICollection<IBHFL.tbl_csst_role>) new HashSet<IBHFL.tbl_csst_role>();
      this.tbl_organisation_banner = (ICollection<IBHFL.tbl_organisation_banner>) new HashSet<IBHFL.tbl_organisation_banner>();
      this.tbl_role = (ICollection<IBHFL.tbl_role>) new HashSet<IBHFL.tbl_role>();
    }

    public int ID_ORGANIZATION { get; set; }

    public int ID_INDUSTRY { get; set; }

    public int ID_BUSINESS_TYPE { get; set; }

    public string ORGANIZATION_NAME { get; set; }

    public string DESCRIPTION { get; set; }

    public string LOGO { get; set; }

    public string CONTACT_NAME { get; set; }

    public string CONTACTNUMBER { get; set; }

    public string CONTACTEMAIL { get; set; }

    public string DEFAULT_EMAIL { get; set; }

    public string STATUS { get; set; }

    public DateTime UPDATED_DATE_TIME { get; set; }

    public virtual ICollection<IBHFL.tbl_assessment> tbl_assessment { get; set; }

    public virtual ICollection<IBHFL.tbl_assessment_general> tbl_assessment_general { get; set; }

    public virtual ICollection<IBHFL.tbl_assessment_sheet> tbl_assessment_sheet { get; set; }

    public virtual ICollection<IBHFL.tbl_banner> tbl_banner { get; set; }

    public virtual tbl_business_type tbl_business_type { get; set; }

    public virtual ICollection<IBHFL.tbl_category> tbl_category { get; set; }

    public virtual ICollection<IBHFL.tbl_category_tiles> tbl_category_tiles { get; set; }

    public virtual ICollection<IBHFL.tbl_content_organization_mapping> tbl_content_organization_mapping { get; set; }

    public virtual ICollection<IBHFL.tbl_content_right_association> tbl_content_right_association { get; set; }

    public virtual ICollection<IBHFL.tbl_csst_role> tbl_csst_role { get; set; }

    public virtual tbl_industry tbl_industry { get; set; }

    public virtual ICollection<IBHFL.tbl_organisation_banner> tbl_organisation_banner { get; set; }

    public virtual ICollection<IBHFL.tbl_role> tbl_role { get; set; }
  }
}
