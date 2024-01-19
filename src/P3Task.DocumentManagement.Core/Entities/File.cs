using System.ComponentModel.DataAnnotations.Schema;

namespace P3Task.DocumentManagement.Core.Entities;

public class File
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    [ForeignKey("folders")]
    public Guid? FolderId { get; set; }
    public Folder? Folder { get; set; }
}