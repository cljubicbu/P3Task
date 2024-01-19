using Microsoft.Extensions.Logging;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Repository.Repositories;

namespace P3Task.DocumentManagement.Application.Services;

public class FileService
{
    private readonly FileRepository _fileRepository;
    private readonly ILogger<FileService> _logger;

    public FileService(
        ILogger<FileService> logger,
        FileRepository fileRepository)
    {
        _logger = logger;
        _fileRepository = fileRepository;
    }

    public async Task<FileItem> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetByIdAsync(id, cancellationToken);

        if (file is null)
            throw new KeyNotFoundException();

        return file;
    }

    public async Task<List<FileItem>> SearchAsync(string search, CancellationToken cancellationToken)
    {
        return await _fileRepository.SearchAsync(search, cancellationToken);
    }

    public async Task<List<FileItem>> GetByNameAsync(string fileName, Guid? folderId,
        CancellationToken cancellationToken)
    {
        return await _fileRepository.GetByNameAsync(fileName, folderId, cancellationToken);
    }

    public async Task<FileItem> AddAsync(FileItem file, CancellationToken cancellationToken)
    {
        return await _fileRepository.AddAsync(file, cancellationToken);
    }

    public async Task DeleteFile(Guid id, CancellationToken cancellationToken)
    {
        await _fileRepository.DeleteAsync(id, cancellationToken);
    }
}