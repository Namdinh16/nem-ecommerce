using NemEcommerce.Public.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace NemEcommerce.Public.Orders
{
    public interface IOrdersAppService : ICrudAppService
        <OrderDto,
        Guid,
        PagedResultRequestDto, CreateOrderDto, CreateOrderDto>
    {

    }
}