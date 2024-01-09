using _01_framework.Application;
using ChatRoomManagement.Infrastructure.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using ServiceHost;
using ServiceHost.Hubs;

var builder = WebApplication.CreateBuilder(args);

var connectionString="Data source=.;Initial catalog=ChatRoomDB;Integrated security=true;Encrypt=True;TrustServerCertificate=True";

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

ChatRoomManagementBootStrapper.Configure(builder.Services,connectionString);
builder.Services.AddTransient<IAuthHelper, AuthHelper>();
builder.Services.AddTransient<ISecurity,Security>();
builder.Services.AddTransient<IFileUploader, FileUploder>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
    {
        o.LoginPath = new PathString("/auth/login");
        o.LogoutPath = new PathString("/auth/logout");
        o.AccessDeniedPath = new PathString("/AccessDenied");
        
    });

var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.MapHub<ChatHub>("/chatHub");

app.Run();
