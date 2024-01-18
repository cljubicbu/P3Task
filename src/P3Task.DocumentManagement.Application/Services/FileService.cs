using Microsoft.Extensions.Logging;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Repository.Repositories;

namespace P3Task.DocumentManagement.Application.Services;

public class FileService
{
    private readonly ILogger<FileService> _logger;
    private readonly FileRepository _fileRepository;

    public FileService(
        ILogger<FileService> logger,
        FileRepository fileRepository)
    {
        _logger = logger;
        _fileRepository = fileRepository;
    }

    public async Task<List<FileItem>> SearchAsync(string fileName, CancellationToken cancellationToken)
    {
        return await _fileRepository.SearchAsync(fileName, cancellationToken);
    }

    public async Task<FileItem> AddFile(FileItem file)
    {
        return _fileRepository.AddAsync(file);
    }
}