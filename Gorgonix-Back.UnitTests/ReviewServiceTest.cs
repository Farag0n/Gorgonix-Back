using FluentAssertions;
using Gorgonix_Back.Application.DTOs;
using Gorgonix_Back.Application.Services;
using Gorgonix_Back.Domain.Entities;
using Gorgonix_Back.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Gorgonix_Back.UnitTests.Application;

public class ReviewServiceTests
{
    private readonly Mock<IReviewRepository> _reviewRepoMock;
    private readonly Mock<IProfileRepository> _profileRepoMock;
    private readonly Mock<IContentRepository> _contentRepoMock;
    private readonly Mock<ILogger<ReviewService>> _loggerMock;
    private readonly ReviewService _service;

    public ReviewServiceTests()
    {
        _reviewRepoMock = new Mock<IReviewRepository>();
        _profileRepoMock = new Mock<IProfileRepository>();
        _contentRepoMock = new Mock<IContentRepository>();
        _loggerMock = new Mock<ILogger<ReviewService>>();

        _service = new ReviewService(
            _reviewRepoMock.Object, 
            _profileRepoMock.Object, 
            _contentRepoMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task AddReview_Should_Return_Dto_When_Profile_And_Content_Exist()
    {
        // Arrange
        var dto = new ReviewCreateDto { ProfileId = Guid.NewGuid(), ContentId = Guid.NewGuid(), Title = "Wow", Body = "Great" };

        // Simulamos que el Perfil EXISTE
        _profileRepoMock.Setup(x => x.GetProfileByIdAsync(dto.ProfileId))
            .ReturnsAsync(new Profile("Perfil1", null, Guid.NewGuid()));

        // Simulamos que el Contenido EXISTE
        _contentRepoMock.Setup(x => x.GetContentByIdAsync(dto.ContentId))
            .ReturnsAsync(new Content("Matrix", "Desc", Guid.NewGuid(), "url", "id", "url", "id"));

        // Act
        var result = await _service.AddReviewAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Wow");
        _reviewRepoMock.Verify(x => x.AddAsync(It.IsAny<Review>()), Times.Once);
    }

    [Fact]
    public async Task AddReview_Should_Throw_When_Profile_Does_Not_Exist()
    {
        // Arrange
        var dto = new ReviewCreateDto { ProfileId = Guid.NewGuid() };

        // Simulamos que el perfil NO existe (retorna null)
        _profileRepoMock.Setup(x => x.GetProfileByIdAsync(dto.ProfileId))
            .ReturnsAsync((Profile)null);

        // Act
        Func<Task> act = async () => await _service.AddReviewAsync(dto);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }
}