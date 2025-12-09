using System.ComponentModel.DataAnnotations.Schema;

namespace payment.entity.BusinessEntities
{
    [Table("PACKAGE_TELCO")]
    public partial class PackageTelco
    {
        [Column("ID")]
        public long Id { get; set; } //1-n
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


        [Column("IS_COMBO")]
        public char? IsCombo { get; set; }
        [Column("DATA_SOCIAL_NETWORK")]
        public string? DataSocialNetwork { get; set; }
        [Column("IS_DOMESTIC_PACKAGE")]
        public char? IsDomesticPackage { get; set; } //trongnuoc, quocte
        [Column("IS_FLASH_SALE")]
        public char? IsFlashSale { get; set; }
        [Column("FROM_TIME_FLASH_SALE")]
        public string? FromTimeFlashSale { get; set; }
        [Column("TO_TIME_FLASH_SALE")]
        public string? ToTimeFlashSale { get; set; }
        [Column("RESOURCE")]
        public string? Resouce { get; set; } //tai nguyen trong nuoc roaming data: 2GB data - Roaming 42 nước
        [Column("RMQT_COUNTRY")]
        public string? RmtqCountry { get; set; }
        [Column("NOTE")]
        public string? Note { get; set; }
    }
}
