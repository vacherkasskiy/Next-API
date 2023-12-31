﻿using AutoBogus;
using Bogus;
using NextAPI.Dal.Entities;

namespace Tests.Fakers;

public static class MessageFaker
{
    private static readonly object Lock = new();

    private static readonly Faker<Message> Faker = new AutoFaker<Message>()
        .RuleFor(x => x.Id, f => f.Random.Int(1, 1000))
        .RuleFor(x => x.ReceiverId, f => f.Random.Int(1, 1000))
        .RuleFor(x => x.AuthorId, f => f.Random.Int(1, 1000))
        .RuleFor(x => x.Text, f => f.Lorem.Sentence())
        .FinishWith((_, message) =>
        {
            message.Receiver = UserFaker.Generate()
                .Single()
                .WithId(message.ReceiverId);
            message.Author = UserFaker.Generate()
                .Single()
                .WithId(message.AuthorId);
        });
    
    public static IEnumerable<Message> Generate(int count = 1)
    {
        lock (Lock)
        {
            var messages = new List<Message>();
            for (var i = 0; i < count; i++)
            {
                messages.Add(Faker.Generate());
            }
            return messages;
        }
    }

    public static Message WithId(
        this Message src,
        int id)
    {
        src.Id = id;
        return src;
    }
    
    public static Message WithReceiverId(
        this Message src,
        int receiverId)
    {
        src.ReceiverId = receiverId;
        return src;
    }
    
    public static Message WithAuthorId(
        this Message src,
        int authorId)
    {
        src.AuthorId = authorId;
        return src;
    }
    
    public static Message WithText(
        this Message src,
        string text)
    {
        src.Text = text;
        return src;
    }
    
    public static Message WithAuthor(
        this Message src,
        User? author)
    {
        src.Author = author;
        return src;
    }
    
    public static Message WithReceiver(
        this Message src,
        User? receiver)
    {
        src.Receiver = receiver;
        return src;
    }
}