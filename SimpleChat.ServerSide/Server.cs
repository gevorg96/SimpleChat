using SimpleChat.Contracts;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;

namespace SimpleChat.ServerSide
{
    /// <inheritdoc cref="IServer" />
    internal class Server : IServer
    {
        private static TcpListener _tcpListener;

        // подключённые клиенты
        private readonly ConcurrentDictionary<string, IClient> _clients =
            new ConcurrentDictionary<string, IClient>();

        /// <inheritdoc />
        public void AddClient(IClient clientObject) =>
            _clients.TryAdd(clientObject.Id, clientObject);

        /// <inheritdoc />
        public void RemoveClient(string clientId) =>
           _clients.TryRemove(clientId, out var _);

        /// <inheritdoc />
        public void Listen()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var (port, host) = Extensions.GetPortHostPair(assembly);

                _tcpListener = new TcpListener(IPAddress.Parse(host), port);
                _tcpListener.Start();
                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    TcpClient tcpClient = _tcpListener.AcceptTcpClient();

                    var clientObject = new ClientEntity(tcpClient, this);
                    var clientThread = new Thread(new ThreadStart(clientObject.Process));
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Disconnect();
            }
        }

        /// <inheritdoc />
        public List<string> GetAllClients() => _clients.Values.Select(x => x.UserName).ToList();

        /// <inheritdoc />
        public void Broadcast(string message, string clientId) =>
            _clients
                .Where(x => x.Key != clientId)
                .ForEach(x => x.Value.Write(message));

        /// <inheritdoc />
        public void Disconnect()
        {
            _tcpListener.Stop();
            _clients.ForEach(x => x.Value.Close());
            _clients.Clear();
            Environment.Exit(0);
        }
    }
}
