namespace PMS_backend.Dto
{
    public class ReportEditBody
    {
        public int id { get; set; }  
        public string report_type { get; set; }

        public string description { get; set; } 

        public string active_status { get; set; }
    }
}
