namespace NextAPI.Exceptions.Post;

public class PostWithWrongReceiverIdException : Exception
{
    public PostWithWrongReceiverIdException() : base("Posts can't be found by wrong owner Id")
    {
    }
}