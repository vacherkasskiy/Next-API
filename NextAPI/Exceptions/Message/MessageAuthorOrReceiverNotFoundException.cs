namespace NextAPI.Exceptions.Message;

public class MessageAuthorOrReceiverNotFoundException : Exception
{
    public MessageAuthorOrReceiverNotFoundException() : base("Message author or receiver was not found by id")
    {
    }
}