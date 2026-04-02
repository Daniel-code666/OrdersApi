using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Order.Domain.Entitites.Enums.OrdersEnum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderState
    {
        Created = 1,
        EmailSent = 2,
    }
}
