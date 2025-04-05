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
        List<Employee> FindByName(string name);
    }
}
