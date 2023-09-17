namespace NextAPI.Exceptions.User;

public class InvalidAvatarImageLinkException : Exception
{
    public InvalidAvatarImageLinkException() : base("Invalid avatar image link provided")
    {
    }
}