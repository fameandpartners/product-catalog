using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Fame.Service.Clients.Interfaces;
using Fame.Service.DTO;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Fame.Common;

namespace Fame.Service.Clients
{
    public class SpreeClient : ISpreeClient
    {
        private readonly HttpClient _httpClient;

        public SpreeClient(IOptions<FameConfig> fameConfig)
        {
            _httpClient = new HttpClient();

            var uri = new Uri(fameConfig.Value.Spree.BaseUrl);
            _httpClient.BaseAddress = uri;


            if (!string.IsNullOrWhiteSpace(uri.UserInfo))
            {
                var byteArray = Encoding.ASCII.GetBytes(uri.UserInfo);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            }

            _httpClient.Timeout = TimeSpan.FromSeconds(120);
        }

        public async Task ImportProduct(ICollection<SpreeImport> data)
        {
            const string url = "/api/v1/product_upload";

            var stringPayload = JsonConvert.SerializeObject(data);
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var response = await _httpClient.PutAsync(url, httpContent))
            {
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
