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

            var result = new ValidationResult
            {
                HandlerType = HandlerType.Empty,
                Content = message
            };

            if (string.IsNullOrEmpty(message))
                return result;

            if (IsCommand(message))
            {
                result.HandlerType = HandlerType.Command;
                result.Content = message.ToLowerInvariant();
            }
            else if (IsStandardQuestion(message))
            {
                result.HandlerType = HandlerType.StandardQuestion;
                result.Content = message.Replace("?", string.Empty).ToLowerInvariant();
            }
            else
                result.HandlerType = HandlerType.Message;

            return result;
        }

        private static bool IsCommand(string message) => AppContants.CommandsList.Contains(message.ToLowerInvariant());

        private static bool IsStandardQuestion(string message) => AppContants.StandardQuestionsDict.ContainsKey(message.Replace("?", string.Empty).ToLowerInvariant());
    }
}
