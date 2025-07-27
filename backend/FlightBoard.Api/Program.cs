using FlightBoard.Infrastructure.Repositories;
using FlightBoard.Infrastructure.Data;
using FlightBoard.Application.Services;
using Microsoft.EntityFrameworkCore;
using FlightBoard.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Serialization;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseSqlite("Data Source=flightboard.db"));

// Repositories & Services
builder.Services.AddScoped<FlightRepository>();
builder.Services.AddScoped<FlightService>();

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonDateTimeUtcConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHub<FlightsHub>("/flightsHub");

app.MapControllers();

app.Run();
