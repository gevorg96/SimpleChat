namespace SimpleChat.ServerSide.Message.Results
{
    public class StandardQuestionResult : ValidationResult
    {
        public StandardQuestionResult(string content)
            : base(content)
        { }

        public override HandlerType HandlerType => HandlerType.StandardQuestion;
    }
}
