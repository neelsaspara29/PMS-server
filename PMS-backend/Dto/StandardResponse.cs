namespace PMS_backend.Dto
{
    public class StandardResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object data { get; set; }

        public StandardResponse(bool success, string message, object data = null)
        {
            this.success = success;
            this.message = message;
            this.data = data;
        }
    }
}
