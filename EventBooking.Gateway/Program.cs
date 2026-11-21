
var builder = WebApplication.CreateBuilder(args);

// Dang ky YARP Reverse Proxy
builder.Services
    .AddReverseProxy()
    .LoadFromConfig(
        builder.Configuration.GetSection("ReverseProxy")
    );

var app = builder.Build();

// Kiem tra Gateway dang hoat dong
app.MapGet("/", () =>
    Results.Ok(new
    {
        Message = "EventBooking Gateway is running",
        Status = "OK"
    })
);

// Chuyen tiep request den cac API instance
app.MapReverseProxy();

app.Run();
