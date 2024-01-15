using Microsoft.Extensions.Logging;

namespace P3Task.DocumentManagement.Core.Services;

public class FileService
{
    private readonly ILogger<FileService> _logger;

    public FileService(
        ILogger<FileService> logger)
    {
        _logger = logger;
    }
}