using Microsoft.EntityFrameworkCore;
using PMS_backend.Model;

namespace PMS_backend.DataContext

{
    public class UserDbContext:DbContext
    {
        public UserDbContext()
        {

        }
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }

        public DbSet<PatientModel> Patients { get; set; }
    }
}
