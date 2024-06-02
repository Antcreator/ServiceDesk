using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDesk.Data.Model;

namespace ServiceDesk.Data.Config
{
    public class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable(nameof(Ticket));
            builder.HasKey(e => e.Id);

            builder
                .Property(e => e.Subject)
                .HasMaxLength(500)
                .IsRequired();

            builder
                .HasOne(e => e.Reporter)
                .WithMany()
                .HasForeignKey(e => e.ReporterId)
                .IsRequired();

            builder
                .HasOne(e => e.Assignee)
                .WithMany()
                .HasForeignKey(e => e.AssigneeId);
        }
    }
}
