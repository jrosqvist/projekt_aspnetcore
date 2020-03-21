using System;
using System.Collections.Generic;
using System.Text;
using asp_projekt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace asp_projekt.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Room { get; set; }
        public DbSet<HotelBooking> HotelBooking { get; set; }
        public DbSet<RestaurantBooking> Restaurantbooking { get; set; }
        public DbSet<Dish> Dish { get; set; }
        public DbSet<Drink> Drink { get; set; }



    }
}
