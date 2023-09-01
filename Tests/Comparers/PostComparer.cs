using NextAPI.Dal.Entities;

namespace Tests.Comparers;

public class PostComparer : IEqualityComparer<Post>
{
    public bool Equals(Post? x, Post? y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        
        return x.Id == y.Id &&
               x.AuthorId == y.AuthorId && 
               x.ReceiverId == y.ReceiverId && 
               x.Text == y.Text && 
               Equals(x.Author, y.Author) &&
               Equals(x.Receiver, y.Receiver);
    }

    public int GetHashCode(Post obj)
    {
        return HashCode.Combine(
            obj.Id,
            obj.AuthorId,
            obj.ReceiverId,
            obj.Text,
            obj.Author,
            obj.Receiver);
    }
}