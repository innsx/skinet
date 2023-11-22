using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity> specifications)
        {
            var query = inputQuery;
            
            // the ORDER of EXECUTION of "specifications" BELOW is VERY important
            // ********************************************************************
            if (specifications.Criteria != null)
            {
                query = query.Where(specifications.Criteria); // Filtering expression: p => p.ProductTypeId == id
            }

            if (specifications.OrderBy != null)
            {
                query = query.OrderBy(specifications.OrderBy); //sorting orderByAscending
            }

            if (specifications.OrderByDescending != null)
            {
                query = query.OrderByDescending(specifications.OrderByDescending); //sorting orderByDescending
            }

            if (specifications.IsPagingEnabled)
            {
                query = query.Skip(specifications.Skip).Take(specifications.Take); // executing paginations
            }


            query = specifications.Includes.Aggregate(query, (currentEntity, includeStatement) => currentEntity.Include(includeStatement));

            return query;        
        }
    }
}
