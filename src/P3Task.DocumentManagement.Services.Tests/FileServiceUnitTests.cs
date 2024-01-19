using Microsoft.Extensions.Logging;
using Moq;
using P3Task.DocumentManagement.Application.Services;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Core.Interfaces;

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
            .ReturnsAsync(new FileItem { Id = _fileId, Name = _fileName, FolderId = _folderId });

    }

    private FileService GetService()
    {
        return new FileService(_mockedLogger.Object, _mockedFileRepository.Object);
    }

    [Test]
    public async Task GetById_ShouldReturnFile()
    {
        // act
        var response = await GetService().GetByIdAsync(_fileId, _cancellationToken);

        // assert
        Assert.IsNotNull(response);
        Assert.That(response.Id, Is.EqualTo(_fileId));
        Assert.That(response.Name, Is.EqualTo(_fileName));
        Assert.That(response.FolderId, Is.EqualTo(_folderId));
    }

    [Test]
    public void GetById_ShouldThrowKeyNotFoundException()
    {
        // arrange
        var nonSetupGuid = Guid.NewGuid();

        // act and assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => GetService().GetByIdAsync(nonSetupGuid, _cancellationToken));
    }

    [Test]
    public async Task GetByName_ShouldReturnOneFile()
    {
        // arrange
        _mockedFileRepository.Setup(x => x.GetByNameAsync(_fileName, _folderId, _cancellationToken))
            .ReturnsAsync([new FileItem { Id = _fileId, Name = _fileName, FolderId = _folderId }]);

        // act
        var response = await GetService().GetByNameAsync(_fileName, _folderId, _cancellationToken);

        // assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Count, Is.EqualTo(1));
        Assert.That(response[0].Id, Is.EqualTo(_fileId));
        Assert.That(response[0].Name, Is.EqualTo(_fileName));
        Assert.That(response[0].FolderId, Is.EqualTo(_folderId));
    }

    [Test]
    public async Task GetByName_ShouldReturnEmptyList()
    {
        // arrange
        _mockedFileRepository.Setup(x => x.GetByNameAsync("SomeFile", _folderId, _cancellationToken))
            .ReturnsAsync([]);
        
        // act
        var response = await GetService().GetByNameAsync("SomeFile", _folderId, _cancellationToken);
        
        // assert
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Count, Is.EqualTo(0));
    }

    [Test]
    public async Task Search_ShouldReturnOneFile()
    {
        // arrange
        var searchString = "Test";
        _mockedFileRepository.Setup(x => x.SearchAsync(searchString, _cancellationToken))
            .ReturnsAsync([new FileItem { Id = _fileId, Name = _fileName, FolderId = _folderId }]);

        var response = await GetService().SearchAsync(searchString, _cancellationToken);
        
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Count, Is.EqualTo(1));
    }
    
    [Test]
    public async Task Search_ShouldReturnEmptyList()
    {
        // arrange
        var searchString = "pero";
        _mockedFileRepository.Setup(x => x.SearchAsync(searchString, _cancellationToken))
            .ReturnsAsync([]);

        var response = await GetService().SearchAsync(searchString, _cancellationToken);
        
        Assert.That(response, Is.Not.Null);
        Assert.That(response.Count, Is.EqualTo(0));
    }
    
    // tests for delete file
}