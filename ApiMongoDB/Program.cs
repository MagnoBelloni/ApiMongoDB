using ApiMongoDB.Config;
using ApiMongoDB.Repository;
using ApiMongoDB.Services;
using ApiMongoDB.Services.Interfaces;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearer" }
            },
            Array.Empty<string>()
        }
    });
});

// MongoDB
builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));
builder.Services.AddSingleton<IDatabaseSettings>(sp => sp.GetRequiredService<IOptions<DatabaseSettings>>().Value);
builder.Services.AddSingleton(typeof(IMongoRepository<>), typeof(MongoRepository<>));

//Redis
builder.Services.AddDistributedRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetSection("Redis:ConnectionString").Value;
});

//Healthcheck
builder.Services.AddHealthChecks()
    .AddRedis(builder.Configuration.GetSection("Redis:ConnectionString").Value, tags: new string[] { "db", "" })
    .AddMongoDb($"{builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value}/{builder.Configuration.GetSection("DatabaseSettings:DatabaseName").Value}", name: "mongodb", tags: new string[] { "db", "data" });

builder.Services.AddHealthChecksUI(opt =>
{
    opt.SetEvaluationTimeInSeconds(15);
    opt.MaximumHistoryEntriesPerEndpoint(60);
    opt.SetApiMaxActiveRequests(1);

    opt.AddHealthCheckEndpoint("default api", "/health");
}).AddInMemoryStorage();

// Services
builder.Services.AddSingleton<NewsService>();
builder.Services.AddTransient<UploadService>();
builder.Services.AddTransient<VideoService>();
builder.Services.AddTransient<GalleryService>();

//Cache
//builder.Services.AddSingleton<IMemoryCache, MemoryCache>();
//builder.Services.AddSingleton<ICacheService, CacheMemoryService>();
builder.Services.AddSingleton<ICacheService, CacheRedisService>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MapperConfig));

// CORS
builder.Services.AddCors();

// JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII
                .GetBytes(builder.Configuration.GetSection("tokenManagement:secret").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

// Serilog
builder.Host.UseSerilog((ctx, lc) => lc
    .WriteTo.Console(LogEventLevel.Debug));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Healthcheck
app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
}).UseHealthChecksUI(h => h.UIPath = "/health-ui");

// CORS
app.UseCors(c =>
{
    c.AllowAnyOrigin();
    c.AllowAnyMethod();
    c.AllowAnyHeader();
});

// Servir Arquivos Estaticos na rota /medias
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "Medias")),
    RequestPath = "/medias"
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
