using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staffly.DAL.Models;

namespace Staffly.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        IEnumerable<Employee> GetAll();

        Employee GetById(int id);

        int Add(Employee Employee);

        int Update(Employee Employee);

        int Delete(Employee Employee);
    }
}
