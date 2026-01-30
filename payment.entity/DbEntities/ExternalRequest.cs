using System.ComponentModel.DataAnnotations.Schema;

namespace payment.entity.DbEntities
{
    [Table("EXTERNAL_REQUEST")]
    public class ExternalRequest : BaseEntity
    {
        [Column("BILL_NUMBER")]
        public string? BillNumber { get; set; }

        [Column("CUSTOMER_ID")]
        public string? CustomerId { get; set; }

        [Column("SYSTEM")]
        public string? System { get; set; }
        [Column("SERVICE")]
        public string? Service { get; set; }
        [Column("SERVICE_ID")]
        public string? ServiceId { get; set; }
        [Column("CARRIER")]
        public string? Carrier { get; set; }
        [Column("CARRIER_ID")]
        public string? CarrierId { get; set; }
        [Column("PACKAGE")]
        public string? Package { get; set; }
        [Column("PACKAGE_ID")]
        public string? PackageId { get; set; }
        [Column("DATA_VOLUME")]
        public string? DataVolume { get; set; }
        [Column("VALUE")]
        public string? Value { get; set; }
        [Column("DISCOUNT_CODE")]
        public string? DiscountCode { get; set; }
        [Column("TOTAL_PAYMENT_AMOUNT")]
        public string? TotalPaymentAmount { get; set; }
        [Column("ISSUE_CORPORATE_INVOICE")]
        public string? IssueCoporateInvoice { get; set; }
        [Column("STATUS")]
        public string? Status { get; set; }
    }
}