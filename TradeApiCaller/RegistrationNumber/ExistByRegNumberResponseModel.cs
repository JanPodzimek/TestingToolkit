using System.Text.Json.Serialization;

namespace TradeApiCaller.RegistrationNumber
{
    public class ExistByRegNumberResponseModel
    {
        [JsonPropertyName("value")]
        public bool Value { get; set; }
    }
}
