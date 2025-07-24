using FlightBoard.Infrastructure.Repositories;
using FlightBoard.Infrastructure.Data;
using FlightBoard.Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseSqlite("Data Source=flightboard.db"));

// Repositories & Services
builder.Services.AddScoped<FlightRepository>();
builder.Services.AddScoped<FlightService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
