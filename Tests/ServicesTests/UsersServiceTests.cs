using Moq;
using NextAPI.Bll.Services;
using NextAPI.Dal.Entities;
using NextAPI.Dal.Repositories.Interfaces;
using Tests.Fakers;
using Xunit;

namespace Tests.ServicesTests;

public class UsersServiceTests
{
    [Fact]
    public async Task GetById_WithValidId_ShouldReturnUser()
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
        Assert.Equal(expectedUser, result);
    }
    
    [Fact]
    public async Task GetById_WithInvalidId_ShouldThrow()
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
    }
}