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
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.Employees.Include(E => E.Department).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
        public async Task AddAsync(T model)
        {
            if(model is not null)
            {
                await _context.Set<T>().AddAsync(model);
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
