﻿namespace PMS_backend.Model
{
    public class UserModel
    {
        public int Id { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string userName { get; set; }
        public string userRole { get; set; }        //admin, sub-admin
        public string active_status { get; set; }   // approved, pending, block
    }
}
