    using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Infrastructure.ORM
{
    public class CheckListDbContext:DbContext
    {
        public DbSet<Person> Person { get; set; }
        public DbSet<UserLogin> UserLogin { get; set; }
        public DbSet<Event> Event { get; set; }

        private readonly string _connectionString;

        public CheckListDbContext(DbContextOptions dbContextOptions, string connectionString):base(dbContextOptions)
        {
            _connectionString = connectionString;
            this.Database.EnsureCreated();
        }

        public CheckListDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(_connectionString);
        }
        public void InitDb()
        {
            var x = Database.GetPendingMigrations();
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(bool) || property.ClrType == typeof(Boolean))
                        property.SetValueConverter(new BoolToZeroOneConverter<Int16>());

                    else if (property.ClrType == typeof(Nullable<bool>) || property.ClrType == typeof(Nullable<Boolean>))
                        property.SetValueConverter(new BoolToZeroOneConverter<Nullable<Int16>>());
                }
                  
            } 
        }
    }
}
