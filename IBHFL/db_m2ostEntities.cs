// Decompiled with JetBrains decompiler
// Type: IBHFL.db_m2ostEntities
// Assembly: IBHFL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 7D5F13E8-66C2-4709-8C6B-C4D11A9CFC14
// Assembly location: D:\M2OST Desktop\console\bin\IBHFL.dll

using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace IBHFL
{
    public class db_m2ostEntities : DbContext
    {
        public db_m2ostEntities()
          : base("name=db_m2ostEntities")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) => throw new UnintentionalCodeFirstException();

        public virtual DbSet<IBHFL.downtime_log> downtime_log { get; set; }

        public virtual DbSet<IBHFL.error_log> error_log { get; set; }

        public virtual DbSet<IBHFL.sc_game_element_weightage> sc_game_element_weightage { get; set; }

        public virtual DbSet<IBHFL.sc_program_assessment_scoring> sc_program_assessment_scoring { get; set; }

        public virtual DbSet<IBHFL.sc_program_content_summary> sc_program_content_summary { get; set; }

        public virtual DbSet<IBHFL.sc_program_kpi_score> sc_program_kpi_score { get; set; }

        public virtual DbSet<IBHFL.sc_program_kpi_weightage> sc_program_kpi_weightage { get; set; }

        public virtual DbSet<IBHFL.sc_program_log> sc_program_log { get; set; }

        public virtual DbSet<IBHFL.sc_report_game_process_path> sc_report_game_process_path { get; set; }

        public virtual DbSet<IBHFL.sc_report_game_process_score> sc_report_game_process_score { get; set; }

        public virtual DbSet<IBHFL.tbl_action> tbl_action { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment> tbl_assessment { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_answer> tbl_assessment_answer { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_audit> tbl_assessment_audit { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_categoty_mapping> tbl_assessment_categoty_mapping { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_general> tbl_assessment_general { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_header> tbl_assessment_header { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_index> tbl_assessment_index { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_mapping> tbl_assessment_mapping { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_push> tbl_assessment_push { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_question> tbl_assessment_question { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_scoring_key> tbl_assessment_scoring_key { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_sheet> tbl_assessment_sheet { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_theme> tbl_assessment_theme { get; set; }

        public virtual DbSet<IBHFL.tbl_assessment_user_assignment> tbl_assessment_user_assignment { get; set; }

        public virtual DbSet<IBHFL.tbl_assessmnt_log> tbl_assessmnt_log { get; set; }

        public virtual DbSet<IBHFL.tbl_assignment_role_assessment> tbl_assignment_role_assessment { get; set; }

        public virtual DbSet<IBHFL.tbl_assignment_role_content> tbl_assignment_role_content { get; set; }

        public virtual DbSet<IBHFL.tbl_assignment_role_program> tbl_assignment_role_program { get; set; }

        public virtual DbSet<IBHFL.tbl_authcode> tbl_authcode { get; set; }

        public virtual DbSet<IBHFL.tbl_banner> tbl_banner { get; set; }

        public virtual DbSet<IBHFL.tbl_business_type> tbl_business_type { get; set; }

        public virtual DbSet<IBHFL.tbl_category> tbl_category { get; set; }

        public virtual DbSet<IBHFL.tbl_category_associantion> tbl_category_associantion { get; set; }

        public virtual DbSet<IBHFL.tbl_category_heading> tbl_category_heading { get; set; }

        public virtual DbSet<IBHFL.tbl_category_order> tbl_category_order { get; set; }

        public virtual DbSet<IBHFL.tbl_category_tiles> tbl_category_tiles { get; set; }

        public virtual DbSet<IBHFL.tbl_cms_role_action> tbl_cms_role_action { get; set; }

        public virtual DbSet<IBHFL.tbl_cms_role_action_mapping> tbl_cms_role_action_mapping { get; set; }

        public virtual DbSet<IBHFL.tbl_cms_roles> tbl_cms_roles { get; set; }

        public virtual DbSet<IBHFL.tbl_cms_users> tbl_cms_users { get; set; }

        public virtual DbSet<IBHFL.tbl_content> tbl_content { get; set; }

        public virtual DbSet<IBHFL.tbl_content_answer> tbl_content_answer { get; set; }

        public virtual DbSet<IBHFL.tbl_content_answer_steps> tbl_content_answer_steps { get; set; }

        public virtual DbSet<IBHFL.tbl_content_banner> tbl_content_banner { get; set; }

        public virtual DbSet<IBHFL.tbl_content_counters> tbl_content_counters { get; set; }

        public virtual DbSet<IBHFL.tbl_content_footer> tbl_content_footer { get; set; }

        public virtual DbSet<IBHFL.tbl_content_header> tbl_content_header { get; set; }

        public virtual DbSet<IBHFL.tbl_content_header_footer> tbl_content_header_footer { get; set; }

        public virtual DbSet<IBHFL.tbl_content_level> tbl_content_level { get; set; }

        public virtual DbSet<IBHFL.tbl_content_link> tbl_content_link { get; set; }

        public virtual DbSet<IBHFL.tbl_content_metadata> tbl_content_metadata { get; set; }

        public virtual DbSet<IBHFL.tbl_content_organization_mapping> tbl_content_organization_mapping { get; set; }

        public virtual DbSet<IBHFL.tbl_content_program_mapping> tbl_content_program_mapping { get; set; }

        public virtual DbSet<IBHFL.tbl_content_right_association> tbl_content_right_association { get; set; }

        public virtual DbSet<IBHFL.tbl_content_role_mapping> tbl_content_role_mapping { get; set; }

        public virtual DbSet<IBHFL.tbl_content_type> tbl_content_type { get; set; }

        public virtual DbSet<IBHFL.tbl_content_type_link> tbl_content_type_link { get; set; }

        public virtual DbSet<IBHFL.tbl_content_user_assisgnment> tbl_content_user_assisgnment { get; set; }

        public virtual DbSet<IBHFL.tbl_csst_role> tbl_csst_role { get; set; }

        public virtual DbSet<IBHFL.tbl_device_type> tbl_device_type { get; set; }

        public virtual DbSet<IBHFL.tbl_feedback_bank> tbl_feedback_bank { get; set; }

        public virtual DbSet<IBHFL.tbl_feedback_bank_link> tbl_feedback_bank_link { get; set; }

        public virtual DbSet<IBHFL.tbl_feedback_data> tbl_feedback_data { get; set; }

        public virtual DbSet<IBHFL.tbl_game_creation> tbl_game_creation { get; set; }

        public virtual DbSet<IBHFL.tbl_game_group> tbl_game_group { get; set; }

        public virtual DbSet<IBHFL.tbl_game_group_association> tbl_game_group_association { get; set; }

        public virtual DbSet<IBHFL.tbl_game_group_participatant> tbl_game_group_participatant { get; set; }

        public virtual DbSet<IBHFL.tbl_game_path> tbl_game_path { get; set; }

        public virtual DbSet<IBHFL.tbl_game_phase> tbl_game_phase { get; set; }

        public virtual DbSet<IBHFL.tbl_game_process_path> tbl_game_process_path { get; set; }

        public virtual DbSet<IBHFL.tbl_game_solo> tbl_game_solo { get; set; }

        public virtual DbSet<IBHFL.tbl_industry> tbl_industry { get; set; }

        public virtual DbSet<IBHFL.tbl_kpi_grid> tbl_kpi_grid { get; set; }
        public virtual DbSet<IBHFL.tbl_kpi_master> tbl_kpi_master { get; set; }
        public virtual DbSet<tbl_kpi_master_details> tbl_kpi_master_details { get; set; }
        public virtual DbSet<tbl_kpi_program_scoring> tbl_kpi_program_scoring { get; set; }
        public virtual DbSet<tbl_kpi_scoring_logic_details> tbl_kpi_scoring_logic_details { get; set; }
        public virtual DbSet<tbl_kpi_scoring_master_details> tbl_kpi_scoring_master_details { get; set; }
        public virtual DbSet<tbl_kpi_sub_type_details> tbl_kpi_sub_type_details { get; set; }
        public virtual DbSet<tbl_kpi_type_details> tbl_kpi_type_details { get; set; }

        public virtual DbSet<IBHFL.tbl_notification> tbl_notification { get; set; }

        public virtual DbSet<IBHFL.tbl_notification_config> tbl_notification_config { get; set; }

        public virtual DbSet<IBHFL.tbl_notification_reminder> tbl_notification_reminder { get; set; }

        public virtual DbSet<IBHFL.tbl_offline_expiry> tbl_offline_expiry { get; set; }

        public virtual DbSet<IBHFL.tbl_organisation_banner> tbl_organisation_banner { get; set; }

        public virtual DbSet<IBHFL.tbl_organisation_banner_links> tbl_organisation_banner_links { get; set; }

        public virtual DbSet<IBHFL.tbl_organization> tbl_organization { get; set; }

        public virtual DbSet<IBHFL.tbl_profile> tbl_profile { get; set; }

        public virtual DbSet<IBHFL.tbl_reminder_notification_config> tbl_reminder_notification_config { get; set; }

        public virtual DbSet<IBHFL.tbl_reminder_notification_log> tbl_reminder_notification_log { get; set; }

        public virtual DbSet<IBHFL.tbl_report_content> tbl_report_content { get; set; }

        public virtual DbSet<IBHFL.tbl_report_login_log> tbl_report_login_log { get; set; }

        public virtual DbSet<IBHFL.tbl_role> tbl_role { get; set; }

        public virtual DbSet<IBHFL.tbl_role_user_mapping> tbl_role_user_mapping { get; set; }

        public virtual DbSet<IBHFL.tbl_rs_type_qna> tbl_rs_type_qna { get; set; }

        public virtual DbSet<IBHFL.tbl_scheduled_event> tbl_scheduled_event { get; set; }

        public virtual DbSet<IBHFL.tbl_scheduled_event_subscription_log> tbl_scheduled_event_subscription_log { get; set; }

        public virtual DbSet<IBHFL.tbl_subscriptions> tbl_subscriptions { get; set; }

        public virtual DbSet<IBHFL.tbl_survey> tbl_survey { get; set; }

        public virtual DbSet<IBHFL.tbl_survey_bank> tbl_survey_bank { get; set; }

        public virtual DbSet<IBHFL.tbl_survey_bank_link> tbl_survey_bank_link { get; set; }

        public virtual DbSet<IBHFL.tbl_survey_data> tbl_survey_data { get; set; }

        public virtual DbSet<IBHFL.tbl_temp_user_upload> tbl_temp_user_upload { get; set; }

        public virtual DbSet<IBHFL.tbl_themes> tbl_themes { get; set; }

        public virtual DbSet<IBHFL.tbl_user> tbl_user { get; set; }

        public virtual DbSet<IBHFL.tbl_user_data> tbl_user_data { get; set; }

        public virtual DbSet<IBHFL.tbl_user_device_link> tbl_user_device_link { get; set; }

        public virtual DbSet<IBHFL.tbl_user_gcm_log> tbl_user_gcm_log { get; set; }

        public virtual DbSet<IBHFL.tbl_user_programs> tbl_user_programs { get; set; }

        public virtual DbSet<IBHFL.tbl_user_zone> tbl_user_zone { get; set; }

        public virtual DbSet<IBHFL.tbl_user_zone_master> tbl_user_zone_master { get; set; }

        public virtual DbSet<IBHFL.tbl_version_control> tbl_version_control { get; set; }

        public virtual DbSet<IBHFL.tbl_prod_que_ans_video> tbl_prod_que_ans_video { get; set; }

        public virtual DbSet<tbl_user_kpi_data_log> tbl_user_kpi_data_log { get; set; }
        public virtual DbSet<tbl_assessment_mastery_score_details> tbl_assessment_mastery_score_details { get; set; }
        public virtual DbSet<tbl_assessment_right_answer_details> tbl_assessment_right_answer_details { get; set; }
        public virtual DbSet<tbl_assessment_grid_details> tbl_assessment_grid_details { get; set; }
        public virtual DbSet<tbl_content_asessment_completion_timeframe_details> tbl_content_asessment_completion_timeframe_details { get; set; }
        public virtual DbSet<tbl_content_assessment_type_master_details> tbl_content_assessment_type_master_details { get; set; }
        public virtual DbSet<tbl_content_completion_notime_details> tbl_content_completion_notime_details { get; set; }
    }
}
