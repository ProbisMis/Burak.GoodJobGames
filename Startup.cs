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
using GoodJobGames.Utilities.Constants;
using System.Text;
using GoodJobGames.Business.Services.Interface;
using GoodJobGames.Utilities.Configurations.Startup;
using GoodJobGames.Utilities.Filters;
using GoodJobGames.Utilities.Helper;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StackExchange.Redis;
using Microsoft.Extensions.Caching.Distributed;
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

            //var csredis = new CSRedis.CSRedisClient("mymaster,password=123,prefix=my_",
            //    new[] { "127.0.0.1:26379", "192.169.1.11:26379", "192.169.1.12:26379" });
            //RedisHelper.Initialization(csredis);
            //services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));
            var csredis = new CSRedis.CSRedisClient("rgjg-edis-2.81gayg.ng.0001.euc1.cache.amazonaws.com:6379");
            RedisHelper.Initialization(csredis);
            services.AddSingleton<IDistributedCache>(new Microsoft.Extensions.Caching.Redis.CSRedisCache(RedisHelper.Instance));
            //ConnectionMultiplexer.Connect("gjgredis.81gayg.ng.0001.euc1.cache.amazonaws.com:6379");
            //services.AddDistributedRedisCache(option =>
            //{
            //    option.Configuration = "127.0.0.1:6379";
            //    option.InstanceName = "master";
            //});

            AddSelectedDataStorage(services);
            AddMappers(services);
            AddValidations(services);
            AddBusinessServices(services);

            //// JWT authentication Aayarlaması
            //var key = Encoding.ASCII.GetBytes(AppConstants.JWTSecretKey);
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});

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

            //app.UseAuthorization();

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
        }
    }
}
