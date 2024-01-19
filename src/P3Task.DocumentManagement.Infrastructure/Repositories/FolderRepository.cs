using Microsoft.EntityFrameworkCore;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Repository.Database;

namespace P3Task.DocumentManagement.Repository.Repositories;

public class FolderRepository
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

    public async Task<Folder?> GetByIdAsync(Guid id, CancellationToken cancellationToken, bool includeChildFolders = false)
    {
        return await _dbContext.Folders
            .Include(x => x.Folders)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task DeleteFolderAsync(Guid id, CancellationToken cancellationToken)
    {
        var folder = new Folder() { Id = id };

        _dbContext.Folders.Attach(folder);
        _dbContext.Folders.Remove(folder);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}