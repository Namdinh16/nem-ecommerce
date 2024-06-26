﻿using NemEcommerce.Orders;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace NemEcommerce.Admin
{
    public class OrderInListDto : EntityDto<Guid>
    {
        public string Code { get; set; }
        public OrderStatus Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public double Total { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public Guid? CustomerUserId { get; set; }
        public Guid Id { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
