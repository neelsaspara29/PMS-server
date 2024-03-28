using BCrypt.Net;

namespace PMS_backend.Services
{
    public class PasswordHasher
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public bool VerifyPassword(string hashPassword, string passwordToVerify)
        {
           return BCrypt.Net.BCrypt.EnhancedVerify(passwordToVerify, hashPassword);
        }
    }
}
