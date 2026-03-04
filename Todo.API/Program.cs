using Microsoft.AspNetCore.Authentication;
using Todo.API.Auth;
using Todo.Application.Services.Implementations;
using Todo.Application.Services.Interfaces;
using Todo.Domain.Interfaces;
using Todo.Infrastructure.Data;
using Todo.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<AppDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<ITokenStore, InMemoryTokenStore>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<Func<string, string>>(sp =>
{
    var tokenStore = sp.GetRequiredService<ITokenStore>();
    return userId => tokenStore.CreateToken(userId);
});

builder.Services
    .AddAuthentication("Bearer")
    .AddScheme<AuthenticationSchemeOptions, BearerTokenAuthenticationHandler>("Bearer", _ => { });
builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
