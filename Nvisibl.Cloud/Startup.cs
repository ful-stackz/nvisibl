using System;
using Nvisibl.Cloud.Helpers;
using Nvisibl.DataLibrary.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nvisibl.DataLibrary.Repositories;
using Nvisibl.DataLibrary.Repositories.Interfaces;
using Nvisibl.Cloud.Middleware;
using Nvisibl.Cloud.WebSockets.Interfaces;
using Nvisibl.Cloud.WebSockets;
using Nvisibl.Cloud.WebSockets.Messages.Interfaces;
using Nvisibl.Cloud.WebSockets.Messages;
using Nvisibl.Business.Interfaces;
using Nvisibl.Business;
using Microsoft.AspNetCore.Identity;
using Nvisibl.Cloud.Authentication;

namespace Nvisibl.Cloud
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(options => options.AddConsole());

            services.AddDbContext<AuthContext>(
                options => options.UseMySql(
                    ConnectionStringHelper.GetConnectionString(Configuration)));

            services.AddDbContext<ChatContext>(
                options => options.UseMySql(
                    ConnectionStringHelper.GetConnectionString(Configuration)));

            services.AddIdentity<IdentityUser, IdentityRole>(
                config =>
                {
                    config.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<AuthContext>()
                .AddDefaultTokenProviders();

            var jwtConfiguration = new JwtConfiguration(Configuration);
            services.AddSingleton(jwtConfiguration);

            services.AddAuthentication()
                .AddJwtBearer(
                    JwtSchemes.Admin,
                    options => jwtConfiguration.ConfigureJwtBearerOptions(JwtSchemes.Admin, options))
                .AddJwtBearer(
                    JwtSchemes.User,
                    options => jwtConfiguration.ConfigureJwtBearerOptions(JwtSchemes.User, options));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUsersManager, UsersManager>();
            services.AddTransient<IChatroomsManager, ChatroomsManager>();
            services.AddTransient<IMessagesManager, MessagesManager>();

            services.AddSingleton<IMessengerService, MessengerService>();
            services.AddSingleton<IMessageParser, MessageParser>();
            services.AddSingleton<ClientsManager>();
            services.AddSingleton<IChatClientsManager>(sp => sp.GetService<ClientsManager>());
            services.AddSingleton<INotificationClientManager>(sp => sp.GetService<ClientsManager>());

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseWebSockets();

            app.UseWebSocketsMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
