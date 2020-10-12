namespace SimpleChat.ServerSide.Message.Results
{
    public class EmptyResult : ValidationResult
    {
        public EmptyResult()
            : base(string.Empty)
        { }

        public override HandlerType HandlerType => HandlerType.Empty;
    }
}
