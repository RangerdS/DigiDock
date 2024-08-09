﻿using AutoMapper;
using DigiDock.Base.Responses;
using DigiDock.Business.Cqrs;
using DigiDock.Data.Domain;
using DigiDock.Data.UnitOfWork;
using DigiDock.Schema.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Business.Query.ProductQueries
{
    public class GetAllPoductQueryHandler : IRequestHandler<GetAllProductQuery, ApiResponse<List<ProductResponse>>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public GetAllPoductQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<ApiResponse<List<ProductResponse>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            List<Product> entityList = await unitOfWork.ProductRepository.GetAllAsync();
            var mappedList = mapper.Map<List<ProductResponse>>(entityList);
            return ApiResponse<List<ProductResponse>>.SuccessResponse(mappedList);
        }
    }
}
