using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BachelorApp
{
    public class BachelorPageModel 
    {
        private readonly IHttpClientFactory _clientFactory;

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
        
        /*public async Task<String> OnGet()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,$"http://127.0.0.1:5000/"); //HttpMethod.Get ...

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                return response.ToString();
            }
            else
            {
                return "";
            }
        }*/
        
    }
}
