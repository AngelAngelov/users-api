using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Users.Api.Models;
using Users.Core.Interfaces;
using Users.Data;
using Users.Data.Repositories;

namespace NHLStats.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add custom policy for CORS 
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddMvc();
            services.AddDbContext<UsersContext>();

            //Add dependencies
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<UsersQuery>();
            services.AddSingleton<UserMutation>();
            services.AddSingleton<UserType>();
            services.AddSingleton<UserInputType>();

            ServiceProvider sp = services.BuildServiceProvider();
            services.AddSingleton<ISchema>(new UserSchema(new FuncDependencyResolver(type => sp.GetService(type))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UsersContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphiQl();
            app.UseMvc();

            //Put some initial sample data
            db.EnsureSeedData();
        }
    }
}
