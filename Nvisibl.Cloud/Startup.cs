using System;
using Nvisibl.Cloud.Helpers;
using Nvisibl.Cloud.Services;
using Nvisibl.DataLibrary.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nvisibl.Cloud.Services.Interfaces;
using Nvisibl.DataLibrary.Repositories;
using Nvisibl.DataLibrary.Repositories.Interfaces;

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
            services.AddDbContext<ChatContext>(
                options => options.UseSqlServer(
                    ConnectionStringHelper.GetConnectionString(Configuration)));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserManagerService, UserManagerService>();
            services.AddTransient<IChatroomManagerService, ChatroomManagerService>();
            services.AddTransient<IMessagesManagerService, MessagesManagerService>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
