using AutoMapper;
using DigiDock.Data.Domain;
using DigiDock.Schema.Requests;
using DigiDock.Schema.Responses;

namespace DigiDock.Business.Mapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Product, ProductResponse>();
            CreateMap<ProductRequest, Product>();
            CreateMap<ProductUpdateRequest, Product>();

            CreateMap<SignInRequest, User>();
            CreateMap<SignInRequest, UserPassword>();

            CreateMap<CouponCreateRequest, Coupon>();
            CreateMap<CouponUpdateRequest, Coupon>();
            CreateMap<Coupon, CouponResponse>();

            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderDetails));
            CreateMap<OrderDetail, OrderItemResponse>();

            CreateMap<Category, CategoryResponse>();
            CreateMap<CategoryRequest, Category>();
        }
    }
}
