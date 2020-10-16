using System;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using GoodJobGames.Data;
using GoodJobGames.Business.Mappers;
using GoodJobGames.Business.Validators;
using GoodJobGames.Utilities.Middleware;
using GoodJobGames.Utilities.ConfigModels;
using GoodJobGames.Utilities.ValidationHelper.ValidatorResolver;
using Microsoft.EntityFrameworkCore;
using GoodJobGames.Business.Services.Implementation;
using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Utilities.Configurations.Startup;
using GoodJobGames.Utilities.Filters;
using GoodJobGames.Utilities.Helper;
using Microsoft.AspNetCore.Http;

namespace GoodJobGames
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
            services.AddControllers();

            //services.AddLogging(builder => builder.AddNLog());
            services.AddOptionsConfiguration(Configuration);
            services.AddMvc(options => options.Filters.Add<GeneralExceptionFilter>());
            services.AddMvc(options => options.EnableEndpointRouting = true);

            AddSelectedDataStorage(services);
            AddMappers(services);
            AddValidations(services);
            AddBusinessServices(services);
            AddCacheServices(services);

            services.AddSwaggerGen(c =>
            c.SwaggerDoc(name: "v1", new OpenApiInfo { Title = "Game API", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(option => option.AllowAnyHeader()
                                        .AllowAnyMethod()
                                        .AllowAnyOrigin());

            app.UseSwagger();

            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint(url: "/swagger/v1/swagger.json", name: "Game API");
            });

            app.UseRouting();

            //app.UseAuthorizatioN();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseTraceIdMiddleware();

            app.Run(context => context.Response.WriteAsync("Hello World, from ASP.NET COREREERE!"));
        }

        private void AddSelectedDataStorage(IServiceCollection services)
        {
            DataStorage dataStorage = ConfigurationHelper.GetDataStorage(Configuration);

            switch (dataStorage.DataStorageType)
            {
                case DataStorageTypes.SqlServer:
                    services.AddDbContext<DataContext>(builder => builder.UseSqlServer(dataStorage.ConnectionString));
                    break;
                case DataStorageTypes.RDS:
                    services.AddDbContext<DataContext>(builder => builder.UseSqlServer(dataStorage.ConnectionString));
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"{dataStorage.DataStorageType} has not been pre-defined");
            }
        }

        private void AddMappers(IServiceCollection services)
        {
            //TODO: Create and add which model mapped to which
            services.AddAutoMapper(typeof(UserMappingProfiles));
            services.AddAutoMapper(typeof(ScoreMappingProfiles));
        }

        private void AddValidations(IServiceCollection services)
        {
            //TODO: Add Request Validators
            services.AddSingleton<IValidatorResolver, ValidatorResolver>();
            services.AddSingleton<IValidator, UserRequestValidator>();
            services.AddSingleton<IValidator, ScoreRequestValidator>();
        }

        private void AddBusinessServices(IServiceCollection services)
        {
            //TODO: Add Services (external,internal)
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IScoreService, ScoreService>();
            services.AddScoped<ICountryService, CountryService>();
        }

        private void AddCacheServices(IServiceCollection services)
        {
            //TODO: Add Services (external,internal)
            services.AddSingleton<RedisServer>();
            services.AddSingleton<ICacheService, RedisCacheService>();
        }
    }
}
