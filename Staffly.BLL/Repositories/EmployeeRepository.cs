using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staffly.BLL.Interfaces;
using Staffly.DAL.Data.Contexts;
using Staffly.DAL.Models;

namespace Staffly.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(StafflyDbContext context) : base(context)
        {
        }
    }
}
