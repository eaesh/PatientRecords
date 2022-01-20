using Microsoft.EntityFrameworkCore;

namespace Backend.Models
{
    public class PatientRecordsContext : DbContext
    {
        public PatientRecordsContext(DbContextOptions<PatientRecordsContext> options) : base(options)
        {

        }

        public DbSet<Patient> Patients { get; set; } = null!;
    }
}
