using Cafe__System.Domain.Entities;
using Cafe__System.Domain.RepositoriesContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe__System.Domain
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;
        TRepo Repository<TEntity, TRepo>()
            where TRepo : IGenaricRepository<TEntity>
            where TEntity : BaseEntity;

        Task<int> CompleteAsync();
    }
}
