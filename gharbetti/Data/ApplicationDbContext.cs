using Gharbetti.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Gharbetti.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Floor> Floors { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomDetail> RoomDetails { get; set; }
        public DbSet<House> Houses { get; set; }
        public DbSet<HouseRoom> HouseRooms { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Complain> Complains { get; set; }
        public DbSet<CleanSchedule> CleanSchedules { get; set; }
        public DbSet<ExpenseType> ExpenseTypes { get; set; }
        public DbSet<PaymentMode> PaymentModes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<TransactionDetail> TransactionDetails { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<TenantMessage> TenantMessages { get; set; }
    }
}
