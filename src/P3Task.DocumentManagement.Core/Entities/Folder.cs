using System.ComponentModel.DataAnnotations.Schema;

namespace P3Task.DocumentManagement.Core.Entities;

public class Folder
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    [ForeignKey("folders")] 
    public Guid? ParentFolderId { get; set; }
    public Folder? ParentFolder { get; set; }

    public virtual List<FileItem>? Files { get; set; }
    public virtual List<Folder>? Folders { get; set; }
}