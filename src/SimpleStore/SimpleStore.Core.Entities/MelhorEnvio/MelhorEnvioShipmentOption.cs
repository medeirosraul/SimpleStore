using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Entities.MelhorEnvio
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MelhorEnvioShipmentOptions
    {
        [JsonProperty("receipt")]
        public bool Receipt { get; set; }

        [JsonProperty("own_hand")]
        public bool OwnHand { get; set; }
    }
}
