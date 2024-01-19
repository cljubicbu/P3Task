namespace P3Task.DocumentManagement.Core.Entities;

public class FileItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public Guid? FolderId { get; set; }
    public Folder? Folder { get; set; }
}