using Autofac;
using Autofac.Extensions.DependencyInjection;
using DotnetNTierArchitecture.API.Modules;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ProductAPI.API.MiddleWares;
using ProductAPI.Repository;
using ProductAPI.Service.Authorization.Abstraction;
using ProductAPI.Service.Authorization.Concrete;
using ProductAPI.Service.Helpers;
using ProductAPI.Service.Mapping;
using ProductAPI.Service.Validations;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()  // Loglarý konsola da yazdýr
    .WriteTo.File(
        "logs/log-.txt",            // Log dosyasý yolu ve adý
        rollingInterval: RollingInterval.Day,  // Günlük loglama
        shared: true,                // Ayný dosyanýn birden fazla uygulama baþlatmasýnda kullanýlmasý
        rollOnFileSizeLimit: false   // Dosya boyutu limiti olduðunda döndürme yapmama
    )
    .CreateLogger();


builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region swagger islemleri
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

#endregion


// AutoMapper
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IJwtAuthenticationManager, JwtAuthenticationManager>();


// Fluent Validations
builder.Services.AddControllers().AddFluentValidation(x => { x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>(); });

//AppDbContext islemleri
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>() // AppDbContext sýnýfýnýz burada kullanýlacak
    .AddDefaultTokenProviders();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

//Buradan Autofac kullanarak yazdýðýmýz RepoServiceModule'ü dahil ediyoruz.
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoModuleService()));

// appsettings.json'daki AppSettings bölümünü DI (Dependency Injection) ile baðlýyoruz
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

var app = builder.Build();

app.UseSerilogRequestLogging();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomException();

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();
app.MapControllers();

app.Run();
