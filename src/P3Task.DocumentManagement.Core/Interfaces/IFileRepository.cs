using P3Task.DocumentManagement.Core.Entities;

namespace P3Task.DocumentManagement.Core.Interfaces;

public interface IFileRepository
{
    Task<FileItem> AddAsync(FileItem file, CancellationToken cancellationToken);
    Task<FileItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<FileItem>> GetByNameAsync(string name, Guid? folderId, CancellationToken cancellationToken);
    Task<List<FileItem>> SearchAsync(string search, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}