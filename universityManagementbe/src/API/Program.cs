using API.Authentication;
using API.Cors;
using API.Exceptions;
using API.RateLimiting;
using Application.Authentication.Login;
using Application.Authentication.Logout;
using Application.Authentication.Refresh;
using Infrastructure;
using Infrastructure.Initialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IRefreshService, RefreshService>();
builder.Services.AddScoped<ILogoutService, LogoutService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddProblemDetails();

// --------------------------------------------------
// CORS
// Angular: http://localhost:4200
// API:     http://localhost:5073
// --------------------------------------------------
builder.Services.AddFrontendCors(builder.Configuration);

// --------------------------------------------------
// JWT authentication
// --------------------------------------------------
builder.Services.AddJwtAuthentication(builder.Configuration);

// --------------------------------------------------
// Rate limiting
// --------------------------------------------------
builder.Services.AddApiRateLimiting(builder.Configuration);

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var startupInitializer = scope.ServiceProvider.GetRequiredService<StartupInitializer>();
    await startupInitializer.InitializeAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseCors(FrontendCorsExtensions.PolicyName);
// app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
