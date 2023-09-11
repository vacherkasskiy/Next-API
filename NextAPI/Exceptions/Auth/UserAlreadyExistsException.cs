namespace NextAPI.Exceptions.Auth;

public class UserAlreadyExistsException : Exception
{
    public UserAlreadyExistsException()
        : base("User with such e-mail already exists")
    {
    }
}