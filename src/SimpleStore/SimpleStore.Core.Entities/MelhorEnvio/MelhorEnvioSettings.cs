using System;

namespace SimpleStore.Core.Entities.MelhorEnvio
{
    public class MelhorEnvioSettings: StoreEntity
    {
        public bool IsSandbox { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime AccessTokenExpiration { get; set; }
        public string ZipCodeFrom { get; set; }
    }
}
