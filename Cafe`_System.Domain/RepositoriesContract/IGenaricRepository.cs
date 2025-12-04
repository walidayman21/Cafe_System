using Cafe__System.Domain.Entities;
using Cafe__System.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe__System.Domain.RepositoriesContract
{
    public interface IGenaricRepository<TEntity> where TEntity : BaseEntity
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task<IEnumerable<TEntity>> GetAllWithSpecsAsync(ISpecification<TEntity> specs);
        Task<TEntity?> GetWithSpecsAsync(ISpecification<TEntity> specs);
        Task<TEntity?> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<int> GetCountWithspecsAsync(ISpecification<TEntity> specs);
    }
}
