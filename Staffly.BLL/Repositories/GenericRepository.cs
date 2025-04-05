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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StafflyDbContext _context;

        public GenericRepository(StafflyDbContext context) {
            this._context = context;
        }
        public IEnumerable<T> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) _context.Employees.Include(E => E.Department).ToList();
            }
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public void Add(T model)
        {
            if(model is not null)
            {
                _context.Set<T>().Add(model);
            }
        }
        public void Update(T model)
        {
            if (model is not null)
            {
                _context.Set<T>().Update(model);
            }
        }
        public void Delete(T model)
        {
            if(model is not null)
            {
                _context.Set<T>().Remove(model);
            }
        }
    }
}
