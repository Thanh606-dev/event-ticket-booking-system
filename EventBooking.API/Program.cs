
using EventBooking.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký Controllers
builder.Services.AddControllers();

// Đăng ký OpenAPI
builder.Services.AddOpenApi();

// Kết nối SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"
        )
    )
);

var app = builder.Build();

// Cấu hình Swagger
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "EventBooking API v1"
        );
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/api/instance", (HttpContext context) =>
{
    return Results.Ok(new
    {
        InstancePort = context.Connection.LocalPort,
        Message = "EventBooking API is running"
    });
});
 
app.Run();
