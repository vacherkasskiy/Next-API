namespace NextAPI.Exceptions.Auth;

public class WrongEmailException : Exception
{
    public WrongEmailException()
        : base("User with such e-mail does not exists")
    {
    }
}