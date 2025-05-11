﻿using BookingApp.Application.DTOs;
using BookingApp.Domain.Entities;

namespace BookingApp.Application.Extensions
{
    public static class UserExtensions
    {
        public static UserDTO ToDto(this User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Email = user.Email,
            }; 
        }

        public static IEnumerable<UserDTO> ToDtoList(this IEnumerable<User> users)
        {
            return users.Select(u => u.ToDto());
        }
    }
}
