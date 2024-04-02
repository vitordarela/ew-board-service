using Application.Services;
using Application.Services.Interfaces;
using Domain.Model.Mapping;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddSingleton(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var mongoClient = new MongoClient(configuration.GetValue<string>("MongoDB:ConnectionString"));
    var dbContextOptions =
        new DbContextOptionsBuilder<MongoDbContext>().UseMongoDB(mongoClient, configuration.GetValue<string>("MongoDB:DatabaseName"));

    return new MongoDbContext(dbContextOptions.Options);
});

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskProjectRepository, TaskProjectRepository>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskProjectService, TaskProjectService>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Board API", Version = "v1", Description = "API To Support Board" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
