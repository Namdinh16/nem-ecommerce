﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NemEcommerce.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NemEcommerce.Configurations.Inventories
{
    public class InventoryConfiguration : IEntityTypeConfiguration<Inventory>
    {
        public void Configure(EntityTypeBuilder<Inventory> builder)
        {
            builder.ToTable(NemEcommerceConsts.DbTablePrefix + "Inventories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.SKU)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsRequired();

            builder.Property(x => x.StockQuantity)
                .IsRequired();
        }
    }
}
