using System.Data.Entity;
using GameStore.Dal.Entities;
using GameStore.Dal.Interfaces;

namespace GameStore.Dal
{
    public class DataBaseConnection : DbContext, IDataBaseConnection
    {
        public DataBaseConnection() : base("GameStore")
        {
            Database.SetInitializer<DataBaseConnection>(
                new DataBaseConnectionInitializer());

            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> PlatformTypes { get; set; }
        public DbSet<Publisher> Publisher { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Language> Languages { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UsersTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}