using Microsoft.Extensions.Logging;
using Moq;
using P3Task.DocumentManagement.Application.Services;
using P3Task.DocumentManagement.Core.Interfaces;

namespace P3Task.DocumentManagement.Services.Tests;

public class FolderServiceUnitTests
{
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
        
    }
}