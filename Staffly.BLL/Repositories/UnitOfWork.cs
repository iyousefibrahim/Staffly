using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staffly.BLL.Interfaces;
using Staffly.DAL.Data.Contexts;

namespace Staffly.BLL.Repositories
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly StafflyDbContext _context;
        public UnitOfWork(StafflyDbContext context)
        {
            _context = context;
            EmployeeRepository = new EmployeeRepository(_context);
            DepartmentRepository = new DepartmentRepository(_context);
        }

        public IEmployeeRepository EmployeeRepository { get; }

        public IDepartmentRepository DepartmentRepository { get; }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
