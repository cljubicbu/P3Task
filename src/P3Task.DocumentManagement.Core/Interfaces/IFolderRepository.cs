using P3Task.DocumentManagement.Core.Entities;

namespace P3Task.DocumentManagement.Core.Interfaces;

public interface IFolderRepository
{
    Task<Folder> CreateAsync(Folder folder, CancellationToken cancellationToken);
    Task<Folder?> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool includeChildFolders = false);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}