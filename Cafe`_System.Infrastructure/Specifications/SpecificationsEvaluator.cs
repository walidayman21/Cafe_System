using Cafe__System.Domain.Entities;
using Cafe__System.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe__System.Infrastructure.Specifications
{
    internal static class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecifications<TEntity> specs)
        {
            var qurey = inputQuery;
            //where
            if(specs.Criteria is not null)
                qurey = qurey.Where(specs.Criteria);

            //include
            qurey = specs.Includes.Aggregate(qurey, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return qurey;
        }
    }
}
