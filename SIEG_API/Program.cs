using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SIEG_API.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var SIEGstring = builder.Configuration.GetConnectionString("SIEG");
builder.Services.AddDbContext<SIEGContext>(options => options.UseSqlServer(SIEGstring));

builder.Services.AddControllers();

string MyAllowOrigins = "AllowAny";
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: MyAllowOrigins,
        policy => policy.WithOrigins("*").WithHeaders("*").WithMethods("*"));
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
