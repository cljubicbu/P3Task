using System.Data;
using Microsoft.Extensions.Logging;
using P3Task.DocumentManagement.Application.Interfaces;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Core.Interfaces;
using File = P3Task.DocumentManagement.Core.Entities.File;

namespace P3Task.DocumentManagement.Application.Services;

public class FileService : IFileService
{
    private readonly IFileRepository _fileRepository;
    private readonly ILogger<FileService> _logger;

    public FileService(
        ILogger<FileService> logger,
        IFileRepository fileRepository)
    {
        _logger = logger;
        _fileRepository = fileRepository;
    }

    public async Task<File> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetByIdAsync(id, cancellationToken);

        if (file is null)
            throw new KeyNotFoundException();

        return file;
    }

    public async Task<List<File>> SearchAsync(string search, CancellationToken cancellationToken)
    {
        return await _fileRepository.SearchAsync(search, cancellationToken);
    }

    public async Task<List<File>> GetByNameAsync(string fileName, Guid? folderId,
        CancellationToken cancellationToken)
    {
        return await _fileRepository.GetByNameAsync(fileName, folderId, cancellationToken);
    }

    public async Task<File> AddAsync(File file, CancellationToken cancellationToken)
    {
        var existingFiles = await _fileRepository.GetByNameAsync(file.Name, file.FolderId, cancellationToken);
        if (existingFiles.Count > 0)
            throw new DuplicateNameException();
        
        return await _fileRepository.AddAsync(file, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var file = await _fileRepository.GetByIdAsync(id, cancellationToken);

        if (file is null)
            throw new KeyNotFoundException();
        
        await _fileRepository.DeleteAsync(file, cancellationToken);
    }
}