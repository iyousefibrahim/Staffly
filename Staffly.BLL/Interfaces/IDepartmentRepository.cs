using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Staffly.DAL.Models;

namespace Staffly.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        IEnumerable<Department> GetAll();

        Department GetById(int id);

        int Add(Department department);

        int Update(Department department);

        int Delete(Department department);

    }
}
