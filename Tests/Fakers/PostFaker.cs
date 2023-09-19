using AutoBogus;
using Bogus;
using NextAPI.Dal.Entities;

namespace Tests.Fakers;

public static class PostFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<Post> Faker = new AutoFaker<Post>()
        .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
        .RuleFor(x => x.ReceiverId, f => f.Random.Int(1, 1000))
        .RuleFor(x => x.AuthorId, f => f.Random.Int(1, 1000))
        .RuleFor(x => x.Text, f => f.Lorem.Sentence())
        .FinishWith((_, post) =>
        {
            post.Receiver = UserFaker.Generate()
                .Single()
                .WithId(post.ReceiverId);
            post.Author = UserFaker.Generate()
                .Single()
                .WithId(post.AuthorId);
        });
    
    public static IEnumerable<Post> Generate(int count = 1)
    {
        lock (Lock)
        {
            var posts = new List<Post>();
            for (var i = 0; i < count; i++)
            {
                posts.Add(Faker.Generate());
            }
            return posts;
        }
    }

    public static Post WithId(
        this Post src,
        int id)
    {
        src.Id = id;
        return src;
    }
    
    public static Post WithReceiverId(
        this Post src,
        int receiverId)
    {
        src.ReceiverId = receiverId;
        return src;
    }
    
    public static Post WithAuthorId(
        this Post src,
        int authorId)
    {
        src.AuthorId = authorId;
        return src;
    }
    
    public static Post WithText(
        this Post src,
        string text)
    {
        src.Text = text;
        return src;
    }
    
    public static Post WithAuthor(
        this Post src,
        User? author)
    {
        src.Author = author;
        return src;
    }
    
    public static Post WithReceiver(
        this Post src,
        User? receiver)
    {
        src.Receiver = receiver;
        return src;
    }
}