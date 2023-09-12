namespace NextAPI.Exceptions.User;

public class UserNotFoundByIdException : Exception
{
    public UserNotFoundByIdException() : base("User was not found by id")
    {
    }
}