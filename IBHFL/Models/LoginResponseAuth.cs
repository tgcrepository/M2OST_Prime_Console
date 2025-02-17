// Decompiled with JetBrains decompiler
// Type: IBHFL.Models.LoginResponseAuth
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

namespace IBHFL.Models
{
    public class LoginResponseAuth
    {
        public string ResponseCode { get; set; }

        public int ResponseAction { get; set; }

        public string ResponseMessage { get; set; }

        public string ResponseUrl { get; set; }

        public int UserID { get; set; }

        public string UserName { get; set; }

        public string LogoPath { get; set; }

        public string BannerPath { get; set; }

        public string ROLEID { get; set; }

        public string ORGID { get; set; }

        public string ORGEMAIL { get; set; }

        public string fullname { get; set; }

        public string EMPLOYEEID { get; set; }
    }


    public class Rootobject
    {
        public string status { get; set; }
        public int statuscode { get; set; }
        public Data data { get; set; }
        public string message { get; set; }
    }

    public class Data
    {
        public _Primary _primary { get; set; }
        public _Points[] _points { get; set; }
        public _Kpi_Score[] _kpi_score { get; set; }
        public Is_Land_Logos[] is_land_logos { get; set; }
        public Theme_Details[] theme_details { get; set; }
        public object[] _poked_data { get; set; }
        public _Back_Images[] _back_images { get; set; }
        public _Personal_Data _personal_data { get; set; }
        public int is_revenue_achievement { get; set; }
        public int is_learningAcademy { get; set; }
        public int is_play_zone { get; set; }
        public string is_champions_league { get; set; }
        public string is_personal_challenge { get; set; }
        public string is_fantasy_league { get; set; }
        public string is_seasonal_theme { get; set; }
        public int is_about_game { get; set; }
        public About_Game[] about_game { get; set; }
    }

    public class _Primary
    {
        public string primary_type { get; set; }
        public string primary_name { get; set; }
        public string primary_id { get; set; }
        public string primary_supervisor { get; set; }
        public string primary_logo { get; set; }
        public string primary_rank { get; set; }
        public string primary_progress { get; set; }
        public string primary_target { get; set; }
        public string primary_label { get; set; }
    }

    public class _Personal_Data
    {
        public string id_role { get; set; }
        public string role_name { get; set; }
        public string user_department { get; set; }
        public string user_designation { get; set; }
        public string user_function { get; set; }
        public string user_grade { get; set; }
        public string cd_coroebus_user { get; set; }
        public string id_coroebus_user { get; set; }
        public string id_coroebus_organization { get; set; }
        public string cd_coroebus_organization { get; set; }
        public string id_coroebus_theme { get; set; }
        public string organization_name { get; set; }
        public string organization_logo { get; set; }
        public string USERID { get; set; }
        public string EMPLOYEEID { get; set; }
        public string first_name { get; set; }
        public string email_id { get; set; }
        public string contact_number { get; set; }
        public string profile_logo { get; set; }
        public string theme_logo { get; set; }
        public string game_logo { get; set; }
        public string id_coroebus_game { get; set; }
        public string id_coroebus_team { get; set; }
        public string game_name { get; set; }
        public string cd_coroebus_game { get; set; }
        public string team_name { get; set; }
        public string team_logo { get; set; }
        public string password { get; set; }
        public string profile_badge_image { get; set; }
        public string level_label { get; set; }
        public string profile_background_image { get; set; }
        public string background_image_profile_web { get; set; }
        public External_Kpi_Data[] external_kpi_data { get; set; }
    }

    public class External_Kpi_Data
    {
        public string kpi_name { get; set; }
        public string is_attempted { get; set; }
        public string is_correct { get; set; }
    }

    public class _Points
    {
        public string lid { get; set; }
        public string label { get; set; }
        public int score { get; set; }
        public string type { get; set; }
    }

    public class _Kpi_Score
    {
        public string ranking_arrow { get; set; }
        public string coroebus_measurement { get; set; }
        public string point_label { get; set; }
        public string point_indicator { get; set; }
        public string id_coroebus_organization { get; set; }
        public string weightage { get; set; }
    }

    public class Is_Land_Logos
    {
        public int order { get; set; }
        public string label { get; set; }
        public string tag { get; set; }
        public string logo { get; set; }
        public string level { get; set; }
        public string _userid { get; set; }
        public string game_id { get; set; }
        public string _data { get; set; }
        public int role_id { get; set; }
    }

    public class Theme_Details
    {
        public string id_coroebus_theme { get; set; }
        public string theme_label { get; set; }
        public string role_3_label { get; set; }
        public string role_4_label { get; set; }
        public string role_6_label { get; set; }
        public string player_label { get; set; }
        public string theme_background { get; set; }
        public string theme_background_web { get; set; }
        public string target_icon { get; set; }
        public string role_8_label { get; set; }
        public string role_9_label { get; set; }
        public string role_10_label { get; set; }
        public string role_11_label { get; set; }
        public string role_12_label { get; set; }
        public string dark_color { get; set; }
        public string medium_color { get; set; }
        public string light_color { get; set; }
        public string up_arrow { get; set; }
        public string down_arrow { get; set; }
        public string equal_arrow { get; set; }
        public string reward_points_image { get; set; }
        public string game_points_image { get; set; }
        public string add_ins_background { get; set; }
        public string notification_background { get; set; }
        public string island_background { get; set; }
        public string point_dist_background { get; set; }
    }

    public class _Back_Images
    {
        public int order { get; set; }
        public string label { get; set; }
        public _Data[] _data { get; set; }
    }

    public class _Data
    {
        public string ranking_badge_level { get; set; }
        public string ranking_badge { get; set; }
        public string ranking_image_level { get; set; }
        public string ranking_image { get; set; }
        public string ranking_image_profile { get; set; }
    }

    public class About_Game
    {
        public string id_about_game { get; set; }
        public string cd_about_game { get; set; }
        public string id_coroebus_organization { get; set; }
        public string id_coroebus_game { get; set; }
        public string created_by { get; set; }
        public object description { get; set; }
        public string file_path { get; set; }
        public string status { get; set; }
        public string created_date { get; set; }
        public string updated_date { get; set; }
        public string file_name { get; set; }
        public string ispdf { get; set; }
    }

}
