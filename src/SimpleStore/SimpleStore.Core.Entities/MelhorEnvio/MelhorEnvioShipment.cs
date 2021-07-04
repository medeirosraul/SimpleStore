using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Entities.MelhorEnvio
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MelhorEnvioShipment
    {
        [JsonProperty("from")]
        public MelhorEnvioShipmentAddress From { get; set; }

        [JsonProperty("to")]
        public MelhorEnvioShipmentAddress To { get; set; }

        [JsonProperty("products")]
        public ICollection<MelhorEnvioShipmentProduct> Products { get; set; }

        [JsonProperty("options")]
        public MelhorEnvioShipmentOptions Options { get; set; }

        [JsonProperty("services")]
        public string Services { get; set; }
    }
}
