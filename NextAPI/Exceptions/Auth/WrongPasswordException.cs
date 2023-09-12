namespace NextAPI.Exceptions.Auth;

public class WrongPasswordException : Exception
{
    public WrongPasswordException()
        : base("Wrong password")
    {
    }
}