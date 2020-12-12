using BachelorApp.Data;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BachelorApp.Interfaces
{
    public class BachelorPageModel : IBachelorPageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public bool isListening = false;

        public BachelorPageModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<String> PutReadingItemAsync(Reading reading)
        {
            var client = _clientFactory.CreateClient();
            var readingItemJson = new StringContent(JsonSerializer.Serialize(reading));

            using var httpResponse = await client.PutAsync($"http://127.0.0.1:5000/", readingItemJson);
            var content = httpResponse.Content.ReadAsStringAsync().Result;
            var answer = JsonSerializer.Deserialize<String>(content);

            return answer;
        }
    }
}
