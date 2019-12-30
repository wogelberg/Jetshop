using Common;
using Microsoft.EntityFrameworkCore;

namespace DatabaseModels
{
    public class CarRentalContext : DbContext
    {
        public static string ConnectionString = @"OnlyTestDbForNow";
        public DbSet<CarRental> CarRentals { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlServer(ConnectionString);
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CarRental>().Property(x => x.Id);
            modelBuilder.Entity<CarRental>().Property(x => x.BookingNumber).HasMaxLength(30).IsRequired(false);
            modelBuilder.Entity<CarRental>().Property(x => x.CustomerSocialSecurityNumber).HasMaxLength(12).IsRequired(false);
            modelBuilder.Entity<CarRental>().Property(x => x.Rented).IsRequired(false);
            modelBuilder.Entity<CarRental>().Property(x => x.Returned).IsRequired(false);
            modelBuilder.Entity<CarRental>().Property(x => x.CarMilageAtRentInKm).HasColumnType("decimal(16, 4)").IsRequired(false);
            modelBuilder.Entity<CarRental>().Property(x => x.CarMilageAtReturnInKm).HasColumnType("decimal(16, 4)").IsRequired(false);
            modelBuilder.Entity<CarRental>().Property(x => x.CarCategory).IsRequired(false);

            modelBuilder.Entity<CarRental>().HasKey(x => x.Id);
            modelBuilder.Entity<CarRental>().HasIndex(x => new { x.BookingNumber });
        }
    }
}
