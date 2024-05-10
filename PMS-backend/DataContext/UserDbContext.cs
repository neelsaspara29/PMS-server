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

        public DbSet<PatientReportsModel> PatientReports { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientModel>()
         .HasMany(p => p.Reports)
         .WithOne(r => r.Patient)
         .HasForeignKey(r => r.PatientId);
        }
    }
}
