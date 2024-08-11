using AutoMapper;
using DigiDock.Data.Domain;
using DigiDock.Schema.Requests;
using DigiDock.Schema.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }
    }
}
