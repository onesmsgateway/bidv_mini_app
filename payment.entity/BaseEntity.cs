using System.ComponentModel.DataAnnotations.Schema;

namespace payment.entity
{
    public class BaseEntity
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("CREATEDATE")]
        public string? CreateDate { get; set; } = DateTime.UtcNow.ToString();
    }
}
