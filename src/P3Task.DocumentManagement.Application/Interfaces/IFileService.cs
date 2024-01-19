using P3Task.DocumentManagement.Core.Entities;
using File = P3Task.DocumentManagement.Core.Entities.File;

namespace P3Task.DocumentManagement.Application.Interfaces;

public interface IFileService
{
    Task<File> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<List<File>> SearchAsync(string search, CancellationToken cancellationToken);

    Task<List<File>> GetByNameAsync(string fileName, Guid? folderId,
        CancellationToken cancellationToken);

    Task<File> AddAsync(File file, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}