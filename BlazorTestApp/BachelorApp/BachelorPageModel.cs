using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task PutReadingItemAsync(Reading reading)
        {
            var client = _clientFactory.CreateClient();
            var readingItemJson = new StringContent(
                JsonSerializer.Serialize(reading));

            using var httpResponse =
                await client.PutAsync($"", readingItemJson); //needs endpoint

            httpResponse.EnsureSuccessStatusCode();
        }
        /*
        public async Task OnGet()  send a get as a "hello world?"
        {
            var request = new HttpRequestMessage(); //HttpMethod.Get ...

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                //success
            }
            else
            {
                //not success
            }
        }
        */
    }
}
