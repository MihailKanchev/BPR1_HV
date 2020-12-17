using BachelorApp.Data;
using BachelorApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace BachelorApp.Interfaces
{
    public class LoraSocket : ILoraSocket
    {
        private ISensorDataService service;
        public LoraSocket()
        {
            OpenSocketAsync();
        }

        public async Task OpenSocketAsync()
        {
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
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var msgString = Encoding.UTF8.GetString(ms.ToArray());
                            if(msgString.Substring(8,2) == "gw")
                            {
                                Console.WriteLine("Message received.");
                                Console.WriteLine(msgString);

                                SaveMessage(msgString);
                            }
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
        }
        public void SaveMessage(String message)
        {
            //Deserialize and save in DB
            Root root = new Root();

            root = JsonSerializer.Deserialize<Root>(message);
            var time = DateTime.Parse(root.gws[0].time);
            
            var data1 = ConvertHexToInt(root.data.Substring(0, 4));
            var data2 = ConvertHexToInt(root.data.Substring(4, 4));

            float temp = ConvertIntToTemperature(data1);
            float pres = ConvertIntToPressure(data2);
            Sensor reading = new Sensor();
            reading.time = time;
            reading.temp = temp;
            reading.pres = pres;
            service.AddReading(reading);
            Console.WriteLine(time + " : " + data1 + ", " + data2);
        }

        public static int ConvertHexToInt(string hexString)
        {
            int response = Int32.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
            return response;
        }

        public float ConvertIntToTemperature(int temp)
        {
            return 1; // Add function to transform int into a temp float
        }
        public float ConvertIntToPressure(int press)
        {
            return 1; // Add function to transform int into a pres float
        }
    }
    public class Gw
    {
        public int rssi { get; set; }
        public int snr { get; set; }
        public object ts { get; set; }
        public int tmms { get; set; }
        public string time { get; set; }
        public string gweui { get; set; }
        public int ant { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }
    }

    public class Root
    {
        public string cmd { get; set; }
        public int seqno { get; set; }
        public string EUI { get; set; }
        public long ts { get; set; }
        public int fcnt { get; set; }
        public int port { get; set; }
        public int freq { get; set; }
        public int toa { get; set; }
        public string dr { get; set; }
        public bool ack { get; set; }
        public List<Gw> gws { get; set; }
        public int bat { get; set; }
        public string data { get; set; }
    }
    // How to request LoRa cache
    /*
            Payload payload = new Payload();
            payload.cmd = "cq";

            var request = JsonSerializer.Serialize(payload);
            byte[] bytes = Encoding.ASCII.GetBytes(request);

            Console.WriteLine("Sending initial message ...");
            ws.SendAsync(bytes, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
    */
}
