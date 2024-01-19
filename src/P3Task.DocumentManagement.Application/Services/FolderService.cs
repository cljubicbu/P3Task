using Microsoft.Extensions.Logging;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Repository.Repositories;

namespace P3Task.DocumentManagement.Application.Services;

public class FolderService
{
    private readonly ILogger<FolderService> _logger;
    private readonly FolderRepository _folderRepository;
    
    public FolderService(
        ILogger<FolderService> logger,
        FolderRepository folderRepository)
    {
        _logger = logger;
        _folderRepository = folderRepository;
    }

    public async Task<Folder> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetByIdAsync(id, cancellationToken);

        if (folder is null)
            throw new KeyNotFoundException();

        return folder;
    }
    
    public async Task<Folder> AddAsync(Folder folder, CancellationToken cancellationToken)
    {
        return await _folderRepository.CreateAsync(folder, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var folder = await _folderRepository.GetByIdAsync(id, cancellationToken, true);

        if (folder is null)
            throw new KeyNotFoundException();

        var deleteFolderTasks = new List<Task>();
        if(folder.Folders is not null) 
            deleteFolderTasks.AddRange(folder.Folders.Select(f => this.DeleteAsync(f.Id, cancellationToken)));

        await Task.WhenAll(deleteFolderTasks);

        await _folderRepository.DeleteFolderAsync(id, cancellationToken);
    }
}