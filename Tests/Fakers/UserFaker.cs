using AutoBogus;
using Bogus;
using NextAPI.Dal.Entities;

namespace Tests.Fakers;

public static class UserFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<User> Faker = new AutoFaker<User>()
        .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
        .RuleFor(x => x.Name, f => f.Person.FullName)
        .RuleFor(x => x.Username, f => f.Person.UserName)
        .RuleFor(x => x.Email, f => f.Person.Email)
        .RuleFor(x => x.City, f => f.Address.City())
        .RuleFor(x => x.Image, f => f.Person.Avatar)
        .RuleFor(x => x.Status, f => f.Person.Company.CatchPhrase)
        .RuleFor(x => x.Website, f => f.Person.Website);

    public static IEnumerable<User> Generate(int count = 1)
    {
        lock (Lock)
        {
            var users = new List<User>();
            for (int i = 0; i < count; i++)
            {
                users.Add(Faker.Generate());
            }
            return users;
        }
    }
    
    public static User WithId(
        this User src,
        int userId)
    {
        src.Id = userId;
        return src;
    }
    
    public static User WithName(
        this User src,
        string name)
    {
        src.Name = name;
        return src;
    }
        
    public static User WithUsername(
        this User src,
        string username)
    {
        src.Username = username;
        return src;
    }

    public static User WithEmail(
        this User src,
        string email)
    {
        src.Email = email;
        return src;
    }
    
    public static User WithCity(
        this User src,
        string? city)
    {
        src.City = city;
        return src;
    }
        
    public static User WithImage(
        this User src,
        string? image)
    {
        src.Image = image;
        return src;
    }
    
    public static User WithStatus(
        this User src,
        string? status)
    {
        src.Status = status;
        return src;
    }
    
    public static User WithWebsite(
        this User src,
        string? website)
    {
        src.Website = website;
        return src;
    }
}