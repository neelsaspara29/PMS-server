using System.Text.Json.Serialization;

namespace PMS_backend.Model
{
    public class PatientReportsModel
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public DateTime report_date { get; set; }
        public string report_type { get; set; }
        public string description { get; set; }

        public string active_status { get; set; }  //draft, active
        public virtual PatientModel Patient { get; set; }
    
    }
}
