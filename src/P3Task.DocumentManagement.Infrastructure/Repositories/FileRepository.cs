using Microsoft.EntityFrameworkCore;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Core.Interfaces;
using P3Task.DocumentManagement.Repository.Database;
using File = P3Task.DocumentManagement.Core.Entities.File;

namespace P3Task.DocumentManagement.Repository.Repositories;

public class FileRepository : IFileRepository
{
    private readonly DocumentManagementDbContext _dbContext;

    public FileRepository(
        DocumentManagementDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<File> AddAsync(File file, CancellationToken cancellationToken)
    {
        _dbContext.Files.Add(file);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return file;
    }

    public async Task<File?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.Files.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<File>> GetByNameAsync(string name, Guid? folderId, CancellationToken cancellationToken)
    {
        var filesQuery = _dbContext.Files.Where(x => x.Name == name);
        
        if (folderId is not null)
            filesQuery = filesQuery.Where(x => x.FolderId == folderId);

        return await filesQuery.ToListAsync(cancellationToken);
    }

    public async Task<List<File>> SearchAsync(string search, CancellationToken cancellationToken)
    {
        return await _dbContext.Files.Where(x => x.Name.StartsWith(search)).Take(10).ToListAsync(cancellationToken);
    }

    public async Task DeleteAsync(File file, CancellationToken cancellationToken)
    {
        _dbContext.Files.Remove(file);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}