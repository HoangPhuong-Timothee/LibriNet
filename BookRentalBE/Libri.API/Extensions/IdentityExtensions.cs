using Libri.API.DTOs.Response.Errors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Libri.API.Extensions
{
    public static class IdentityExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            //Authentication with JWT Bearer
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = config["JWT:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = config["JWT:Audience"],
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!)),
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents 
                    { 
                        OnAuthenticationFailed = context => 
                        { 
                            context.Response.StatusCode = 401; 
                            context.Response.ContentType = "application/json"; 
                            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }; 
                            var response = new APIResponse(401, "Xác thực không thành công");
                            var json = JsonSerializer.Serialize(response, options);
                            return context.Response.WriteAsync(json); 
                        }, 
                        OnTokenValidated = context => 
                        { 
                            return Task.CompletedTask; 
                        }, 
                        OnChallenge = async context => { 
                            context.Response.StatusCode = 401; 
                            context.Response.ContentType = "application/json"; 
                            var response = new APIResponse(401);
                            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                            var json = JsonSerializer.Serialize(response, options); 
                            await context.Response.WriteAsync(json); 
                        } 
                    };
                });

            //Athorization 
            services.AddAuthorization();

            return services;
        }
    }
}
