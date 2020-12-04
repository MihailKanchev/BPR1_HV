using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

            //HttpContent httpContent = new StringContent(JsonSerializer.Serialize<Reading>(reading), Encoding.UTF8, "application/json");

            //HttpResponseMessage response = await client.PutAsync($"http://127.0.0.1:5000/", httpContent);

            using var httpResponse = await client.PutAsync($"http://127.0.0.1:5000/", readingItemJson); //needs endpoint
            
            var content = httpResponse.Content.ReadAsStringAsync().Result;

            //var answer = JsonSerializer.Deserialize<String>(content);

            return content;

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
