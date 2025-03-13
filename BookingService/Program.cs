using Microsoft.EntityFrameworkCore;
using BookingApp.Infrastructure.DB;
using BookingApp.Infrastructure.Repositories;
using BookingApp.Domain.Interfaces;
using BookingApp.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<BookingDbContext>(options => options.UseInMemoryDatabase("BookingDB"));

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();

builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<HotelService>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseDeveloperExceptionPage();
    // Использование страницы разработки.

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Booking App API v1");
        c.RoutePrefix = string.Empty;
    });
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
