using domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace infrastructure.Persistence
{
    internal class OrderActivityEntityTypeConfiguration : IEntityTypeConfiguration<OrderActivity>
    {
        public void Configure(EntityTypeBuilder<OrderActivity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd(); 
        }
    }
}
