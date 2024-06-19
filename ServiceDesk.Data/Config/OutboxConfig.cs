using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDesk.Data.Model;

namespace ServiceDesk.Data;

public class OutboxConfig : IEntityTypeConfiguration<Outbox>
{
    public void Configure(EntityTypeBuilder<Outbox> builder)
    {
        builder.ToTable(nameof(Outbox));
        builder.HasKey(x => x.Id);

        builder
            .Property(e => e.EntityId)
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(e => e.EntityName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(e => e.Message)
            .HasMaxLength(500)
            .IsRequired();
    }
}
