using FluentAssertions;
using Moq;
using NextAPI.Bll.Services;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;
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
        result.Should()
            .BeEquivalentTo(expected, options => options
                .Using(new MessageComparer())
                .WithStrictOrdering());
        mockMessageRepository.Verify(x => x.GetAll(), Times.Once);
    }
}