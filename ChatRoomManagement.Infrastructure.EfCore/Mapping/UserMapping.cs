

using ChatRoomManagement.Domain.UserAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatRoomManagement.Infrastructure.EfCore.Mapping
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.Property(p=>p.Name).HasMaxLength(50).IsRequired();
            builder.Property(p=>p.UserName).HasMaxLength(50).IsRequired();
            builder.Property(p=>p.Email).HasMaxLength(150).IsRequired();
            builder.Property(p=>p.Password).HasMaxLength(300).IsRequired();
            builder.Property(p=>p.Picture).HasMaxLength(500).IsRequired();



            builder.HasMany(p=>p.Groups).WithMany(p=>p.Users);
        }
    }
}
