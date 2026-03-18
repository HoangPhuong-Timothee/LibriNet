using Libri.API.DTOs.Response.Errors;
using Libri.BAL.Services;
using Libri.BAL.Services.Interfaces;
using Libri.DAL.DatabaseContext;
using Libri.DAL.Repositories;
using Libri.DAL.Repositories.Interfaces;
using Libri.DAL.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Libri.API.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            //DbContext
            services.AddDbContext<LibriContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection")!);
            });

            //Store procedure context
            services.AddTransient<CustomContext>();

            //Redis
            services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                try
                {
                    var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis")!);
                    options.AbortOnConnectFail = false;
                    return ConnectionMultiplexer.Connect(options);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Kết nối tới Redis thất bại: {e.Message}");
                    return null!;
                }
            });

            //Http context accessor
            services.AddHttpContextAccessor();

            //Auto mapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IAuthorRepository, AuthorRepository>();
            services.AddTransient<IBookRepository, BookRepository>();
            services.AddTransient<IBookStoreRepository, BookStoreRepository>();
            services.AddTransient<IDappperRepository, DapperRepository>();
            services.AddTransient<IDeliveryMethodRepository, DeliveryMethodRepository>();
            services.AddTransient<IUserAddressRepository, UserAddressRepository>();
            services.AddTransient<IUserAuthRepository, UserAuthRepository>();
            services.AddTransient<IGenreRepository, GenreRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IPublisherRepository, PublisherRepository>();
            services.AddTransient<IUnitOfMeasureRepository, UnitOfMeasureRepository>();

            //Unit of work
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            //Cache servie
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();

            //Services
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IAuthorService, AuthorService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IBookInventoryService, BookInventoryService>();
            services.AddTransient<IBookStoreService, BookStoreService>();
            services.AddTransient<IDeliveryMethodService, DeliveryMethodService>();
            services.AddTransient<IGenreSevice, GenreService>();
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddTransient<IPublisherService, PublisherService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUnitOfMeasureService, UnitOfMeasureService>();

            //Config errors response
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new APIValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });

            //CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200");
                });
            });

            return services;
        }
    }
}
