namespace SimpleChat.Contracts
{
    /// <summary>
    /// Обработчик клиентов на стороне сервера
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Уникальный идентификатор клиента
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Имя клиента
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Обрабатывает сообщения клиента
        /// </summary>
        void Process();

        /// <summary>
        /// Написать сообщение 
        /// </summary>
        void Write(string message, int offset = 0);

        /// <summary>
        /// Закрывает подключение
        /// </summary>
        void Close();
    }
}
