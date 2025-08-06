using FlightBoard.Infrastructure.Repositories;
using FlightBoard.Infrastructure.Data;
using FlightBoard.Application.Services;
using Microsoft.EntityFrameworkCore;
using FlightBoard.Api.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json.Serialization;
using System.Text.Json;
using FluentValidation.AspNetCore;
using FluentValidation;
using FlightBoard.Application.Validators;
using FlightBoard.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseSqlite("Data Source=flightboard.db"));

// Repositories & Services
builder.Services.AddScoped<FlightRepository>();
builder.Services.AddScoped<FlightService>();

builder.Services.AddValidatorsFromAssemblyContaining<FlightValidator>();
builder.Services.AddFluentValidationAutoValidation();
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
        policy.WithOrigins("http://localhost:3000", "http://flightboard-frontend")
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

//app.UseHttpsRedirection();

app.MapHub<FlightsHub>("/flightsHub");

app.MapControllers();

if (app.Environment.IsProduction())
{
    app.Urls.Add("http://0.0.0.0:80");
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FlightDbContext>();
    db.Database.Migrate();

    // Seed data - if the table is empty.
    if (!db.Flights.Any())
    {
        db.Flights.AddRange(
            new Flight
            {
                FlightNumber = "AA123",
                Destination = "New York",
                DepartureTime = DateTime.UtcNow.AddHours(2),
                Status = "On Time",
                Gate = "A1"
            },
            new Flight
            {
                FlightNumber = "BA456",
                Destination = "London",
                DepartureTime = DateTime.UtcNow.AddHours(4),
                Status = "Delayed",
                Gate = "B2"
            }
        );
        db.SaveChanges();
    }
}

app.Run();
