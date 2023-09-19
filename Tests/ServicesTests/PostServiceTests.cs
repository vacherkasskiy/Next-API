using FluentAssertions;
using Moq;
using NextAPI.Bll.Services;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;
using NextAPI.Exceptions.Post;
using Tests.Comparers;
using Tests.Fakers;
using Xunit;

namespace Tests.ServicesTests;

public class PostServiceTests
{
    [Fact]
    public async Task GetAll_ShouldSuccessAsync()
    {
        // Arrange
        var expected = PostFaker
            .Generate(5)
            .ToArray();
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockPostRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(expected);
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);

        // Act
        var result = await service.GetAll();

        // Assert
        result.Should().NotBeEmpty();
        result.Should().NotBeNull();
        result.Length.Should().Be(5);
        result.Should()
            .BeEquivalentTo(expected, options => options
                .Using(new PostComparer())
                .WithStrictOrdering());
        mockPostRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetPartition_ShouldSuccessAsync()
    {
        // Arrange
        const int skip = 2;
        const int limit = 5;
        var allPosts = PostFaker
            .Generate(10)
            .ToArray();
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockPostRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(allPosts
                .OrderBy(y => y.Id)
                .ToArray());
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);
        var expected = new GetPartitionResponse<Post>(
            allPosts
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(limit)
                .ToArray(), allPosts.Length);

        // Act
        var result = await service.GetPartition(skip, limit);

        // Assert
        result.Entities.Should().NotBeNull();
        result.Entities.Should().NotBeEmpty();
        result.Entities.Should().BeEquivalentTo(expected.Entities, options => options
            .Using(new PostComparer())
            .WithStrictOrdering());
        result.Length.Should().Be(expected.Length);
        mockPostRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldSuccessAsync()
    {
        // Arrange
        const int postId = 1;
        var expected = PostFaker
            .Generate()
            .Single();
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockPostRepository
            .Setup(x => x.GetById(postId))
            .ReturnsAsync(expected);
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);

        // Act
        var result = await service.GetById(postId);

        // Assert
        result
            .Should()
            .NotBeNull();
        result
            .Should()
            .BeEquivalentTo(expected, options =>
                options.Using(new PostComparer()));
        mockPostRepository
            .Verify(x =>
                x.GetById(postId), Times.Once);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldThrowAsync()
    {
        // Arrange
        const int invalidId = -1;
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockPostRepository
            .Setup(x => x.GetById(invalidId))
            .ReturnsAsync((Post?) null);
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);

        // Act & Assert
        await Assert.ThrowsAsync<PostWrongIdException>(() => service.GetById(invalidId));
        mockPostRepository.Verify(x => x.GetById(invalidId), Times.Once);
    }

    [Fact]
    public async Task Add_WithValidIds_ShouldSuccessAsync()
    {
        // Arrange
        const int authorId = 1;
        const int receiverId = 2;
        var post = PostFaker
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
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockUserRepository
            .Setup(x => x.GetById(authorId))
            .ReturnsAsync(author);
        mockUserRepository
            .Setup(x => x.GetById(receiverId))
            .ReturnsAsync(receiver);
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);

        // Act
        await service.Add(post);

        // Assert
        mockPostRepository.Verify(x => x.Add(post), Times.Once);
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
        var post = PostFaker
            .Generate()
            .Single()
            .WithAuthorId(authorId)
            .WithReceiverId(receiverId)
            .WithAuthor(null)
            .WithReceiver(null);
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockUserRepository
            .Setup(x => x.GetById(authorId))
            .ReturnsAsync((User?) null);
        mockUserRepository
            .Setup(x => x.GetById(receiverId))
            .ReturnsAsync((User?) null);
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);

        // Act & Assert
        await Assert
            .ThrowsAsync<PostAuthorOrReceiverNotFoundException>(async () =>
                await service.Add(post));
        mockUserRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Exactly(2));
        mockUserRepository.Verify(x => x.GetById(authorId), Times.Once);
        mockUserRepository.Verify(x => x.GetById(receiverId), Times.Once);
        mockPostRepository.Verify(x => x.Add(post), Times.Never);
    }

    [Fact]
    public async Task Update_ShouldSuccessAsync()
    {
        // Arrange
        var post = PostFaker.Generate().Single();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);

        // Act
        await service.Update(post);

        // Assert
        mockPostRepository.Verify(x => x.Update(post), Times.Once);
    }

    [Fact]
    public async Task DeleteById_WithValidId_ShouldSuccessAsync()
    {
        // Arrange
        const int validId = 1;
        var post = PostFaker
            .Generate()
            .Single()
            .WithId(validId);
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        mockPostRepository
            .Setup(x => x.GetById(validId))
            .ReturnsAsync(post);
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);

        // Act
        await service.DeleteById(validId);

        // Assert
        mockPostRepository.Verify(x => x.Delete(post), Times.Once);
        mockPostRepository.Verify(x => x.GetById(validId), Times.Once);
    }

    [Fact]
    public async Task DeleteById_WithInvalidId_ShouldSuccessAsync()
    {
        // Arrange
        const int invalidId = -1;
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        mockPostRepository
            .Setup(x => x.GetById(invalidId))
            .ReturnsAsync((Post?) null);
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);

        // Act & Assert
        await Assert
            .ThrowsAsync<PostWrongIdException>(async () =>
                await service.DeleteById(invalidId));
        mockPostRepository
            .Verify(x =>
                x.Delete(It.IsAny<Post>()), Times.Never);
        mockPostRepository
            .Verify(x =>
                x.GetById(invalidId), Times.Once);
    }

    [Fact]
    public async Task GetForUser_WithValidId_ShouldSuccessAsync()
    {
        // Arrange
        const int validId = 1;
        var user = UserFaker
            .Generate()
            .Single()
            .WithId(validId);
        var posts = PostFaker
            .Generate(10)
            .Select(x => x
                .WithReceiverId(x.ReceiverId % 3 + 1))
            .ToArray();
        var expected = posts
            .Where(x => x.ReceiverId == validId)
            .ToArray();
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockUserRepository
            .Setup(x => x.GetById(validId))
            .ReturnsAsync(user);
        mockPostRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(posts);
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);
        
        // Act
        var result = await service.GetForUser(validId);
        
        // Assert
        result.Should().NotBeNull();
        result.Length.Should().Be(expected.Length);
        result
            .Should()
            .BeEquivalentTo(expected, options => options
                .Using(new PostComparer())
                .WithStrictOrdering());
        mockUserRepository.Verify(x => x.GetById(validId), Times.Once);
        mockUserRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
        mockPostRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetForUser_WithInvalidId_ShouldThrowAsync()
    {
        // Arrange
        const int invalidId = -1;
        var mockPostRepository = new Mock<IBaseRepository<Post>>();
        var mockUserRepository = new Mock<IBaseRepository<User>>();
        mockUserRepository
            .Setup(x => x.GetById(invalidId))
            .ReturnsAsync((User?) null);
        var service = new PostsService(
            mockPostRepository.Object,
            mockUserRepository.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<PostWithWrongReceiverIdException>(async () =>
            await service.GetForUser(invalidId));
        mockPostRepository.Verify(x => x.GetAll(), Times.Never);
        mockUserRepository.Verify(x => x.GetById(invalidId), Times.Once);
        mockUserRepository.Verify(x => x.GetById(It.IsAny<int>()), Times.Once);
    }
}