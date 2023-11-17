using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> specifications)
        {
            var specificationResults = await ApplySpecification(specifications).FirstOrDefaultAsync();

            return specificationResults;
        }

        public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specifications)
        {
            var specificationResultsList = await ApplySpecification(specifications).ToListAsync();

            return specificationResultsList;
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> specifications)
        {
            var specificationQueryResults = SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specifications);

            return specificationQueryResults;
        }

    }
}
