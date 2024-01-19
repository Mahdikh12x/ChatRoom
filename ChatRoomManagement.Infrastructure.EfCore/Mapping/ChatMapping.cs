

using ChatRoomManagement.Domain.ChatAgg;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatRoomManagement.Infrastructure.EfCore.Mapping
{
    public class ChatMapping : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(p=>p.Id);


            builder.HasOne(p=>p.User).WithMany(p=>p.Chats).HasForeignKey(p=>p.UserId);
            builder.HasOne(p=>p.Group).WithMany(p=>p.Chats).HasForeignKey(p=>p.GroupId);

        }
    }
}
