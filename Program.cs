using certeficates.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using certeficates.Interfaces;
// using certeficates.Services;
// using ProductCrudApi.Services;



var builder = WebApplication.CreateBuilder(args);
// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Logs to the console
builder.Logging.AddDebug();   // Logs to the debugger

// Add services to the container.

// Configure PostgreSQL database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IProductDetailService, ProductDetailService>();

// Add controllers and JSON options to handle reference loops
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve);

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else 
{
    app.UseHttpsRedirection();
}

// Other middleware configurations
// app.UseMiddleware<certeficates.Middleware.ExceptionMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();