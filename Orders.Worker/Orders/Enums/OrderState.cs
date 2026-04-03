using System.Text.Json.Serialization;

namespace Orders.Worker.Orders.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderState
    {
        Created = 1,
        EmailSent = 2,
    }
}
