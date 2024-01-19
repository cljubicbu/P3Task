using System.Data;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using P3Task.DocumentManagement.Application.Services;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Core.Interfaces;
using File = P3Task.DocumentManagement.Core.Entities.File;

namespace P3Task.DocumentManagement.Services.Tests;

public class FileServiceUnitTests
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private readonly Guid _fileId = Guid.NewGuid();
    private readonly string _fileName = "TestFile";
    private readonly Guid _folderId = Guid.NewGuid();

    private Mock<IFileRepository> _mockedFileRepository = null!;
    private Mock<ILogger<FileService>> _mockedLogger = null!;

    [SetUp]
    public void Setup()
    {
        _mockedLogger = new Mock<ILogger<FileService>>();
        _mockedFileRepository = new Mock<IFileRepository>();

        _mockedFileRepository.Setup(x => x.GetByIdAsync(_fileId, _cancellationToken))
            .ReturnsAsync(new File { Id = _fileId, Name = _fileName, FolderId = _folderId });
    }

    private FileService GetService()
    {
        return new FileService(_mockedLogger.Object, _mockedFileRepository.Object);
    }
    
    [Test]
    public async Task Create_ShouldReturnCreatedFile()
    {
        // arrange
        var fileItem = new File()
        {
            Id = _fileId,
            Name = _fileName,
            FolderId = _folderId
        };
        _mockedFileRepository.Setup(x => x.GetByNameAsync(_fileName, _folderId, _cancellationToken))
            .ReturnsAsync([]);
        
        _mockedFileRepository.Setup(x => x.AddAsync(fileItem, _cancellationToken))
            .ReturnsAsync(fileItem);
        
        var service = GetService();
        
        // act
        var response = await service.AddAsync(fileItem, _cancellationToken);

        // assert
        response.Should().NotBeNull();
        response.Id.Should().Be(_fileId);
        response.Name.Should().Be(_fileName);
        response.FolderId.Should().Be(_folderId);
    }
    
    [Test]
    public async Task Create_ShouldThrowDuplicateNameException()
    {
        // arrange
        var fileItem = new File()
        {
            Id = _fileId,
            Name = _fileName,
            FolderId = _folderId
        };
        _mockedFileRepository.Setup(x => x.GetByNameAsync(_fileName, _folderId, _cancellationToken))
            .ReturnsAsync([fileItem]);
        
        _mockedFileRepository.Setup(x => x.AddAsync(fileItem, _cancellationToken))
            .ReturnsAsync(fileItem);
        
        var service = GetService();
        
        // act and assert
        var act = async () => { await service.AddAsync(fileItem, _cancellationToken); };
        await act.Should().ThrowAsync<DuplicateNameException>();
    }
    
    [Test]
    public async Task GetById_ShouldReturnFile()
    {
        // arrange
        var service = GetService();
        
        // act
        var response = await service.GetByIdAsync(_fileId, _cancellationToken);

        // assert
        response.Should().NotBeNull();
        response.Id.Should().Be(_fileId);
        response.Name.Should().Be(_fileName);
        response.FolderId.Should().Be(_folderId);
    }

    [Test]
    public async Task GetById_ShouldThrowKeyNotFoundException()
    {
        // arrange
        var nonSetupGuid = Guid.NewGuid();
        var service = GetService();
        
        // act and assert
        var act = async () => { await service.GetByIdAsync(nonSetupGuid, _cancellationToken); };
        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Test]
    public async Task GetByName_ShouldReturnOneFile()
    {
        // arrange
        _mockedFileRepository.Setup(x => x.GetByNameAsync(_fileName, _folderId, _cancellationToken))
            .ReturnsAsync([new File { Id = _fileId, Name = _fileName, FolderId = _folderId }]);
        
        var service = GetService();
        
        // act
        var response = await service.GetByNameAsync(_fileName, _folderId, _cancellationToken);

        // assert
        response.Should().NotBeNull();
        response.Count.Should().Be(1);
        response[0].Id.Should().Be(_fileId);
        response[0].Name.Should().Be(_fileName);
        response[0].FolderId.Should().Be(_folderId);
    }

    [Test]
    public async Task GetByName_ShouldReturnEmptyList()
    {
        // arrange
        _mockedFileRepository.Setup(x => x.GetByNameAsync("SomeFile", _folderId, _cancellationToken))
            .ReturnsAsync([]);
        
        var service = GetService();
        
        // act
        var response = await service.GetByNameAsync("SomeFile", _folderId, _cancellationToken);
        
        // assert
        response.Should().NotBeNull();
        response.Count.Should().Be(0);
    }

    [Test]
    public async Task Search_ShouldReturnOneFile()
    {
        // arrange
        var searchString = "Test";
        _mockedFileRepository.Setup(x => x.SearchAsync(searchString, _cancellationToken))
            .ReturnsAsync([new File { Id = _fileId, Name = _fileName, FolderId = _folderId }]);

        var service = GetService();
        
        // act
        var response = await service.SearchAsync(searchString, _cancellationToken);
        
        // assert
        response.Should().NotBeNull();
        response.Count.Should().Be(1);
        response[0].Id.Should().Be(_fileId);
        response[0].Name.Should().Be(_fileName);
        response[0].FolderId.Should().Be(_folderId);
    }
    
    [Test]
    public async Task Search_ShouldReturnEmptyList()
    {
        // arrange
        var searchString = "pero";
        _mockedFileRepository.Setup(x => x.SearchAsync(searchString, _cancellationToken))
            .ReturnsAsync([]);

        var service = GetService();
        
        // act
        var response = await service.SearchAsync(searchString, _cancellationToken);
        
        // assert
        response.Should().NotBeNull();
        response.Count.Should().Be(0);
    }
    
    [Test]
    public async Task Delete_ShouldPass()
    {
        // arrange
        var file = new File()
        {
            Id = _fileId,
            Name = _fileName,
            FolderId = _folderId
        };
        _mockedFileRepository.Setup(x => x.DeleteAsync(file, _cancellationToken))
            .Returns(Task.CompletedTask);

        var service = GetService();
        
        // act and assert
        var act = async () => await service.DeleteAsync(_fileId, _cancellationToken);
        await act.Should().NotThrowAsync();
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
}