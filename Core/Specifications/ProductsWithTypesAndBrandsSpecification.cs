using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        //  constructor
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams  productParams) 
            : base(p => (!productParams.BrandId.HasValue || p.ProductBrandId == productParams.BrandId) 
                     && (!productParams.TypeId.HasValue || p.ProductTypeId == productParams.TypeId)
                     && (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search))
            )
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);

            AddOrderByAscending(x => x.Name); // Execute OrderByAscending method by DEFAULT

            // executing Pagination
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderByAscending(x => x.Price); // Execute OrderByAscending method
                        break;
                    case "priceDesc":
                        AddOrderByDescending(x => x.Price); // Execute AddOrderByDescending method
                        break;
                    default:
                        AddOrderByAscending(x => x.Name); // Execute OrderByAscending method
                        break;
                }
            }
        }          

        public ProductsWithTypesAndBrandsSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductType);
            AddInclude(x => x.ProductBrand);
        }
    }
}
