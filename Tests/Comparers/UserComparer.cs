using NextAPI.Dal.Entities;

namespace Tests.Comparers;

public class UserComparer : IEqualityComparer<User>
{
    public bool Equals(User? x, User? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        
        return
            x.Id == y.Id &&
            x.Image == y.Image &&
            x.Name == y.Name &&
            x.Username == y.Username &&
            x.Email == y.Email &&
            x.Website == y.Website &&
            x.City == y.City &&
            x.Status == y.Status;
    }

    public int GetHashCode(User obj)
    {
        return HashCode.Combine(
            obj.Id,
            obj.Image,
            obj.Name,
            obj.Username,
            obj.Email,
            obj.Website,
            obj.City,
            obj.Status);
    }
}