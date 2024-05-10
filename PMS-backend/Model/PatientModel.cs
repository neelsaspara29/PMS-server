namespace PMS_backend.Model
{
    public class PatientModel
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public string mobile_number { get; set; }
        public string gender { get; set; }
        public DateTime dob { get; set; }

        public virtual ICollection<PatientReportsModel> Reports { get; set; } = new List<PatientReportsModel>();
    }
}   