namespace Fanex.Tarus.BetList.Business
{
    using Newtonsoft.Json;

    public class JsonConverterImpl : Result.ThirdParty.Services.JsonConverter
    {
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

        public string SerializeObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
