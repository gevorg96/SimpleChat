using System;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using SimpleChat.Contracts;

namespace SimpleChat.Client
{
    class Program
    {
        static TcpClient client;
        static NetworkStream stream;

        static void Main(string[] args)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var (port, host) = Extensions.GetPortHostPair(assembly);

            client = new TcpClient();
            try
            {
                client.Connect(host, port); 
                stream = client.GetStream(); 

                var receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); 

                Console.WriteLine("Добро пожаловать!");
                SendMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Disconnect();
            }
        }

        static void SendMessage()
        {
            while (true)
            {
                string message = Console.ReadLine();
                byte[] data = Encoding.Unicode.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }
        }

        static void ReceiveMessage()
        {
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; 
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string message = builder.ToString();
                    Console.WriteLine(message);
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!");
                    Console.ReadLine();
                    Disconnect();
                }
            }
        }

        static void Disconnect()
        {
            if (stream != null)
                stream.Close();

            if (client != null)
                client.Close();

            Environment.Exit(0);
        }
    }
}
