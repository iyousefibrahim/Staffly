using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Staffly.BLL.Interfaces;
using Staffly.DAL.Data.Contexts;
using Staffly.DAL.Models;

namespace Staffly.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        private readonly StafflyDbContext _context;

        public EmployeeRepository(StafflyDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<List<Employee>> FindByNameAsync(string name)
        {
            return await _context.Employees
                .Include(E => E.Department)
                .Where(E => E.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
    }
}
