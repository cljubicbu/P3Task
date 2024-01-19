using Microsoft.EntityFrameworkCore;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Core.Interfaces;
using P3Task.DocumentManagement.Repository.Database;

namespace P3Task.DocumentManagement.Repository.Repositories;

public class FolderRepository : IFolderRepository
{
    private readonly DocumentManagementDbContext _dbContext;

    public FolderRepository(
        DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Folder> CreateAsync(Folder folder, CancellationToken cancellationToken)
    {
        _dbContext.Add(folder);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return folder;
    }

    public async Task<Folder?> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool includeChildFolders = false, bool includeChildFiles = false)
    {
        var foldersQuery = _dbContext.Folders.AsQueryable();
        if (includeChildFolders)
            foldersQuery = foldersQuery.Include(x => x.Folders);

        if (includeChildFiles)
            foldersQuery = foldersQuery.Include(x => x.Files);
        
        return await foldersQuery
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task DeleteAsync(Folder folder, CancellationToken cancellationToken)
    {
        _dbContext.Folders.Remove(folder);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}