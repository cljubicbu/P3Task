using Microsoft.EntityFrameworkCore;
using P3Task.DocumentManagement.Core.Entities;
using File = P3Task.DocumentManagement.Core.Entities.File;

namespace P3Task.DocumentManagement.Repository.Database;

public class DocumentManagementDbContext : DbContext
{
    public DocumentManagementDbContext(
        DbContextOptions<DocumentManagementDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<File> Files { get; set; } = null!;
    public DbSet<Folder> Folders { get; set; } = null!;
}