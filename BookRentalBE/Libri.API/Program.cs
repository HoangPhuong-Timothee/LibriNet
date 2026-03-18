using Libri.API.Extensions;
using Libri.API.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//var logger = new LoggerConfiguration()
//        .ReadFrom.Configuration(builder.Configuration)
//        .CreateLogger();
//builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog((context, services, config) =>
    config
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext()
);

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerService();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionHandler>();

// Configure the HTTP request pipeline.
app.UseSwaggerDocumentation();

// app.UseSwaggerDocumentation();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseSerilogRequestLogging();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

// ######################## THIS IS FOR AUTO SEED DATA WHEN RUN THE PROJECT##################
//using var scope = app.Services.CreateScope();
//var services = scope.ServiceProvider;
//var dbContext = services.GetRequiredService<BookRentalContext>();
//var logger = services.GetRequiredService<ILogger<Program>>();
//try
//{
//    await dbContext.Database.MigrateAsync();
//    await BookRentalContextSeed.SeedAsync(dbContext);
//}
//catch (Exception e)
//{
//    logger.LogError(e, "An error occured during migrations.");
//}
// ######################## THIS IS FOR AUTO SEED DATA WHEN RUN THE PROJECT##################

app.Run();
