using System.Text;
using AutoMapper;
using FluentValidation;
using Iob.Bank.Domain.Data.Dtos;
using Iob.Bank.Domain.Interfaces.Infra;
using Iob.Bank.Domain.Interfaces.Services;
using Iob.Bank.Domain.Middlewares;
using Iob.Bank.Domain.Services;
using Iob.Bank.Domain.Services.Auth;
using Iob.Bank.Domain.Validators;
using Iob.Bank.Infra;
using Iob.Bank.Infra.AutoMapper;
using Iob.Bank.Infra.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Iob.Bank.Application.IoC;

public static class DependencyResolver
{
    public static IServiceCollection Resolve(this IServiceCollection services, IConfiguration configuration)
    {
        DbResolver(services, configuration);
        ConfigureCors(services);
        ConfigureServices(services);
        ConfigureValidators(services);
        JwtAuthentication(services, configuration);

        return services;
    }

    private static void DbResolver(IServiceCollection services, IConfiguration configuration)
    {
        var connection = configuration.GetConnectionString("IobBankConnectionString");

        services.AddDbContext<DataContext>(options =>
            options.UseMySql(connection, ServerVersion.AutoDetect(connection), optionsBuilder => optionsBuilder.MigrationsAssembly("Iob.Bank.Infra")));

        var mappingConfig = new MapperConfiguration(configuration =>
        {
            configuration.AllowNullCollections = true;
            configuration.AddProfile(new MappingProfile());
        });

        var mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddTransient<IDbModule, DbModule>();
    }

    private static void ConfigureCors(IServiceCollection services) =>
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IBankAccountService, BankAccountService>();
        services.AddTransient<IBankLaunchService, BankLaunchService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IUserService, UserService>();
    }

    public static void ConfigureMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }

    public static void ConfigureValidators(IServiceCollection services)
    {
        services.AddScoped<IValidator<BankLaunchDto>, BankLaunchDtoValidator>();
        services.AddScoped<IValidator<UserDto>, UserDtoValidator>();
        services.AddScoped<IValidator<UserAuthRequestDto>, UserAuthRequestDtoValidator>();
    }

    private static void JwtAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.ASCII.GetBytes(configuration.GetSection("AppSettings").GetSection("Jwt")["Key"] ?? string.Empty);
        services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
    }
}
