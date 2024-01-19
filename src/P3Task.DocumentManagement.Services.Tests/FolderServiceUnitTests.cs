using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using P3Task.DocumentManagement.Application.Services;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Core.Interfaces;

namespace P3Task.DocumentManagement.Services.Tests;

public class FolderServiceUnitTests
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private readonly Guid _folderId = Guid.NewGuid();
    private readonly string _folderName = "TestFolder";
    private readonly Guid _parentFolderId = Guid.NewGuid();
    
    private Mock<IFolderRepository> _mockedFolderRepository = null!;
    private Mock<ILogger<FolderService>> _mockedLogger = null!;

    [SetUp]
    public void Setup()
    {
        _mockedLogger = new Mock<ILogger<FolderService>>();
        _mockedFolderRepository = new Mock<IFolderRepository>();
    }

    private FolderService GetService()
    {
        return new FolderService(_mockedLogger.Object, _mockedFolderRepository.Object);
    }

    [Test]
    public async Task CreateFolder_ShouldReturnCreatedFolder()
    {
        // arrange
        var folder = new Folder()
        {
            Id = _folderId,
            Name = _folderName
        };

        _mockedFolderRepository.Setup(x => x.CreateAsync(folder, _cancellationToken))
            .ReturnsAsync(folder);

        var service = GetService();
        
        // act
        var response = await service.AddAsync(folder, _cancellationToken);
        
        // assert
        response.Should().NotBeNull();
        response.Id.Should().Be(_folderId);
        response.Name.Should().Be(_folderName);
        response.ParentFolderId.Should().BeNull();
    }

    [Test]
    public async Task Delete_ShouldThrowKeyNotFoundException()
    {
        // arrange
        var nonSetupGuid = Guid.NewGuid();
        var service = GetService();
        
        // act and assert
        var act = async () => await service.DeleteAsync(nonSetupGuid, _cancellationToken);
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task Delete_ShouldPass()
    {
        // arrange
        var childFolder1 = new Folder()
        {
            Id = Guid.NewGuid(),
            Name = "child folder 1"
        };
        var childOfChildFolder2 = new Folder()
        {
            Id = Guid.NewGuid(),
            Name = "Child of child folder 2"
        };
        var childFolder2 = new Folder()
        {
            Id = Guid.NewGuid(),
            Name = "child folder 2",
            Folders = [childOfChildFolder2]
        };
        var folderWithChildren = new Folder()
        {
            Id = _folderId,
            Name = _folderName,
            Folders = [childFolder1, childFolder2]
        };

        _mockedFolderRepository.Setup(x => x.GetByIdAsync(_folderId, _cancellationToken, true, true))
            .ReturnsAsync(folderWithChildren);
        _mockedFolderRepository.Setup(x => x.GetByIdAsync(childFolder1.Id, _cancellationToken, true, true))
            .ReturnsAsync(childFolder1);
        _mockedFolderRepository.Setup(x => x.GetByIdAsync(childFolder2.Id, _cancellationToken, true, true))
            .ReturnsAsync(childFolder2);
        _mockedFolderRepository.Setup(x => x.GetByIdAsync(childOfChildFolder2.Id, _cancellationToken, true, true))
            .ReturnsAsync(childOfChildFolder2);
        
        var service = GetService();
        
        // act and assert
        var act = async () => await service.DeleteAsync(_folderId, _cancellationToken);
        await act.Should().NotThrowAsync();

        _mockedFolderRepository.Verify(x => x.DeleteAsync(It.IsAny<Folder>(), _cancellationToken), Times.Exactly(4));
    }
}