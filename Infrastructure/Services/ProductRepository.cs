using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            var brands = await _context.ProductBrands.ToListAsync();

            return brands;
        }


        public async Task<Product> GetProductByIdAsync(int id)
        {
            var product = await _context.Products
                                .Include(p => p.ProductType)
                                .Include(p => p.ProductBrand)
                                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            var productList = await _context.Products
                                .Include(p => p.ProductType)
                                .Include(p => p.ProductBrand)
                                .ToListAsync();

            return productList;
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            var productTypes = await _context.ProductTypes.ToListAsync();

            return productTypes;
        }
    }
}