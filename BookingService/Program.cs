using Microsoft.EntityFrameworkCore;
using BookingApp.Infrastructure.DB;
using BookingApp.Infrastructure.Repositories;
using BookingApp.Domain.Interfaces;
using BookingApp.Application.Services;
using BookingApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddDbContext<BookingDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<BookingDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<BookingApp.Application.Services.HotelService>();
builder.Services.AddScoped<BookingApp.Application.Services.RoomService>();
builder.Services.AddScoped<UserService>();

builder.Services.AddOpenApi();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SigningKey SigningKey SigningKey SigningKey SigningKey SigningKey SigningKey SigningKey "]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("admin"));
});

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000");
                      });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BookingDbContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await DbInitializer.Initialize(context, userManager, roleManager);
}

app.UseStatusCodePages(context =>
{
    var response = context.HttpContext.Response;

    if (response.StatusCode == 404 && context.HttpContext.User.Identity?.IsAuthenticated == false)
    {
        response.StatusCode = 401;
        return response.WriteAsync("Недоступно. Необходима авторизация.");
    }

    return Task.CompletedTask;
});

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

app.UseAuthorization();

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
