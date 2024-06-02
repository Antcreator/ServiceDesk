using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ServiceDesk.Data.Model;

namespace ServiceDesk.Data.Config
{
    internal class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable(nameof(User));
            builder.HasKey(e => e.Id);

            builder
                .Property(e => e.FirstName)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(e => e.LastName)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(e => e.Email)
                .HasMaxLength(250)
                .IsRequired();

            builder
                .Property(e => e.Password)
                .IsRequired();

            builder
                .HasIndex(e => e.Email)
                .IsUnique();
        }
    }
}
