using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Staffly.DAL.Models;

namespace Staffly.DAL.Data.Contexts
{
    public class StafflyDbContext : IdentityDbContext<ApplicationUser>
    {

        public StafflyDbContext(DbContextOptions<StafflyDbContext> contextOptions):base(contextOptions) 
        {
            
        }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
