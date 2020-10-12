using System.Collections.Generic;

namespace SimpleChat.ServerSide.Message
{
    public static class AppContants
    {
        public static class Commands
        {
            public static class Receiver
            {
                public const string Server = "/toserver";

                public const string All = "/toall";
            }

            public const string ShowAllUsers = "/showall";

            public const string Disconnect = "/bye";

            public const string Info = "/info";

            public const string Command = "/commands";
        }

        public static class StandardQuestions
        {
            public const string Name = "как тебя зовут";

            public const string Hau = "как дела";

            public const string News = "что нового";
        }

        public static class StandardAnswers
        {
            public const string Name = "Сервер";

            public const string Hau = "Всё отлично!";

            public const string News = "В США протесты, а на Камчатке проснулся вулкан!";
        }

        public static readonly List<string> CommandsList = new List<string>
        {
            Commands.Disconnect,
            Commands.ShowAllUsers,
            Commands.Command,
            Commands.Info,
            Commands.Receiver.All,
            Commands.Receiver.Server
        };

        public static readonly Dictionary<string, string> StandardQuestionsDict = new Dictionary<string, string>
        {
            { StandardQuestions.Hau, StandardAnswers.Hau },
            { StandardQuestions.Name, StandardAnswers.Name },
            { StandardQuestions.News, StandardAnswers.News }
        };
    }
}
