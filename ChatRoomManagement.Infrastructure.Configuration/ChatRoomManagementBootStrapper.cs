
using ChatRoomManagement.Application;
using ChatRoomManagement.Application.Contracts.Chat;
using ChatRoomManagement.Application.Contracts.Group;
using ChatRoomManagement.Application.Contracts.User;
using ChatRoomManagement.Domain.ChatAgg;
using ChatRoomManagement.Domain.GroupAgg;
using ChatRoomManagement.Domain.UserAgg;
using ChatRoomManagement.Infrastructure.EfCore;
using ChatRoomManagement.Infrastructure.EfCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace ChatRoomManagement.Infrastructure.Configuration
{
    public static class ChatRoomManagementBootStrapper
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();

            services.AddTransient<IGroupApplication, GroupApplication>();
            services.AddTransient<IUserApplication, UserApplication>();

            services.AddTransient<IChatApplication, ChatApplication>();
            services.AddTransient<IChatRepository, ChatRepository>();


            services.AddDbContext<ChatRoomContext>(p => p.UseSqlServer(connectionString));
        }
    }
}
