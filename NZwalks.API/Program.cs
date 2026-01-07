using Microsoft.EntityFrameworkCore;
using NZwalks.API.Data;
using NZwalks.API.Mappings;
using NZwalks.API.Repository;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<NZwalksDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("NZwalksConnectionString")));
builder.Services.AddAutoMapper(cfg => { }, typeof(AutoMapperProfiles).Assembly);
builder.Services.AddScoped<IRegionRepository, SQLRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
