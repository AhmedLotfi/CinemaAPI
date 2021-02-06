using AutoMapper;
using CinemaAPI.Data;
using CinemaAPI.MappingProfiles;
using CinemaAPI.Services.Accounts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace CinemaAPI.Extensions
{
    public static class AppExtensions
    {
        public static void AddAutoMapperServie(this IServiceCollection services)
        {
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MainProfile());
            }).CreateMapper());
        }

        public static void AddAppDbContextService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CinemaDbContext>(option => option.UseSqlServer(configuration["ConnectionStrings:CinemaConnectionStr"]));
        }

        public static void AddAppJWTAuthenticaionService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidateIssuerSigningKey = true,
                     ValidIssuer = configuration["Tokens:Issuer"],
                     ValidAudience = configuration["Tokens:Issuer"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Tokens:Key"])),
                     ClockSkew = TimeSpan.Zero,
                 };
             });
        }


        public static void ResolveAppServices(this IServiceCollection services)
        {
            services.AddTransient<IAccountServices, AccountServices>();
        }
    }
}
