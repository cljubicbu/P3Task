using Microsoft.EntityFrameworkCore;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Repository.Database;

namespace P3Task.DocumentManagement.Repository.Repositories;

public class FileRepository
{
    private readonly DocumentManagementDbContext _dbContext;

    public FileRepository(
        DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<FileItem> AddAsync(FileItem file, CancellationToken cancellationToken)
    {
        _dbContext.Files.Add(file);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return file;
    }

    public async Task<FileItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<FileItem>> GetByNameAsync(string name, Guid? folderId, CancellationToken cancellationToken)
    {
        var filesQuery = _dbContext.Files.Where(x => x.Name == name);


        if (folderId is not null)
            filesQuery = filesQuery.Where(x => x.FolderId == folderId);

        return await filesQuery.ToListAsync(cancellationToken);
    }

    public async Task<List<FileItem>> SearchAsync(string search, CancellationToken cancellationToken)
    {
        return await _dbContext.Files.Where(x => x.Name.StartsWith(search)).Take(10).ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var file = new FileItem { Id = id };
        _dbContext.Files.Attach(file);
        _dbContext.Files.Remove(file);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}