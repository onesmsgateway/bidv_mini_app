using System.ComponentModel.DataAnnotations.Schema;

namespace payment.entity.DbEntities
{
    [Table("CUSTOMER_ACCOUNT_INFO")]
    public class CustomerAccountInfo : BaseEntity
    {
        [Column("CUSTOMER_ID")]
        public string CustomerId { get; set; }
    }
}
