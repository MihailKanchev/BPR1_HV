using BachelorApp.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
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

        public async Task CollectSensorData()
        {
            Uri link = new Uri("wss://iotnet.teracom.dk/app?token=vnoTQQAAABFpb3RuZXQuY2liaWNvbS5ka45GqovHqK3PVCJypYVMsIw=");
            var ws = new ClientWebSocket();
            Console.WriteLine("Connecting ...");
            ws.ConnectAsync(link, CancellationToken.None).Wait();

            /*Payload payload = new Payload();
            payload.cmd = "rx";
            payload.EUI = "0004A30B0025A3D5";
            payload.port = 2;
            payload.data = "42";

            var request = JsonSerializer.Serialize(payload);
            byte[] bytes = Encoding.ASCII.GetBytes(request);
            Console.WriteLine("Sending initial message ...");
            ws.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None).Wait();*/

            try
            {
                using (var ms = new MemoryStream())
                {
                    do
                    {
                        WebSocketReceiveResult result;
                        var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
                        Console.WriteLine("Receiving...");

                        result = await ws.ReceiveAsync(messageBuffer, CancellationToken.None);
                        ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
                        if (result.EndOfMessage && result.MessageType == WebSocketMessageType.Text)
                        {
                            Console.WriteLine("Deserializing...");
                            var msgString = Encoding.UTF8.GetString(ms.ToArray());
                            Console.WriteLine(msgString);

                            ms.Seek(0, SeekOrigin.Begin);
                            ms.Position = 0;
                        }
                    }
                    while (ws.State == WebSocketState.Open);
                    Console.WriteLine("Closing connection ...");
                }
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("[WS] Tried to receive message while already reading one.");
            }
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
