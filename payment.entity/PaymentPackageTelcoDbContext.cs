using Microsoft.EntityFrameworkCore;
using payment.entity.BusinessEntities;
using payment.entity.DbEntities;

namespace payment.entity
{
    public class PaymentPackageTelcoDbContext : DbContext
    {
        public PaymentPackageTelcoDbContext()
        {
        }

        public PaymentPackageTelcoDbContext(DbContextOptions<PaymentPackageTelcoDbContext> options) : base(options)
        {
        }

        public virtual DbSet<ExternalRequest> ExternalRequests { get; set; } = null!;
        public virtual DbSet<CustomerAccountInfo> CustomerAccountInfos { get; set; } = null!;
        public virtual DbSet<InstantPaymentNotification> InstantPaymentNotifications { get; set; } = null!;
        public virtual DbSet<PackageTelco> PackageTelcos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }

    public static class DbInitializer
    {
        public static void Initialize(PaymentPackageTelcoDbContext context)
        {
            context.Database.EnsureCreated();
            context.SaveChanges();
        }
    }
}
