using Cafe__System.Domain.Entities;
using Cafe__System.Domain.RepositoriesContract;
using Cafe__System.Domain.Specifications;
using Cafe__System.Infrastructure.Data;
using Cafe__System.Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe__System.Infrastructure.GenericRepository
{
    internal class GenericRepository<T>(AppDbContext context) : IGenaricRepository<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;


        public void Add(T entity)
            => _context.Set<T>().Add(entity);

        public void Delete(T entity)
            => _context.Set<T>().Remove(entity);

        public void Update(T entity)
            => _context.Set<T>().Update(entity);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllWithSpecsAsync(ISpecification<T> specs)
            => await ApplySpecifications(specs).ToListAsync();


        public async Task<T?> GetByIdAsync(int id)
            => await _context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);


        public async Task<int> GetCountWithspecsAsync(ISpecification<T> specs)
            => await ApplySpecifications(specs).CountAsync();


        public async Task<T?> GetWithSpecsAsync(ISpecification<T> specs)
            => await ApplySpecifications(specs).FirstOrDefaultAsync();
        

        protected IQueryable<T> ApplySpecifications(ISpecification<T> specs)
            => SpecificationsEvaluator<T>.GetQuery(_context.Set<T>(), specs);
        
    }
}
