namespace NextAPI.Exceptions.Message;

public class MessageNotFoundByIdException : Exception
{
    public MessageNotFoundByIdException() : base("Message was not found by id")
    {
    }
}