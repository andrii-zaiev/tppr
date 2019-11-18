using Lab1.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab1.DataAccess
{
    public class PhoneDatabaseContext : DbContext
    {
        public DbSet<Phone> Phones { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Parameter> Parameters { get; set; }

        public DbSet<ParameterValue> ParameterValues { get; set; }

        public DbSet<PhoneParameterValue> PhoneParameterValues { get; set; }

        public DbSet<PhoneSelection> PhoneSelections { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=localhost;Database=TPPRPhones;Integrated Security=SSPI;");
    }
}
