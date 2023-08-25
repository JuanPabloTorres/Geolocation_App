using GeolocationAdsAPI.Context;
using GeolocationAdsAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("AdsGeolocationConnectionString");

builder.Services.AddDbContext<GeolocationContext>(opt => opt.UseSqlServer(connectionString, sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

builder.Services.AddTransient<IAdvertisementRepository, AdvertisementRepository>();

builder.Services.AddTransient<IGeolocationAdRepository, GeolocationAdRepository>();

builder.Services.AddTransient<ILoginRespository, LoginRepository>();

builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IAppSettingRepository, AppSettingRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();