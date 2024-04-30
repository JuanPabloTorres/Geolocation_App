using GeolocationAdsAPI.Context;
using GeolocationAdsAPI.Repositories;
using GeolocationAdsAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("AdsGeolocationConnectionString");

builder.Services.AddDbContext<GeolocationContext>(options =>
{
    options.UseSqlServer(connectionString, sqlServerOptions =>
    {
        sqlServerOptions.EnableRetryOnFailure(3); // Enable retry on failure, if needed
        sqlServerOptions.CommandTimeout(150); // Set the connection timeout to 30 seconds
        sqlServerOptions.UseRelationalNulls();
    });
});

// Configure CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("DefaultCorsPolicy", builder =>
//    {
//        builder.AllowAnyOrigin() // Replace with your .NET MAUI app's origin
//               .AllowAnyHeader()
//               .AllowAnyMethod();
//    });
//});

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = int.MaxValue; // Or another higher limit
});

// Add custom MIME types
builder.Services.Configure<StaticFileOptions>(options =>
{
    options.ContentTypeProvider = new FileExtensionContentTypeProvider(
        new Dictionary<string, string>
        {
                { ".m3u8", "application/vnd.apple.mpegurl" },
                { ".ts", "video/MP2T" }
            // Add any additional file types here
        });
});

builder.Services.AddTransient<IAdvertisementRepository, AdvertisementRepository>();

builder.Services.AddTransient<IGeolocationAdRepository, GeolocationAdRepository>();

builder.Services.AddTransient<ILoginRespository, LoginRepository>();

builder.Services.AddTransient<IUserRepository, UserRepository>();

builder.Services.AddTransient<IAppSettingRepository, AppSettingRepository>();

builder.Services.AddTransient<IForgotPasswordRepository, ForgotPasswordRepository>();

builder.Services.AddTransient<IAdvertisementSettingsRepository, AdvertisementSettingsRepository>();

builder.Services.AddTransient<IContentTypeRepository, ContentTypeRepository>();

builder.Services.AddTransient<ICaptureRepository, CaptureRepository>();

builder.Services.AddHostedService<CleanupService>();

// In ConfigureServices method
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //// The issuer (iss) and audience (aud) that the token is valid for.
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidAudience = builder.Configuration["Jwt:Issuer"],

            // Your signing key to validate the token's signature.
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            // Validate the token's lifetime
            ValidateLifetime = true,

            // Clock skew to account for server/client time differences (optional)
            ClockSkew = TimeSpan.Zero,

            // Additional security settings (customize as needed)
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseStaticFiles(); // Asegúrate de que puedes servir archivos estáticos, incluyendo .ts y .m3u8

// Enable CORS with the defined policy
//app.UseCors("DefaultCorsPolicy");

app.UseAuthentication(); // Make sure authentication comes before authorization

app.UseAuthorization();

//app.UseHttpsRedirection();

//app.UseAuthorization();

app.MapControllers();

app.Run();