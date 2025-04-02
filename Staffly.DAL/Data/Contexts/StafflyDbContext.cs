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

        public StafflyDbContext(DbContextOptions<StafflyDbContext> contextOptions):base(contextOptions) 
        {
            
        }
        public DbSet<Department> Departments { get; set; }
    }
}
