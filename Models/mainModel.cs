namespace ISB.Models
{
    public class mainModel
    {
        public List<employee> logined_user { get; set; }
        public List<Companies> companies_details { get; set; }
        public Companies company_data { get; set; }
        public List<roles> role_detail { get; set; }
        public roles roles_data { get; set; }

        public List<depatement> department_details { get; set; }
        public depatement department_data { get; set; }
        public List<designation> design_details { get; set; }
        public designation design_data { get; set; }
        public List<employee> employee_details { get; set; }
        public employee employee_data { get; set; }
        public client client_data { get; set; }

        public List<client> client_details { get; set; }
        public List<team> team_details { get; set; }
        public team team_data { get; set; }
        public project project_data { get; set; }
        public List<project> project_details { get; set; }
        public List<task> task_details { get; set; }
        public task task_data { get; set; }
        public survey survey_data { get; set; }
        public List<survey> survey_details { get; set; }
        public List<Impact_risk> impact_Risks { get; set; }
        public Impact_risk impactrisk_data { get; set; }
        public List<Likelihood_risk> Likelihood_risk { get; set; }
        public Likelihood_risk likelihoodrisk_data { get; set; }
        public List<risk> risk_details { get; set; }
        public risk risk_data { get; set; }
        public List<project_status> pro_Statusdetail { get; set; }
        public project_status proStatus_data { get; set; }
        public  List<project_type> projects_types { get; set; }
        public project_type projects_typesData { get; set; }
        public List<task_type> task_typedetails { get; set; }
       
        public task_type task_typedata { get; set; }
        public task_status task_statusdata { get; set; }
        public List<task_status> task_statusdetails { get; set; }
        public List<project_priority> projects_prioritesDetails { get; set; }
        public project_priority projects_prioritesData { get; set; }
        public List<task_priority> task_prioritesDetails { get; set; }
        public task_priority task_prioritesData { get; set; }
        public List<task_files> files_details { get; set; }
        public List<sub_task> subTask_details { get; set; }
        public sub_task subTask_data { get; set; }
        public List<emp_type> emp_types_details { get; set; }
        public emp_type emp_types_data { get; set; }
        public List<maritalStatus> materialStatus_details { get; set; }
        public maritalStatus materialStatus_data { get; set; }
  
        public List<risk_type> Risktypes_details { get; set; }
        public risk_type Risktypes_data { get; set; }
    }
}
