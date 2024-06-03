using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDesk.Data.Model;

namespace ServiceDesk.Data.Config;

public class DocumentConfig: IEntityTypeConfiguration<Document>
{
    public void Configure(EntityTypeBuilder<Document> builder)
    {
        builder.ToTable(nameof(Document));
        builder.HasKey(e => e.Id);

        builder
            .Property(e => e.Title)
            .HasMaxLength(500)
            .IsRequired();
        
        builder
            .Property(e => e.File)
            .HasMaxLength(500)
            .IsRequired();

        builder
            .HasOne(e => e.Ticket)
            .WithMany(e => e.Documents)
            .HasForeignKey(e => e.TicketId)
            .IsRequired();
    }
}