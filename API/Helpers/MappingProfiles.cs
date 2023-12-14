using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.OrderAggregate;
// using Core.Entities.Identity;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(dest => dest.ProductBrand, option => option.MapFrom(source => source.ProductBrand.Name))
                .ForMember(dest => dest.ProductType, option => option.MapFrom(source => source.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom<ProductUrlResolver>());
            
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();

            CreateMap<CustomerBasketDto, CustomerBasket>();

            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<AddressDto, Core.Entities.OrderAggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethod, o => o.MapFrom(source => source.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, o => o.MapFrom(source => source.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, o => o.MapFrom(source => source.ItemOrdered.ProductItemId))
                .ForMember(dest => dest.ProductName, o => o.MapFrom(source => source.ItemOrdered.ProductName))
                .ForMember(dest => dest.PictureUrl, o => o.MapFrom(source => source.ItemOrdered.PictureUrl))
                .ForMember(dest => dest.PictureUrl, o => o.MapFrom<OrderItemUrlResolver>());

        }
    }
}


/*
            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethod, o => o.MapFrom(source => source.DeliveryMethod.ShortName))
                .ForMember(dest => dest.ShippingPrice, o => o.MapFrom(source => source.DeliveryMethod.Price));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductId, o => o.MapFrom(source => source.ItemOrdered.ProductItemId))
                .ForMember(dest => dest.ProductName, o => o.MapFrom(source => source.ItemOrdered.ProductName))
                .ForMember(dest => dest.PictureUrl, o => o.MapFrom(source => source.ItemOrdered.PictureUrl));

*/