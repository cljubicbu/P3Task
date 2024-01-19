using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Core.Helpers;

namespace P3Task.DocumentManagement.Grpc.Services;

public class FolderProtoService : FolderService.FolderServiceBase
{
    private readonly Application.Services.FolderService _folderService;

    public FolderProtoService(
        Application.Services.FolderService folderService)
    {
        _folderService = folderService;
    }

    public override async Task<CreateFolderResponse> CreateFolder(CreateFolderRequest request, ServerCallContext context)
    {
        Guid? parentFolderId = null;
        if (!string.IsNullOrEmpty(request.ParentFolderId) && NullableHelper.TryParseNullable(request.ParentFolderId, out parentFolderId, Guid.TryParse) == false)
            throw new ArgumentException("FolderId should be a valid guid");

        var folder = await _folderService.AddAsync(
            new Core.Entities.Folder() { Name = request.Name, ParentFolderId = parentFolderId },
            context.CancellationToken);
        
        return new CreateFolderResponse()
        {
            Folder = new Folder()
            {
                Id = folder.Id.ToString(),
                Name = folder.Name,
                ParentFolderId = folder.ParentFolderId?.ToString()
            }
        };
    }

    public override async Task<Empty> DeleteFolder(DeleteFolderRequest request, ServerCallContext context)
    {
        if (Guid.TryParse(request.Id, out var folderId) == false)
            throw new ArgumentException("File id should be a valid guid");

        await _folderService.DeleteAsync(folderId, context.CancellationToken);

        return new Empty();
    }
}