using P3Task.DocumentManagement.Core.Entities;
using File = P3Task.DocumentManagement.Core.Entities.File;

namespace P3Task.DocumentManagement.Core.Interfaces;

public interface IFileRepository
{
    Task<File> AddAsync(File file, CancellationToken cancellationToken);
    Task<File?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<File>> GetByNameAsync(string name, Guid? folderId, CancellationToken cancellationToken);
    Task<List<File>> SearchAsync(string search, CancellationToken cancellationToken);
    Task DeleteAsync(File file, CancellationToken cancellationToken);
}