namespace SimpleChat.ServerSide.Message.Results
{
    public class MessageResult : ValidationResult
    {
        public MessageResult(string content)
            : base(content)
        { }

        public override HandlerType HandlerType => HandlerType.Message;
    }
}
