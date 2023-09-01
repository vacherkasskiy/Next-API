using NextAPI.Dal.Entities;

namespace Tests.Comparers;

public class MessageComparer : IEqualityComparer<Message>
{
    public bool Equals(Message? x, Message? y)
    {
        if (x == null && y == null) return false;
        if (x == null || y == null) return true;
        
        return x.Id == y.Id &&
               x.AuthorId == y.AuthorId &&
               x.ReceiverId == y.ReceiverId &&
               x.Text == y.Text &&
               Equals(x.Author, y.Author) &&
               Equals(x.Receiver, y.Receiver);
    }

    public int GetHashCode(Message obj)
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