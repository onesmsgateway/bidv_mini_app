using System.Text.Json.Serialization;

namespace PaymentPackageTelco.entity.CommonModel
{
    public interface ISearchModel
    {
        [JsonPropertyName("page_index")]
        public int PageIndex { get; set; }

        [JsonPropertyName("page_size")]
        public int PageSize { get; set; }
    }

    public interface IFilterModel
    {
        [JsonPropertyName("duration_cycle")] //chuky: thoigian, 1day, 2day
        public DurationCycle? DateUse { get; set; }

        [JsonPropertyName("data_volume")]
        public DataVolume? DataVolume { get; set; }

        [JsonPropertyName("social_net")]
        public SocialNet? SocialNet { get; set; }
    }
    
    public enum DurationCycle
    {
        Time = 1,
        OneToSevenDays = 2,
        SevenToFiveteenDays = 3,
        FiveteenToThirtyDays = 4,
    }

    public enum DataVolume
    {
        SmallThan1GB = 1,
        From1GbTo2Gb = 2,
        From2GbTo4Gb = 3,
        From4GbTo8Gb = 4,
        MoreThan8Gb = 5
    }

    public enum SocialNet
    {
        Facebook = 1,
        Tiktok = 2,
        Youtube = 3,
    }
}
