using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MunchrBackendV2.Models;

namespace MunchrBackendV2.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base (options){}
        public DbSet<BusinessModel> Business { get; set; }
        public DbSet<UserModel> User { get; set; }
        public DbSet<ReviewModel> Review { get; set; }
        public DbSet<FavoritesModel> FavoriteBusinesses { get; set; }
        public DbSet<MenuItemModel> MenuItems { get; set; }
    }
}