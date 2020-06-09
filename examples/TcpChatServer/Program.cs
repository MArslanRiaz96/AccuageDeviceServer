using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using TcpChatServer.Repositroy;
using TcpChatServer.Model;

namespace TcpChatServer
{

    class ClientManagement
    {
        public string LoginId { get; set; }
        public Guid NewKey { get; set; }
        public Guid OldKey { get; set; }
    }
    class AccuguageSession : TcpSession
    {

        List<ClientManagement> tcpClient = new List<ClientManagement>();
        public AccuguageSession(TcpServer server) : base(server) {
           
        }
        

        private void fileWrite(string file,Guid clientId)
        {
            try
            {

                string path = @"C:\Users\Administrator\Desktop\ArslanTCP\ArslanTCP.txt";
                // This text is added only once to the file.
                if (!File.Exists(path))
                {
                    //var response = DataRepositroy.IsertRaw("Server=173.248.132.203,1533; Initial Catalog=accuguage_db; User Id = sa; Password=7gUSS@nKH;", new DeviceRawData { DeviceLogin = this.DeviceCurrentLogin, RawData = file });
                    //Console.WriteLine(response);
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        var response = DataRepositroy.IsertRaw("Server=173.248.132.203,1533; Initial Catalog=accuguage_TCP_db; User Id = sa; Password=7gUSS@nKH;", new DeviceRawData { DeviceLogin = this.tcpClient.FirstOrDefault(x => x.NewKey == Id).LoginId, RawData = file });
                        Console.WriteLine(response);
                        sw.WriteLine(file);

                    }
                }
          

            // This text is always added, making the file longer over time
            // if it is not deleted.
            using (StreamWriter sw = File.AppendText(path))
            {
                    Console.WriteLine($"TCP Connected Client{tcpClient.Count}");
                var response = DataRepositroy.IsertRaw("Server=173.248.132.203,1533; Initial Catalog=accuguage_TCP_db; User Id = sa; Password=7gUSS@nKH;", new DeviceRawData { DeviceLogin = this.tcpClient.FirstOrDefault(x=>x.NewKey==Id).LoginId, RawData = file });
                Console.WriteLine(response);
                sw.WriteLine(file);
                
            }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        protected override void OnConnected()
        {
            Console.WriteLine($" TCP session with Id {Id} connected!");
            SendAsync("#AL#1\r\n");
        }

        protected override void OnDisconnected()
        {
            var response=tcpClient.FirstOrDefault(x => x.NewKey == Id);
            if (response != null)
            {
                response.OldKey = response.NewKey;
            }
            Console.WriteLine($" TCP session with Id {Id} disconnected!");
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {

            Console.WriteLine("connect Client Id" + this.Id);
            string message = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
          if(message.Contains("#L#"))
            {
                //message.Contains("#L#")
                //check if key is contain
               var response=tcpClient.FirstOrDefault(x=>x.LoginId.Equals(message.Split(";")[1]));
                if (response == null)
                {
                    tcpClient.Add(new ClientManagement
                    {
                        NewKey = Id,
                        LoginId = message.Split(";")[1]
                    });
                }
                else 
                {
                    
                 if (response.LoginId.Equals(message.Split(";")[1]))
                    {
                        response.NewKey = Id;
                    }
                    //get index
                    tcpClient[tcpClient.IndexOf(response)] = response;
                }
                //check 
                //  tcpClient.Values.

                //check if the key is contain but the value is the same


            }
            fileWrite(message,Id);
            Console.WriteLine("Incoming: " + message);
            string str = "";
            if (message.Contains("#L#"))
            {
                if (message.Contains("NA"))
                {
                    //check device 
                    //get current device id
                    str = "#AL#01\r\n";
                }
                else
                {
                    str = "#AL#1\r\n";
                }

            }
            else if (message.Contains("#D#"))
            {
                if (message.Split("#")[2].Split(";").Length < 15)
                {
                    str = "#AD#-1\r\n";
                }
                else
                {
                    str = "#AD#1\r\n";
                }

            }
            else if (message.Contains("#B#"))
            {
                str = "#AB#3\r\n";
            }
            else
            {
                str = "#AL#0\r\n";
            }
            SendAsync(str);
          
            Console.WriteLine(str);
          

            if (message == "!")
                Disconnect();
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"TCP session caught an error with code {error}");
        }
    }

    class AccuguageServer : TcpServer
    {
        public AccuguageServer(IPAddress address, int port) : base(address, port) {}

        protected override TcpSession CreateSession() { return new AccuguageSession(this); }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"TCP server caught an error with code {error}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // TCP server port
            int port = 8999;
            if (args.Length > 0)
                port = int.Parse(args[0]);

            Console.WriteLine($"TCP server port: {port}");

            Console.WriteLine();

            // Create a new TCP chat server
            //173.248.132.203
            var server = new AccuguageServer(IPAddress.Parse("173.248.132.203"), port);

            // Start the server
            Console.Write("Server starting...");
            server.Start();
            Console.WriteLine("Done!");

            Console.WriteLine("Press Enter to stop the server or '!' to restart the server...");

            // Perform text input
            for (;;)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrEmpty(line))
                    break;

                // Restart the server
                if (line == "!")
                {
                    Console.Write("Server restarting...");
                    server.Restart();
                    Console.WriteLine("Done!");
                    continue;
                }

                // Multicast admin message to all sessions
                line = "(admin) " + line;
                server.Multicast(line);
            }

            // Stop the server
            Console.Write("Server stopping...");
            server.Stop();
            Console.WriteLine("Done!");
        }
    }
}
