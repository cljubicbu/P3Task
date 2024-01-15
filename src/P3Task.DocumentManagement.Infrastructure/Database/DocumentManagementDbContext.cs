using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using P3Task.DocumentManagement.Core.Entities;

namespace P3Task.DocumentManagement.Repository.Database;

public class DocumentManagementDbContext : DbContext
{
    private readonly ILogger _logger;
    
    public DocumentManagementDbContext(
        ILogger<DocumentManagementDbContext> logger)
    {
        _logger = logger;
    }

    public DbSet<FileItem> Files { get; set; }
    public DbSet<Folder> Folders { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}