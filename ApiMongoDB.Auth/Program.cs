using ApiMongoDB.Auth.Models;
using ApiMongoDB.Auth.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Services
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<UserService>();

// Settings
builder.Services.Configure<TokenManagement>(builder.Configuration.GetSection("tokenManagement"));

// CORS
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS
app.UseCors(c =>
{
    c.AllowAnyHeader();
    c.AllowAnyMethod();
    c.AllowAnyOrigin();

});

app.UseAuthorization();

app.MapControllers();

app.Run();
