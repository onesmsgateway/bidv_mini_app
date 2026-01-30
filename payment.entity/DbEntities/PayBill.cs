using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace payment.entity.DbEntities
{
    [Table("PAY_BILL")]
    public class PayBill : BaseEntity
    {
        [Column("TRANSACTION_ID")]
        public string TransactionId { get; set; }
        [Column("TRANSACTION_BIDV")]
        public string TransactionBidv { get; set; }
        [Column("TRANSACTION_DATE")]
        public string TransactionDate { get; set; }
        [Column("CUSTOMER_ID")]
        public string CustomerId { get; set; }
        [Column("SERVICE_ID")]
        public string ServiceId { get; set; }
        [Column("BILLNUMBER")]
        public string BillNumber { get; set; }
        [Column("VALUE")]
        public string Value { get; set; }
        [Column("CHECKSUM")]
        public string Checksum { get; set; }
    }
}
