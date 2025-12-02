using System.ComponentModel.DataAnnotations.Schema;

namespace payment.entity.BusinessEntities
{
    [Table("PACKAGE_TELCO")]
    public class PackageTelco
    {
        [Column("ID")]
        public long Id { get; set; }
        [Column("PACKAGE_NAME")]
        public string? PackageName { get; set; }
        [Column("TELCO")]
        public string? Telco { get; set; }
        [Column("DATA")]
        public decimal? Data { get; set; }
        [Column("AMT")]
        public string? Amount { get; set; }
        [Column("CREATE_DATE")]
        public string? CreateDate { get; set; }
        [Column("CREATE_USER")]
        public string? CreateUser { get; set; }
        [Column("EDIT_DATE")]
        public string? EditDate { get; set; }
        [Column("EDIT_USER")]
        public string? EditUser { get; set; }
        [Column("DATE_USE")]
        public int? DateUse { get; set; }
    }
}
