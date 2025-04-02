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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StafflyDbContext _context;

        public GenericRepository(StafflyDbContext context) {
            this._context = context;
        }
        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public int Add(T model)
        {
            if(model is not null)
            {
                _context.Set<T>().Add(model);
                return _context.SaveChanges();
            }
            return 0;
        }
        public int Update(T model)
        {
            if(model is not null)
            {
                _context.Set<T>().Update(model);
                return _context.SaveChanges();
            }
            return 0;
        }
        public int Delete(T model)
        {
            if(model is not null)
            {
                _context.Set<T>().Remove(model);
                return _context.SaveChanges();
            }
            return 0;
        }
    }
}
