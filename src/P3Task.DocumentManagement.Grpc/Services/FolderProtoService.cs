// using Google.Protobuf.WellKnownTypes;
// using Grpc.Core;
//
// namespace P3Task.DocumentManagement.Grpc.Services;
//
// public class FolderProtoService : FolderSer
// {
//     private readonly Application.Services.FolderService _folderService;
//
//     public FolderProtoService(
//         Application.Services.FolderService folderService)
//     {
//         _folderService = folderService;
//     }
//
//     public override Task<Folder> CreateFolder(CreateFolderRequest request, ServerCallContext context)
//     {
//         var folder = await _folderService.AddAsync(
//             new Core.Entities.Folder() { Name = request.Name, ParentFolderId = request.FolderId },
//             context.CancellationToken);
//         return base.CreateFolder(request, context);
//     }
//
//     public override Task<Empty> DeleteFolder(DeleteFolderRequest request, ServerCallContext context)
//     {
//         return base.DeleteFolder(request, context);
//     }
// }