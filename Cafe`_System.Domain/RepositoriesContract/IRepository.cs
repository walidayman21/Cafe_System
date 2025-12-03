using Cafe__System.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe__System.Domain.RepositoriesContract
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        //Task<TEntity> GetWithSpecsAsync(Ispecification<TEntity> specs);

    }
}
