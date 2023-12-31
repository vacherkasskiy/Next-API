﻿using FluentAssertions;
using Moq;
using NextAPI.Bll.Services;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;
using NextAPI.Exceptions.Message;
using Tests.Comparers;
using Tests.Fakers;
using Xunit;

namespace Tests.ServicesTests;

public class MessageServiceTests
{
    [Fact]
    public async Task GetAll_ShouldSuccessAsync()
    {
        // Arrange
        var expected = MessageFaker
            .Generate(5)
            .ToArray();
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockMessageRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(expected);
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);

        // Act
        var result = await service.GetAll();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().NotBeNull();
        result.Length.Should().Be(5);
        result.Should()
            .BeEquivalentTo(expected, options => options
                .Using(new MessageComparer())
                .WithStrictOrdering());
        mockMessageRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetPartition_ShouldSuccessAsync()
    {
        // Arrange
        const int skip = 2;
        const int limit = 5;
        var allMessages = MessageFaker
            .Generate(10)
            .ToArray();
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockMessageRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(allMessages
                .OrderBy(y => y.Id)
                .ToArray());
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);
        var expected = new GetPartitionResponse<Message>(
            allMessages
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(limit)
                .ToArray(), allMessages.Length);

        // Act
        var result = await service.GetPartition(skip, limit);

        // Assert
        result.Entities.Should().NotBeNull();
        result.Entities.Should().NotBeEmpty();
        result.Entities.Should().BeEquivalentTo(expected.Entities, options => options
            .Using(new MessageComparer())
            .WithStrictOrdering());
        result.Length.Should().Be(expected.Length);
        mockMessageRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldSuccessAsync()
    {
        // Arrange
        const int messageId = 1;
        var expected = MessageFaker
            .Generate()
            .Single();
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockMessageRepository
            .Setup(x => x.GetById(messageId))
            .ReturnsAsync(expected);
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);

        // Act
        var result = await service.GetById(messageId);

        // Assert
        result
            .Should()
            .NotBeNull();
        result
            .Should()
            .BeEquivalentTo(expected, options =>
                options.Using(new MessageComparer()));
        mockMessageRepository
            .Verify(x =>
                x.GetById(messageId), Times.Once);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldThrowAsync()
    {
        // Arrange
        const int invalidId = -1;
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockMessageRepository
            .Setup(x => x.GetById(invalidId))
            .ReturnsAsync((Message?) null);
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<MessageNotFoundByIdException>(() => service.GetById(invalidId));
        mockMessageRepository.Verify(x => x.GetById(invalidId), Times.Once);
    }

    [Fact]
    public async Task Add_WithValidIds_ShouldSuccessAsync()
    {
        // Arrange
        const int authorId = 1;
        const int receiverId = 2;
        var message = MessageFaker
            .Generate()
            .Single()
            .WithAuthorId(authorId)
            .WithReceiverId(receiverId)
            .WithAuthor(null)
            .WithReceiver(null);
        var author = UserFaker
            .Generate()
            .Single()
            .WithId(authorId);
        var receiver = UserFaker
            .Generate()
            .Single()
            .WithId(receiverId);
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockUserRepository
            .Setup(x => x.GetById(authorId))
            .ReturnsAsync(author);
        mockUserRepository
            .Setup(x => x.GetById(receiverId))
            .ReturnsAsync(receiver);
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);

        // Act
        await service.Add(message);

