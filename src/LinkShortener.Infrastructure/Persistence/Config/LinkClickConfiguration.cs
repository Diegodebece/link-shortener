using LinkShortener.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LinkShortener.Infrastructure.Persistence.Config
{
    public class LinkClickConfiguration : IEntityTypeConfiguration<LinkClick>
    {
        public void Configure(EntityTypeBuilder<LinkClick> builder)
        {
            throw new NotImplementedException();
        }
    }
}
