using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Staffly.DAL.Models;

namespace Staffly.DAL.Data.Contexts
{
    public class StafflyDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=.;Database=Staffly;Trusted_Connection=True;TrustServerCertificate=True");
        }
        public DbSet<Department> Departments { get; set; }
    }
}
