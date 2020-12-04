using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
            var readingItemJson = new StringContent(
                JsonSerializer.Serialize(reading));

            using var httpResponse =
                await client.PutAsync($"http://127.0.0.1:5000/", readingItemJson); //needs endpoint

            httpResponse.EnsureSuccessStatusCode();
            //return httpResponse.ReasonPhrase;
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            return responseString;
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
