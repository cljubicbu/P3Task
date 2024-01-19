using P3Task.DocumentManagement.Core.Entities;

namespace P3Task.DocumentManagement.Application.Interfaces;

public interface IFolderService
{
    Task<Folder> AddAsync(Folder folder, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}