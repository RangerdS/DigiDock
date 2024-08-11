using DigiDock.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using Hangfire;
using DigiDock.Api.MiddleWares;
using Autofac;
using Hangfire.SqlServer;
using DigiDock.Business.Mapper;
using MediatR;
using DigiDock.Business.Cqrs;
using DigiDock.Business.Command.ProductCommands;
using DigiDock.Data.UnitOfWork;
using DigiDock.Business.Services;
using DigiDock.Base.Token;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DigiDock.Business.Token;

namespace DigiDock.Api;

public class Startup
{
    public IConfiguration Configuration;
    public static JwtConfig jwtConfig { get; private set; }

    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        #region Jwt
        jwtConfig = Configuration.GetSection("JwtConfig").Get<JwtConfig>();
        services.AddSingleton<JwtConfig>(jwtConfig);
        #endregion

        #region Http Context
        services.AddHttpContextAccessor();
        #endregion

        #region Json
        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.WriteIndented = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });
        #endregion

        #region DB Connection
        services.AddDbContext<DigiDockMsDBContext>(options =>
            options.UseSqlServer(
                Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"),
                sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
            ));
        #endregion
        
        #region Hangfire
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"), new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));
        services.AddHangfireServer();
        #endregion

        #region Auto Mapper
        services.AddAutoMapper(typeof(MapperConfig));
        #endregion

        #region MediatR
        services.AddMediatR(typeof(CreateProductCommandHandler).Assembly);
        #endregion

        #region Jwt Authentication
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Secret)),
                ValidAudience = jwtConfig.Audience,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(2)
            };
        });
        #endregion

        #region Swagger With Auth
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Para Api Management", Version = "v1.0" });
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Para Management for IT Company",
                Description = "Enter JWT Bearer token **_only_**",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };
            c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, new string[] { } }
            });
        });
        #endregion
    }

    // Register services with Autofac
    public void ConfigureContainer(ContainerBuilder builder)
    {
        builder.RegisterModule(new AutofacBusinessModule());
    }
    public class AutofacBusinessModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Register Log Services
            builder.RegisterType<LogService>().AsSelf().SingleInstance();
            builder.RegisterType<LogQueueService>().AsSelf().SingleInstance();

            // Register Email Services
            builder.RegisterType<EmailService>().AsSelf().SingleInstance();
            builder.RegisterType<EmailQueueService>().AsSelf().SingleInstance();

            // Register UnitOfWork
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();

            // Register DbContext
            builder.RegisterType<DigiDockMsDBContext>().AsSelf().InstancePerLifetimeScope();

            // Register Token Service
            builder.RegisterType<TokenService>().As<ITokenService>().InstancePerLifetimeScope();
        }
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || true)
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DigiDock.Api v1"));
        }

        #region MiddleWares
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<RequestLoggingMiddleware>();
        #endregion

        app.UseHangfireDashboard();

        RecurringJob.AddOrUpdate<LogQueueService>(
            "process-log-messages", 
            service => service.ProcessQueue(), 
            Cron.Minutely);

        RecurringJob.AddOrUpdate<EmailQueueService>(
            "process-email-messages",
            service => service.ProcessQueue(),
            "*/3 * * * *"
        );

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}