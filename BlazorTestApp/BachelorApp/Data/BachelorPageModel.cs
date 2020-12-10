﻿using BachelorApp.Data;
using System;
using System.IO;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BachelorApp.Data
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

        public async Task CollectSensorDataAsync()
        {
            if (!isListening)
            {
                isListening = true;
                Uri link = new Uri("wss://iotnet.teracom.dk/app?token=vnoTQQAAABFpb3RuZXQuY2liaWNvbS5ka45GqovHqK3PVCJypYVMsIw=");
                var ws = new ClientWebSocket();
                Console.WriteLine("Connecting ...");
                ws.ConnectAsync(link, CancellationToken.None).Wait();
                
                try
                {
                    using (var ms = new MemoryStream())
                    {
                        Console.WriteLine("Listening ...");
                        while (ws.State == WebSocketState.Open)
                        {
                            WebSocketReceiveResult result;
                            do
                            {
                                var messageBuffer = WebSocket.CreateClientBuffer(1024, 16);
                                result = await ws.ReceiveAsync(messageBuffer, CancellationToken.None);
                                ms.Write(messageBuffer.Array, messageBuffer.Offset, result.Count);
                            }
                            while (!result.EndOfMessage);
                            Console.WriteLine("Message received.");
                            if (result.MessageType == WebSocketMessageType.Text)
                            {
                                var msgString = Encoding.UTF8.GetString(ms.ToArray());
                                Console.WriteLine(msgString);
                            }
                            ms.Seek(0, SeekOrigin.Begin);
                            ms.Position = 0;
                        }
                    }
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Oopsie ...");
                }
                isListening = false;
            }
            
        }

        /*
                Payload payload = new Payload();
                payload.cmd = "tx";
                payload.EUI = "0004A30B0025A3D5";
                payload.port = 2;
                payload.data = "42";

                var request = JsonSerializer.Serialize(payload);
                byte[] bytes = Encoding.ASCII.GetBytes(request); 

                Console.WriteLine("Sending initial message ...");
                ws.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
                */

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
