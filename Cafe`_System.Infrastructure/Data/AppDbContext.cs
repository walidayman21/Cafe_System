using Cafe__System.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Cafe__System.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            SetGlobalQueryFilter<BaseEntity>(modelbuilder, e => !e.IsDeleted);

            AppContextSeed.SeedDatabase(modelbuilder);
             
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTimeOffset.UtcNow;
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = now;
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = now;
                }
                else if (entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.UpdatedAt = now;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }


        private void SetGlobalQueryFilter<TBase>(ModelBuilder modelBuilder, Expression<Func<TBase, bool>> filter) where TBase : class
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Only apply filter to types that inherit from TBase
                if (typeof(TBase).IsAssignableFrom(entityType.ClrType) && entityType.ClrType != typeof(TBase))
                {
                    try
                    {
                        var parameter = Expression.Parameter(entityType.ClrType);
                        var body = ReplacingExpressionVisitor.Replace(filter.Parameters[0], parameter, filter.Body);
                        var lambda = Expression.Lambda(body, parameter);

                        entityType.SetQueryFilter(lambda);
                    }
                    catch
                    {
                        // Skip if filter cannot be applied to this entity type
                    }
                }
            }
        }
    }
}
