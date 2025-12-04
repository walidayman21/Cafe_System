using Cafe__System.Domain;
using Cafe__System.Domain.Entities;
using Cafe__System.Domain.RepositoriesContract;
using Cafe__System.Infrastructure.Data;
using Cafe__System.Infrastructure.GenericRepository;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe__System.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<Type, object> _Repositories = new();

        public UnitOfWork(AppDbContext context, ServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IGenaricRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {

            var type = typeof(IGenaricRepository<TEntity>);

            return (IGenaricRepository<TEntity>)_Repositories.GetOrAdd(
                type,
                _ => new GenericRepository<TEntity>(_context)
            );


            //var type = typeof(TEntity).Name;

            //if (!_Repositories.ContainsKey(type))
            //{
            //    var repo = new GenericRepository<TEntity>(_context);
            //    _Repositories.Add(type, repo);
            //}
            //return (IGenaricRepository<TEntity>)_Repositories[type];
        }

        public TRepo Repository<TEntity, TRepo>()
            where TEntity : BaseEntity
            where TRepo : IGenaricRepository<TEntity>
        {
            var type = typeof(TRepo);

            return (TRepo)_Repositories.GetOrAdd(
                type,
                _ => _serviceProvider.GetRequiredService<TRepo>()
            );
        }

        public async Task<int> CompleteAsync()
            => await _context.SaveChangesAsync();

        public ValueTask DisposeAsync()
            => _context.DisposeAsync();
    }
}
