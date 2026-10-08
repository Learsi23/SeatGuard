using SeatGuard.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Retrieve the connection string from configuration (automatically includes User Secrets in Development)
var connectionString = builder.Configuration.GetConnectionString("SeatGuard")
    ?? throw new InvalidOperationException("Connection string 'SeatGuard' was not found in configuration.");

// Register AppDbContext with dependency injection using the custom database options extension
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSeatGuardDatabase(connectionString));

var app = builder.Build();

app.Run();