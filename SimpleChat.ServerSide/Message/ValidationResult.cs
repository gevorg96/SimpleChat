namespace SimpleChat.ServerSide.Message
{
    /// <summary>
    /// Результат валидации сообщения
    /// </summary>
    public class ValidationResult
    {
        public HandlerType HandlerType { get; set; }

        public string Content { get; set; }
    }

    /// <summary>
    /// Типы результатов валидации
    /// </summary>
    public enum HandlerType : byte
    {
        Message = 1,
        Command = 2,
        StandardQuestion = 3,
        Empty = 4
    }
}
