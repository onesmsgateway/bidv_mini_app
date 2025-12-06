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
        public virtual DbSet<PayBill> PayBills { get; set; } = null!;
        public virtual DbSet<PackageTelco> PackageTelcos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("GWTEST")
                .UseCollation("USING_NLS_COMP");

            #region CUSTOMER_ACCOUNT_INFO
            modelBuilder.Entity<CustomerAccountInfo>(entity =>
            {
                entity.ToTable("CUSTOMER_ACCOUNT_INFO");

                entity.Property(e => e.Id)
                      .HasPrecision(18)
                      .HasColumnName("ID")
                      .HasDefaultValueSql("\"GWTEST\".\"CUSTOMER_ACCOUNT_INFO_SEQ\".nextval ");

                entity.Property(e => e.CustomerId)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("CUSTOMER_ID");
      
                entity.Property(e => e.Fullname)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("FULLNAME");

                entity.Property(e => e.CreateDate)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("CREATEDATE");
            });
            #endregion
            
            #region EXTERNAL_REQUEST
            modelBuilder.Entity<ExternalRequest>(entity =>
            {
                entity.ToTable("EXTERNAL_REQUEST");

                entity.Property(e => e.Id)
                      .HasPrecision(18)
                      .HasColumnName("ID")
                      .HasDefaultValueSql("\"GWTEST\".\"EXTERNAL_REQUEST_SEQ\".nextval ");

                entity.Property(e => e.BillNumber)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("BILL_NUMBER");

                entity.Property(e => e.System)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("SYSTEM");

                entity.Property(e => e.CustomerId)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("CUSTOMER_ID");

                entity.Property(e => e.Service)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("SERVICE");

                entity.Property(e => e.ServiceId)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("SERVICE_ID");

                entity.Property(e => e.Carrier)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("CARRIER");

                entity.Property(e => e.CarrierId)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("CARRIER_ID");

                entity.Property(e => e.Package)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("PACKAGE");

                entity.Property(e => e.PackageId)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("PACKAGE_ID");


                entity.Property(e => e.DataVolume)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("DATA_VOLUME");

                entity.Property(e => e.Value)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("VALUE");

                entity.Property(e => e.Status)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .IsRequired(false)
                    .HasColumnName("STATUS");

                entity.Property(e => e.DiscountCode)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("DISCOUNT_CODE");

                entity.Property(e => e.TotalPaymentAmount)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("TOTAL_PAYMENT_AMOUNT");

                entity.Property(e => e.IssueCoporateInvoice)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("ISSUE_CORPORATE_INVOICE");

                entity.Property(e => e.CreateDate)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("CREATEDATE");

            });
            #endregion

            #region EXTERNAL_REQUEST
            modelBuilder.Entity<PayBill>(entity =>
            {
                entity.ToTable("PAY_BILL");

                entity.Property(e => e.Id)
                      .HasPrecision(18)
                      .HasColumnName("ID")
                      .HasDefaultValueSql("\"GWTEST\".\"PAY_BILL_SEQ\".nextval ");

                entity.Property(e => e.TransactionId)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("TRANSACTION_ID");

                entity.Property(e => e.TransactionBidv)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("TRANSACTION_BIDV");

                entity.Property(e => e.TransactionDate)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("TRANSACTION_DATE");

                entity.Property(e => e.CustomerId)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("CUSTOMER_ID");

                entity.Property(e => e.ServiceId)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("SERVICE_ID");

                entity.Property(e => e.BillNumber)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("BILLNUMBER");

                entity.Property(e => e.Value)
                      .HasMaxLength(300)
                      .IsUnicode(false)
                      .IsRequired(false)
                      .HasColumnName("VALUE");

                entity.Property(e => e.Checksum)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("CHECKSUM");

                entity.Property(e => e.CreateDate)
                     .HasMaxLength(300)
                     .IsUnicode(false)
                     .IsRequired(false)
                     .HasColumnName("CREATEDATE");
            });
            #endregion

            modelBuilder.HasSequence("CUSTOMER_ACCOUNT_INFO_SEQ");
            modelBuilder.HasSequence("EXTERNAL_REQUEST_SEQ");
            modelBuilder.HasSequence("PAY_BILL_SEQ");
        }
    }
}