using System;
using System.Collections;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Services;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _storeContext;
        private Hashtable _hashtableRepositories;

        public UnitOfWork(StoreContext storeContext)
        {
            _storeContext = storeContext;            
        }

        public async Task<int> Complete()
        {
            return await _storeContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _storeContext.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            // Check if hashTableRepo is NULL, if it is; instantiate a new HashTable
            if (_hashtableRepositories == null)
            {
                _hashtableRepositories = new Hashtable();
            }

            // get the TENTITY name and
            // get the type of Entity that was passed-in i.e. Product, Brand, ProductType...
            var currentEntityByType = typeof(TEntity).Name;

            // if the TENTITY is NOT in HashtableRespository
            if (!_hashtableRepositories.ContainsKey(currentEntityByType))
            {
                var repositoryType = typeof(GenericRepository<>);

                var createInstanceOfGenericRepositoryWithEntity = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _storeContext);

                _hashtableRepositories.Add(currentEntityByType, createInstanceOfGenericRepositoryWithEntity);
            }

            var hashedGenericRepoWithEntity = (IGenericRepository<TEntity>) _hashtableRepositories[currentEntityByType];

            return hashedGenericRepoWithEntity;
        }
    }
}
