namespace SimpleChat.ServerSide.Message
{
    /// <summary>
    /// Результат валидации сообщения
    /// </summary>
    public abstract class ValidationResult
    {
        protected ValidationResult(string content) => Content = content;

        public abstract HandlerType HandlerType { get; }

        public string Content { get; }
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
