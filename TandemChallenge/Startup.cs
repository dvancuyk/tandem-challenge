using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using TandemChallenge.Api.Mapping;
using TandemChallenge.Api.Validation;
using TandemChallenge.Domain;
using TandemChallenge.Domain.Configuration;
using TandemChallenge.Infrastructure.MongoDb;

namespace TandemChallenge
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddUserViewModelValidator>()); 

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TandemChallenge", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.Configure<MongoConnection>(configuration.GetSection("MongoConnection"));
            services.AddScoped<IUserRepository, MongoUserRepository>();
            services.AddMediatR(typeof(CreateUserCommand).Assembly);
            services.AddAutoMapper(cfg => {
                cfg.AddProfile<AddUserViewModelToCreateUserCommandProfile>();
                cfg.AddProfile<CreateUserRequestToUserMappingProfile>();
                cfg.AddProfile<UserToUserViewModelProfile>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TandemChallenge v1"));
            }

            if(env.EnvironmentName != "tests")
            {
                app.UseHttpsRedirection();
            }
            

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
