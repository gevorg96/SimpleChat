using SimpleChat.ServerSide.Message.Results;

namespace SimpleChat.ServerSide.Message
{
    /// <summary>
    /// Хелпер для обработки сообщений
    /// </summary>
    public static class MessageHelper
    {
        /// <summary>
        /// Провалидировать сообщение
        /// </summary>
        public static ValidationResult Validate(string message)
        {
            message = message.Trim();

            if (string.IsNullOrEmpty(message))
                return new EmptyResult();

            if (IsCommand(message))
                return new CommandResult(message.ToLowerInvariant());
            
            else if (IsStandardQuestion(message))
                return new StandardQuestionResult(message.Replace("?", string.Empty).ToLowerInvariant());
            
            else
                return new MessageResult(message);
        }

        private static bool IsCommand(string message) => AppContants.CommandsList.Contains(message.ToLowerInvariant());

        private static bool IsStandardQuestion(string message) => AppContants.StandardQuestionsDict.ContainsKey(message.Replace("?", string.Empty).ToLowerInvariant());
    }
}
