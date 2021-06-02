using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SimpleStore.Core.Entities.Carts;
using SimpleStore.Core.Entities.MelhorEnvio;
using SimpleStore.Core.Entities.Shipping;
using SimpleStore.Core.Services.Shipping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SimpleStore.Core.Services.MelhorEnvio
{
    public class MelhorEnvioService: IShippingMethod
    {
        private const string MELHOR_ENVIO_URL = "https://melhorenvio.com.br";
        private const string MELHOR_ENVIO_URL_SANDBOX = "https://sandbox.melhorenvio.com.br";
        private const string CEP_URL = "https://viacep.com.br/";

        private readonly IMelhorEnvioSettingsService _settingsService;

        public MelhorEnvioService(IMelhorEnvioSettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        #region Methods
        public async Task<ICollection<ShippingOption>> GetShippingOptions(Cart cart)
        {
            var options = new List<ShippingOption>();

            var shipment = new MelhorEnvioShipment();

            var result = await PostRequest("/api/v2/me/shipment/calculate", shipment);
            var resultObject = JObject.Parse("{ \"result\": " + result + "}");
            var shippingOptionsList = resultObject["result"].Children();

            // Show ship options
            foreach (var option in shippingOptionsList)
            {
                if (!string.IsNullOrEmpty(option["error"]?.ToString()))
                    continue;
                var description = $"{option["custom_delivery_range"]["min"]}-{option["custom_delivery_range"]["max"]} dias úteis.";
                var shippingValue = Convert.ToDecimal(option["custom_price"]);
                var shippingOption = new ShippingOption
                {
                    Name = option["company"]["name"].ToString() + " - " + option["name"].ToString(),
                    Description = description,
                    Value = shippingValue
                };

                options.Add(shippingOption);
            }

            return options;
        }

        public string GetCepInfo(string cep)
        {
            var client = new RestClient($"{CEP_URL}/ws/{cep}/json");
            var request = new RestRequest(Method.GET);

            var response = client.Execute(request);

            // Validate response
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return response.Content;
        }

        public async Task<string> PostRequest(string endpoint, object content)
        {
            var client = await CreateClient(endpoint);
            var request = await CreateRequest(Method.POST);

            request.AddParameter("application/json", JsonConvert.SerializeObject(content), ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            // Validate response
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return null;
            }

            return response.Content;
        }

        public async Task<RestRequest> CreateRequest(Method method)
        {
            var settings = await GetSettings();

            // Validate Access Token
            if (settings.AccessTokenExpiration < DateTime.Now)
                await RefreshToken();

            var request = new RestRequest(method);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", $"Bearer {settings.AccessToken}");

            return request;
        }

        public async Task<RestClient> CreateClient(string endpoint)
        {
            var settings = await GetSettings();

            // Create base Url
            var baseUrl = settings.IsSandbox ? MELHOR_ENVIO_URL_SANDBOX : MELHOR_ENVIO_URL;

            // Fix endpoint
            if (endpoint.First() != '/')
                endpoint = "/" + endpoint;

            var client = new RestClient($"{baseUrl}{endpoint}");

            return client;
        }

        public async Task RefreshToken()
        {
            var settings = await GetSettings();

            var client = await CreateClient("/oauth/token");
            client.Timeout = -1;

            var request = new RestRequest(Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("refresh_token", settings.RefreshToken);
            request.AddParameter("client_id", settings.ClientId);
            request.AddParameter("client_secret", settings.ClientSecret);

            IRestResponse response = client.Execute(request);

            // Validate response
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new UnauthorizedAccessException("Acesso a API do Melhor Envio negado.");
            }

            // Save tokens
            var contentObject = JObject.Parse(response.Content);
            var accessToken = contentObject.Value<string>("access_token");
            var refreshToken = contentObject.Value<string>("refresh_token");
            var expiresIn = contentObject.Value<int>("expires_in");

            settings.AccessToken = accessToken;
            settings.RefreshToken = refreshToken;

            // Set expire date to 1 day before
            settings.AccessTokenExpiration = DateTime.Now.AddSeconds(expiresIn).AddDays(-1);

            await _settingsService.Update(settings);
        }

        public Task<MelhorEnvioSettings> GetSettings()
        {
            return _settingsService.GetByCurrentStore();
        }

        #endregion
    }
}
