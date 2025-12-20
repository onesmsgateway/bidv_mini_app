using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentPackageTelco.entity.DbEntities.BusinessEntities
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
        public int? Data { get; set; }
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
        public decimal? DateUse { get; set; }


        [Column("DESCRIPTION_DATA")]
        public string? DescriptionData { get; set; }
        [Column("DESCRIPTION_BONUS_FREE")]
        public string? DescriptionBonusFree { get; set; }
        [Column("DESCRIPTION_BONUS_UTILITY")]
        public string? DescriptionBonusUtility { get; set; }
        [Column("TOTAL_CAPACITY")]
        public int? TotalCapacity { get; set; }
        [Column("SELLING_PRICE")]
        public int? SellingPrice { get; set; }
        [Column("TOTAL_QUANTITY")]
        public int? TotalQuanlity { get; set; }
        [Column("PACKAGE_DATA_TYPE")]
        public int? PackageDataType { get; set; } // null,0: co dinh dung luong, 1: cộng dồn
        [Column("DESCRIPTION")]
        public string? Description { get; set; }
        [Column("PACKAGE_TYPE")]
        public int? PackageType { get; set; } //null, 0: topupdata, 1:commbo
        [Column("UTILITY_TYPE")]
        public int? UtilityType { get; set; } //null,0: normal, 1: social network utility
        [Column("DATA_SOCIAL_NETWORK")]
        public string? DataSocialNetwork { get; set; }
        [Column("AREA_PACKAGE")]
        public int? AreaPackage { get; set; } //null:0: trongnuoc, 1 quoc te, 2 khu vuc , 3 quoc gia
        [Column("RESOURCE")]
        public string? Resource { get; set; } //tai nguyen trong nuoc roaming data: 2GB data - Roaming 42 nước
        [Column("RMQT_COUNTRY")]
        public string? RmtqCountry { get; set; }
        [Column("NOTE")]
        public string? Note { get; set; }
        [Column("STATUS")]
        public string Status { get; set; } //new, hot, flash sale
    }
}
