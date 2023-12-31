using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepo, IGenericRepository<ProductBrand> productBrandRepo, 
                                  IGenericRepository<ProductType> productTypeRepo, IMapper mapper)
        {
            _mapper = mapper;
            _productsRepo = productRepo;
            _productBrandRepo = productBrandRepo;
            _productTypeRepo = productTypeRepo;
        }

        [Cached(600)] // cache for 600 seconds
        [HttpGet()]        
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams productParams)
        {
            var specsToQuery = new ProductsWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductsWithFiltersForCountSpecification(productParams);

            var totalItems = await _productsRepo.CountAsync(countSpec);  
            
            var products = await _productsRepo.ListAsync(specsToQuery);

            // var productToReturnDtoList = products.Select(product => new ProductToReturnDto
            // {                
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     Price = product.Price,
            //     PictureUrl = product.PictureUrl,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name
            // })
            // .ToList();

            // return Ok(productToReturnDtoList);

            if (products == null)
            {
                int statusCode = 400;
                ApiResponse errorMessage = new ApiResponse(statusCode);
                
                return NotFound(errorMessage);
            }

            var productsToReturnDtoList = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            var productsWithPagination = new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, productsToReturnDtoList);

            return Ok(productsWithPagination);
        }

        [Cached(600)] // cache object for 600 seconds
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProductById(int id)
        {
            var specsToQuery = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _productsRepo.GetEntityWithSpec(specsToQuery);

            // var productToReturnDto = new ProductToReturnDto
            // {                
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     Price = product.Price,
            //     PictureUrl = product.PictureUrl,
            //     ProductBrand = product.ProductBrand.Name,
            //     ProductType = product.ProductType.Name
            // };

            // return Ok(productToReturnDto);

            if (product == null)
            {
                int statusCode = 404;
                ApiResponse errorMessage = new ApiResponse(statusCode);
                return NotFound(errorMessage);
            }

            var productToReturnDto = _mapper.Map<Product, ProductToReturnDto>(product);

            return Ok(productToReturnDto);
        }

        [Cached(600)] // cache object for 600 seconds
        // productBrand
        [HttpGet("brands")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductBrand>>> GetProductBrands()
        {
            var productBrands = await _productBrandRepo.ListAllAsync();

            return Ok(productBrands);
        }

        [HttpGet("brands/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductBrand>> GetProductBrandById([FromQuery] int id)
        {
            var productBrand = await _productBrandRepo.GetByIdAsync(id);

            if (productBrand == null)
            {
                int statusCode = 404;
                ApiResponse errorMessage = new ApiResponse(statusCode);
                return NotFound(errorMessage);
            }

            return Ok(productBrand);
        }

        [Cached(600)] // cache object for 600 seconds
        // ProductTypes
        [HttpGet("types")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<ProductType>>> GetProductTypes()
        {
            var productTypes = await _productTypeRepo.ListAllAsync();

            return Ok(productTypes);
        }

        [HttpGet("types/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductType>> GetProductTypeById([FromQuery] int id)
        {
            var ProductType = await _productTypeRepo.GetByIdAsync(id);

            if (ProductType == null)
            {
                int statusCode = 404;
                ApiResponse errorMessage = new ApiResponse(statusCode);
                return NotFound(errorMessage);
            }

            return Ok(ProductType);
        }
    }
}
