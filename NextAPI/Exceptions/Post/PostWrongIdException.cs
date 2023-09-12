namespace NextAPI.Exceptions.Post;

public class PostWrongIdException : Exception
{
    public PostWrongIdException() : base("Wrong post id provided")
    {
    }
}