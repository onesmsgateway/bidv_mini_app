using PaymentPackageTelco.api.Common;
using System.Text.Json.Serialization;

public interface ISearchable
    {
    [JsonPropertyName("page_index")]
    public int PageIndex { get; set; }

    [JsonPropertyName("page_size")]
    public int PageSize { get; set; }
    }

    public interface IFilterable
    {
        [JsonPropertyName("date_use")]
        public DateUse? DateUse { get; set; }

        [JsonPropertyName("data_volume")]
        public DataVolume? DataVolume { get; set; }

        [JsonPropertyName("social_net")]
        public SocialNet? SocialNet { get; set; }
    }
    
  
