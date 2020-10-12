using SimpleChat.Contracts;
using System;
using System.Threading;

namespace SimpleChat.ServerSide
{
    internal class Program
    {
        // сервер
        private static IServer server;

        // прослушиваемый поток
        private static Thread listenThread;

        private static void Main(string[] args)
        {
            try
            {
                server = new Server();
                listenThread = new Thread(new ThreadStart(server.Listen));
                listenThread.Start();
            }
            catch (Exception ex)
            {
                server.Disconnect();
                Console.WriteLine(ex.Message);
            }
        }
    }
}
