using FluentAssertions;
using Moq;
using NextAPI.Bll.Services;
using NextAPI.Bll.Services.Interfaces;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;
using Tests.Comparers;
using Tests.Fakers;
using Xunit;

namespace Tests.ServicesTests;

public class UsersServiceTests
{
    [Fact]
    public async Task GetById_WithValidId_ShouldReturnUserAsync()
    {
        // Arrange
        const int userId = 1;
        var expectedUser = UserFaker.Generate()
            .Single()
            .WithId(userId);
        var mockRepository = new Mock<IBaseRepository<User>>();
        
        mockRepository
            .Setup(x => x.GetById(userId))
            .ReturnsAsync(expectedUser);
        var userService = new UsersService(mockRepository.Object);
        
        // Act
        var result = await userService.GetById(userId);

        // Assert
        Assert.Equal(expectedUser, result, new UserComparer());
        mockRepository.Verify(x => x.GetById(userId), Times.Once);
    }
    
    [Fact]
    public async Task GetById_WithInvalidId_ShouldThrowAsync()
    {
        // Arrange
        const int invalidId = -1;
        var mockRepository = new Mock<IBaseRepository<User>>();
        mockRepository
            .Setup(x => x.GetById(invalidId))
            .ReturnsAsync((User?)null);
        var userService = new UsersService(mockRepository.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => userService.GetById(invalidId));
        mockRepository.Verify(x => x.GetById(invalidId), Times.Once);
    }

    [Fact]
    public async Task GetAll_ShouldSuccessAsync()
    {
        // Arrange
        var expectedUsers = UserFaker
            .Generate(5)
            .ToArray();
        var mockRepository = new Mock<IBaseRepository<User>>();
        mockRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(expectedUsers);
        var service = new UsersService(mockRepository.Object);

        // Act
        var result = await service.GetAll();
        
        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Length.Should().Be(5);
        result.Should().BeEquivalentTo(expectedUsers, options => options
            .Using(new UserComparer())
            .WithStrictOrdering());
        mockRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GetPartition_ShouldSuccessAsync()
    {
        // Arrange
        const int skip = 2;
        const int limit = 5;
        var allUsers = UserFaker
            .Generate(10)
            .ToArray();
        var mockRepository = new Mock<IBaseRepository<User>>();
        mockRepository
            .Setup(x => x.GetAll())
            .ReturnsAsync(allUsers
                .OrderBy(y => y.Id)
                .ToArray());
        var service = new UsersService(mockRepository.Object);
        var expected = new GetPartitionResponse<User>(
            allUsers
                .OrderBy(x => x.Id)
                .Skip(skip)
                .Take(limit)
                .ToArray(), allUsers.Length);
        
        // Act
        var result = await service.GetPartition(skip, limit);
        
        // Assert
        result.Entities.Should().NotBeNull();
        result.Entities.Should().NotBeEmpty();
        result.Entities.Should().BeEquivalentTo(expected.Entities, options => options
            .Using(new UserComparer())
            .WithStrictOrdering());
        result.Length.Should().Be(expected.Length);
        mockRepository.Verify(x => x.GetAll(), Times.Once);
    }

    [Fact]
    public async Task DeleteById_WithInvalidUserId_ShouldThrowAsync()
    {
        // Arrange
        const int invalidUserId = -1;
        var mockRepository = new Mock<IBaseRepository<User>>();
        mockRepository
            .Setup(x => x.GetById(invalidUserId))
            .ReturnsAsync((User?) null);
        var service = new UsersService(mockRepository.Object);
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.DeleteById(invalidUserId));
        mockRepository.Verify(x => x.GetById(invalidUserId), Times.Once);
        mockRepository.Verify(x => x.Delete(It.IsAny<User>()), Times.Never);
    }
    
    [Fact]
    public async Task DeleteById_WithValidUserId_ShouldSucceedAsync()
    {
        // Arrange
        const int userId = 1;
        var user = UserFaker.Generate().Single().WithId(userId);
        var mockRepository = new Mock<IBaseRepository<User>>();
        mockRepository
            .Setup(x => x.GetById(userId))
            .ReturnsAsync(user);
        var service = new UsersService(mockRepository.Object);

        // Act
        await service.DeleteById(userId);

        // Assert
        mockRepository.Verify(x => x.GetById(userId), Times.Once);
        mockRepository.Verify(x => x.Delete(user), Times.Once);
    }

    [Fact]
    public async Task Add_ShouldSuccessAsync()
    {
        // Arrange
        var user = UserFaker.Generate().Single();
        var mockRepository = new Mock<IBaseRepository<User>>();
        var service = new UsersService(mockRepository.Object);
        
        // Act
        await service.Add(user);
        
        // Assert
        mockRepository.Verify(x => x.Add(user), Times.Once);
    }
    
    [Fact]
    public async Task Update_ShouldSuccessAsync()
    {
        // Arrange
        var user = UserFaker.Generate().Single();
        var mockRepository = new Mock<IBaseRepository<User>>();
        var service = new UsersService(mockRepository.Object);
        
        // Act
        await service.Update(user);
        
        // Assert
        mockRepository.Verify(x => x.Update(user), Times.Once);
    }
}