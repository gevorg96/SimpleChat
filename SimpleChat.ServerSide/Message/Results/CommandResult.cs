namespace SimpleChat.ServerSide.Message.Results
{
    public class CommandResult : ValidationResult
    {
        public CommandResult(string content)
            : base(content)
        { }

        public override HandlerType HandlerType => HandlerType.Command;
    }
}
