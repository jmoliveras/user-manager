using Microsoft.EntityFrameworkCore;
using UserManager.Domain.Interfaces;
using UserManager.Infrastructure.Data;
using UserManager.Infrastructure.Repositories;
using UserManager.API.Polly;
using UserManager.Application.Services;
using UserManager.Application.Queries.Handlers;
using MediatR;
using UserManager.Application.DTO;
using UserManager.Application.Queries.Requests;
using UserManager.Application.Mapping;
using UserManager.Application.Commands.Requests;
using UserManager.Application.Commands.Handlers;
using UserManager.Application.Settings;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
           options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetAllUsersQueryHandler).Assembly));
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

var downloadServiceSettings = builder.Configuration.GetSection("UserDownloadService").Get<UserDownloadServiceSettings>();
builder.Services.Configure<UserDownloadServiceSettings>(builder.Configuration.GetSection("UserDownloadServic"));

builder.Services.AddHttpClient("UserApiClient", client =>
{
    client.BaseAddress = new Uri(downloadServiceSettings!.UserApiUrl);
})
    .AddPolicyHandler(PollyPolicies.GetRetryPolicy())
    .AddPolicyHandler(PollyPolicies.GetCircuitBreakerPolicy());

builder.Services.AddTransient<IRequestHandler<GetAllUsersQuery, List<UserDto>>, GetAllUsersQueryHandler>();
builder.Services.AddTransient<IRequestHandler<UpdateUserCommand, bool>, UpdateUserCommandHandler>();
builder.Services.AddTransient<IRequestHandler<DeleteUserCommand, bool>, DeleteUserCommandHandler>();

builder.Services.AddHostedService<UserDownloadService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "User Management API",
        Description = "An API to manage users"
    });    
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "User Management API v1");
        c.RoutePrefix = string.Empty; 
    });
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
