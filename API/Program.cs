using Internal.API.Middleware;
using Internal.API.Seeders;
using Internal.Application;
using Internal.Domain.Entities;
using Internal.Infrastructure;
using Internal.Infrastructure.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // extensions
    builder.Services
        .AddAplication()
        .AddInfrastructure(builder.Configuration);

    // this should be a extension method
    builder.Services.AddIdentityCore<User>()//(opt => { opt.Lockout.DefaultLockoutTimeSpan = })
        .AddRoles<IdentityRole<int>>()
        .AddEntityFrameworkStores<MovieDbContext>()
        .AddDefaultTokenProviders();

    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSettings:TokenKey"])),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        }
    );

    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(builder =>
        {
            // Replace the below origins with the appropriate origins for your frontend application.
            // "*" allows any origin. You should replace it with your frontend domain(s) for added security.
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
    });
};

var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .CreateLogger();

    // middlewares
    app.UseMiddleware<ErrorHandlingMiddleware>();
    app.UseMiddleware<RequestTimingMiddleware>();

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.UseCors();
    app.UseStaticFiles();

    var serviceScope = app.Services.CreateScope();

    var movieDbContext = serviceScope.ServiceProvider.GetService<MovieDbContext>();
    var userManager = serviceScope.ServiceProvider.GetService(typeof(UserManager<User>)) as UserManager<User>;
    var roleManager = serviceScope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole<int>>)) as RoleManager<IdentityRole<int>>;

    DbInitializer? init = new(userManager, movieDbContext, roleManager);

    init.InitializeAsync().GetAwaiter().GetResult();

    app.Run();
};

