using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleStore.Core.Entities.MelhorEnvio
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class MelhorEnvioShipmentProduct
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("weight")]
        public decimal Weight { get; set; }

        [JsonProperty("insurance_value")]
        public decimal InsuranceValue { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }
    }
