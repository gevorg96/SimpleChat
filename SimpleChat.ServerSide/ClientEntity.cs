using SimpleChat.Contracts;
using SimpleChat.ServerSide.Message;
using System;
using System.Net.Sockets;
using System.Text;

namespace SimpleChat.ServerSide
{
    /// <inheritdoc cref="IClient" />
    internal class ClientEntity : IClient
    {
        /// <inheritdoc />
        public string Id { get; private set; }

        /// <inheritdoc />
        public string UserName { get; private set; }

        // Экземпляр сервера
        private readonly Server _server;
        private Action<string> Send;
        private NetworkStream _stream;
        private readonly TcpClient _client;

        public ClientEntity(TcpClient tcpClient, Server server)
        {
            Id = Guid.NewGuid().ToString();
            _client = tcpClient;
            _server = server;
            _server.AddClient(this);

            HandleReceiver(AppContants.Commands.Receiver.All);
        }

        /// <inheritdoc />
        public void Process()
        {
            try
            {
                _stream = _client.GetStream();
                Write("Введите свое имя: ");
                var message = GetMessage();

                UserName = message;
                message = UserName + " вошел в чат";

                // посылаем сообщение о входе пользователя 
                // всем другим клиентам
                _server.Broadcast(message, Id);
                Console.WriteLine(message);
                Write(_getInfo);

                while (true)
                {
                    try
                    {
                        message = GetMessage();
                        Console.WriteLine(string.Format("{0}: {1}", UserName, message));

                        var validationResult = MessageHelper.Validate(message);
                        HandleMessageResult(validationResult);
                    }
                    catch
                    {
                        message = string.Format("{0}: покинул чат", UserName);
                        Console.WriteLine(message);
                        _server.Broadcast(message, Id);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                _server.RemoveClient(Id);
                Close();
            }
        }

        /// <inheritdoc />
        public void Write(string message, int offset = 0)
        {
            var data = Encoding.Unicode.GetBytes(message);
            _stream.Write(data, offset, data.Length);
        }

        /// <inheritdoc />
        public void Close()
        {
            if (_stream != null)
                _stream.Close();
            if (_client != null)
                _client.Close();
        }

        /// <summary>
        /// Чтение входящего сообщения и преобразование в строку
        /// </summary>
        /// <returns></returns>
        private string GetMessage()
        {
            var data = new byte[64];
            var builder = new StringBuilder();
            do
            {
                var bytes = _stream.Read(data, 0, data.Length);
                builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
            }
            while (_stream.DataAvailable);

            return builder.ToString();
        }

        /// <summary>
        /// Хэндлер сообщений
        /// </summary>
        /// <param name="result"></param>
        private void HandleMessageResult(ValidationResult result)
        {
            if (result.HandlerType == HandlerType.Message)
            {
                Send(result.Content);
                return;
            }

            if (result.HandlerType == HandlerType.Command && result.Content == AppContants.Commands.Disconnect)
            {
                Close();
                return;
            }

            var message = result.HandlerType switch
            {
                HandlerType.StandardQuestion => AppContants.StandardQuestionsDict[result.Content],
                HandlerType.Command => result.Content switch
                {
                    AppContants.Commands.ShowAllUsers => string.Format("Сервер:\n-{0}", string.Join("\n-", _server.GetAllClients())),
                    AppContants.Commands.Receiver.All => HandleReceiver(result.Content),
                    AppContants.Commands.Receiver.Server => HandleReceiver(result.Content),
                    AppContants.Commands.Info => _getInfo,
                    AppContants.Commands.Command => _getCommands,
                    _ => null
                }
            };

            if (message != null)
                Write(message);
        }
   
        /// <summary>
        /// Хэндлер получателей для клиента
        /// </summary>
        private string HandleReceiver(string command)
        {
            Send = command switch
            {
                AppContants.Commands.Receiver.Server => (string message) => Write(string.Format("Сервер: {0}", message)),
                AppContants.Commands.Receiver.All => (string message) => _server.Broadcast(string.Format("{0}: {1}", UserName, message), Id),
                _ => (string message) => Write(string.Format("Сервер: {0}", message))
            };
            return $"Получатель сменён на {command.Replace("/to", "")}";
        }

        private string _getInfo => 
            string.Join("\n", 
                $"Возможные вопросы:",
                $"-{string.Join("\n-", AppContants.StandardQuestionsDict.Keys)}",
                $"Посмотреть команды - {AppContants.Commands.Command}",
                $"Инфо - {AppContants.Commands.Info}"
            );

        private string _getCommands => $"Команды:\n-{string.Join("\n-", AppContants.CommandsList)}";
    }
}
