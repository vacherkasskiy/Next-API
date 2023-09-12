namespace NextAPI.Exceptions.Post;

public class PostAuthorOrReceiverNotFoundException : Exception
{
    public PostAuthorOrReceiverNotFoundException() : base("Wrong author or/and receiver id provided")
    {
    }
}