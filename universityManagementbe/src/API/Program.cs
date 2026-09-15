using API.Exceptions;
using Application.Authentication.Login;
using Infrastructure;
using Infrastructure.Initialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
 
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
app.UseAuthorization();
app.MapControllers();
app.Run();