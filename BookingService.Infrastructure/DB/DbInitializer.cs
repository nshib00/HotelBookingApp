using BookingApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Infrastructure.DB
{
    public static class DbInitializer
    {
        public static async Task Initialize(BookingDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            await context.Database.MigrateAsync();

            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                await roleManager.CreateAsync(new IdentityRole("user"));
            }

            if (!userManager.Users.Any())
            {
                var adminUser = new User
                {
                    UserName = "admin",
                    FirstName = "admin",
                    LastName = "adminov",
                    PhoneNumber = "+78005553535",
                    BirthDate = DateOnly.Parse("12/03/1989"),
                    Email = "admin@example.com",
                };
                var userCreationResult = await userManager.CreateAsync(adminUser, "A@dmin_User123");
                if (userCreationResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "admin");
                }

                var normalUser = new User
                {
                    UserName = "user",
                    FirstName = "user",
                    LastName = "obychniy",
                    PhoneNumber = "+79121234567",
                    BirthDate = DateOnly.Parse("22/05/1999"),
                    Email = "user@example.com"
                };
                var normalUserCreationResult = await userManager.CreateAsync(normalUser, "N@rmal_User@123");
                if (normalUserCreationResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "user");

                    var normalUserId = await userManager.GetUserIdAsync(normalUser);

                    if (!context.Hotels.Any() && !context.Rooms.Any())
                    {
                        var rooms1 = new List<Room>
                        {
                            new Room { Name = "Стандарт с большой кроватью", Description = "Двуспальная кровать", Quantity = 20, Price = 6490 },
                            new Room { Name = "Стандарт с раздельными кроватями", Description = "2 двуспальные кровати (2шт)", Quantity = 25, Price = 6490 },
                            new Room { Name = "Cупериор", Description = "2 односпальные ・ Либо 1 двуспальная кровать", Quantity = 17, Price = 7090 },
                            new Room { Name = "Джуниор Сюит", Description = "King-size кровать", Quantity = 6, Price = 10090 },
                        };
                        var rooms2 = new List<Room>
                        {
                            new Room { Name = "Valo Comfort 3*", Description = "King-size кровать ・ 2 односпальные кровати (2шт)", Quantity = 15, Price = 4200 },
                            new Room { Name = "Valo Junior Suite 3*", Description = "King-size кровать ・ 2 односпальные кровати (2шт)", Quantity = 23, Price = 4900 },
                            new Room { Name = "Valo Suite 3*", Description = "King-size кровать ・ 2 односпальные кровати (2шт)", Quantity = 11, Price = 7500 },
                        };

                        var hotel1 = new Hotel
                        {
                            Name = "Отель Измайлово Альфа",
                            Description = "Здесь должно быть описание отеля",
                            Address = "Измайлово район, г. Москва, Измайловское шоссе, д. 71 корп. А",
                            City = "Москва",
                            StarRating = 4,
                            Rooms = rooms1,
                            Services = new List<HotelService> {
                                new HotelService { Name = "Wi-Fi" },
                                new HotelService { Name = "Кондиционер" },
                                new HotelService { Name = "Можно с питомцами" },
                            },
                        };

                        var hotel2 = new Hotel
                        {
                            Name = "Valo Hotel Сity",
                            Description = "6.7 км от центра, 309 м от метро Бухарестская",
                            Address = "Фрунзенский район, г. Санкт-Петербург, ул. Салова, д. 61",
                            City = "Санкт-Петербург",
                            StarRating = 3,
                            Rooms = rooms2,
                            Services = new List<HotelService>
                            {
                                new HotelService { Name = "Бесплатный Wi-Fi" },
                                new HotelService { Name = "Крытый бассейн" },
                                new HotelService { Name = "Сауна" },
                                new HotelService { Name = "Салон красоты" },
                                new HotelService { Name = "Фитнес" },
                            },
                        };
                        context.Hotels.Add(hotel1);
                        context.Hotels.Add(hotel2);
                        context.Rooms.AddRange(rooms1);
                        context.Rooms.AddRange(rooms2);
                        await context.SaveChangesAsync();
                    }

                    if (!context.Bookings.Any())
                    {
                        var booking1 = new Booking
                        {
                            RoomId = 2,
                            UserId = normalUserId,
                            DateFrom = new DateTime(2025, 3, 20, 14, 0, 0, DateTimeKind.Utc),
                            DateTo = new DateTime(2025, 4, 4, 12, 0, 0, DateTimeKind.Utc),
                        };
                        var booking2 = new Booking
                        {
                            RoomId = 3,
                            UserId = normalUserId,
                            DateFrom = new DateTime(2025, 5, 16, 14, 0, 0, DateTimeKind.Utc),
                            DateTo = new DateTime(2025, 5, 27, 12, 0, 0, DateTimeKind.Utc),
                        };
                        var booking3 = new Booking
                        {
                            RoomId = 7,
                            UserId = normalUserId,
                            DateFrom = new DateTime(2026, 1, 9, 14, 0, 0, DateTimeKind.Utc),
                            DateTo = new DateTime(2026, 1, 17, 12, 0, 0, DateTimeKind.Utc),
                        };

                        context.Bookings.Add(booking1);
                        context.Bookings.Add(booking2);
                        context.Bookings.Add(booking3);
                        await context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
