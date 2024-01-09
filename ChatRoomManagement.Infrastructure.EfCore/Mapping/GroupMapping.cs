using ChatRoomManagement.Domain.GroupAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatRoomManagement.Infrastructure.EfCore.Mapping
{
	public class GroupMapping : IEntityTypeConfiguration<Group>
	{
		public void Configure(EntityTypeBuilder<Group> builder)
		{
			builder.ToTable("Groups");

			builder.HasKey(x => x.Id);
			builder.Property(p=>p.GroupTitle).HasMaxLength(50);
			builder.Property(p=>p.Picture).HasMaxLength(500);

			builder.HasMany(p=>p.Users).WithMany(p=>p.Groups);
		}
	}
}
