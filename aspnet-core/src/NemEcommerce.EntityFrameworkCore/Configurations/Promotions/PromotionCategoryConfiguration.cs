﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NemEcommerce.Promotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemEcommerce.Configurations.Promotions
{
    public class PromotionCategoryConfiguration : IEntityTypeConfiguration<PromotionCategory>
    {
        public void Configure(EntityTypeBuilder<PromotionCategory> builder)
        {
            builder.ToTable(NemEcommerceConsts.DbTablePrefix + "PromotionCategories");
            builder.HasKey(x => x.Id);
        }
    }
}
