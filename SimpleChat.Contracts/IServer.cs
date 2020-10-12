using System.Collections.Generic;

namespace SimpleChat.Contracts
{
    /// <summary>
    /// TCP-сервер
    /// </summary>
    public interface IServer
    {
        /// <summary>
        /// Добавление нового клиента
        /// </summary>
        void AddClient(IClient clientObject);

        /// <summary>
        /// Удаление клиента
        /// </summary>
        void RemoveClient(string clientId);

        /// <summary>
        /// Прослушивание входящих подключений
        /// </summary>
        void Listen();

        /// <summary>
        /// Трансляция сообщения подключенным клиентам
        /// </summary>
        void Broadcast(string message, string clientFromId);

        /// <summary>
        /// Отключение всех клиентов
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Возвращает список всех активных клиентов
        /// </summary>
        List<string> GetAllClients();
    }
}