        // Assert
        mockMessageRepository.Verify(x => x.Add(message), Times.Once);
        mockUserRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(2));
        mockUserRepository.Verify(x => x.GetById(authorId), Times.Once);
        mockUserRepository.Verify(x => x.GetById(receiverId), Times.Once);
    }

    [Fact]
    public async Task Add_WithInvalidIds_ThrowsAsync()
    {
        // Arrange
        const int authorId = -1;
        const int receiverId = -2;
        var message = MessageFaker
            .Generate()
            .Single()
            .WithAuthorId(authorId)
            .WithReceiverId(receiverId)
            .WithAuthor(null)
            .WithReceiver(null);
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockUserRepository
            .Setup(x => x.GetById(authorId))
            .ReturnsAsync((User?) null);
        mockUserRepository
            .Setup(x => x.GetById(receiverId))
            .ReturnsAsync((User?) null);
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);

        // Act & Assert
        await Assert
            .ThrowsAsync<MessageAuthorOrReceiverNotFoundException>(async () =>
                await service.Add(message));
        mockUserRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(2));
        mockUserRepository.Verify(x => x.GetById(authorId), Times.Once);
        mockUserRepository.Verify(x => x.GetById(receiverId), Times.Once);
        mockMessageRepository.Verify(x => x.Add(message), Times.Never);
    }

    [Fact]
    public async Task Update_ShouldSuccessAsync()
    {
        // Arrange
        var message = MessageFaker.Generate().Single();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);

        // Act
        await service.Update(message);

        // Assert
        mockMessageRepository.Verify(x => x.Update(message), Times.Once);
    }

    [Fact]
    public async Task DeleteById_WithValidId_ShouldSuccessAsync()
    {
        // Arrange
        const int validId = 1;
        var message = MessageFaker
            .Generate()
            .Single()
            .WithId(validId);
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        mockMessageRepository
            .Setup(x => x.GetById(validId))
            .ReturnsAsync(message);
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);

        // Act
        await service.DeleteById(validId);

        // Assert
        mockMessageRepository.Verify(x => x.Delete(message), Times.Once);
        mockMessageRepository.Verify(x => x.GetById(validId), Times.Once);
    }

    [Fact]
    public async Task DeleteById_WithInvalidId_ShouldSuccessAsync()
    {
        // Arrange
        const int invalidId = -1;
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        mockMessageRepository
            .Setup(x => x.GetById(invalidId))
            .ReturnsAsync((Message?) null);
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);

        // Act & Assert
        await Assert
            .ThrowsAsync<MessageNotFoundByIdException>(async () =>
                await service.DeleteById(invalidId));
        mockMessageRepository
            .Verify(x =>
                x.Delete(It.IsAny<Message>()), Times.Never);
        mockMessageRepository
            .Verify(x =>
                x.GetById(invalidId), Times.Once);
    }

    [Fact]
    public async Task GetAllForUsersPair_WithValidIds_ShouldSuccessAsync()
    {
        // Arrange
        const int firstValidId = 1;
        const int secondValidId = 2;
        var firstUser = UserFaker
            .Generate()
            .Single()
            .WithId(firstValidId);
        var secondUser = UserFaker
            .Generate()
            .Single()
            .WithId(secondValidId);
        var messages = MessageFaker
            .Generate(10)
            .Select(x => x.WithAuthorId(x.AuthorId % 3 + 1))
            .Select(x => x.WithReceiverId(x.ReceiverId % 3 + 1))
            .ToArray();
        var expected = messages
            .Where(x =>
                (x.AuthorId == firstValidId && x.ReceiverId == secondValidId) ||
                (x.AuthorId == secondValidId && x.ReceiverId == firstValidId))
            .ToArray();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        mockMessageRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(messages);
        mockUserRepository
            .Setup(x => x.GetById(firstValidId))
            .ReturnsAsync(firstUser);
        mockUserRepository
            .Setup(x => x.GetById(secondValidId))
            .ReturnsAsync(secondUser);
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);

        // Act
        var result = await service.GetAllForUsersPair(firstValidId, secondValidId);

        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(expected.Length);
        result
            .Should()
            .BeEquivalentTo(expected, options => options
                .Using(new MessageComparer())
                .WithStrictOrdering());
        mockUserRepository.Verify(x => x.GetById(firstValidId), Times.Once);
        mockUserRepository.Verify(x => x.GetById(secondValidId), Times.Once);
        mockUserRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(2));
        mockMessageRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetAllForUsersPair_WithInvalidIds_ShouldSuccessAsync()
    {
        // Arrange
        const int firstInvalidId = -1;
        const int secondInvalidId = -2;
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        var mockMessageRepository = new Mock<IBaseRepository<Message>>();
        mockUserRepository
            .Setup(x => x.GetById(firstInvalidId))
            .ReturnsAsync((User?) null);
        mockUserRepository
            .Setup(x => x.GetById(secondInvalidId))
            .ReturnsAsync((User?) null);
        var service = new MessagesService(
            mockMessageRepository.Object,
            mockUserRepository.Object);

        // Act & Assert
        await Assert
            .ThrowsAsync<MessageAuthorOrReceiverNotFoundException>(async () => await service
                .GetAllForUsersPair(firstInvalidId, secondInvalidId));
        mockUserRepository.Verify(x => x.GetById(firstInvalidId), Times.Once);
        mockUserRepository.Verify(x => x.GetById(secondInvalidId), Times.Once);
        mockMessageRepository.Verify(x => x.GetAll(), Times.Never);
    }
}