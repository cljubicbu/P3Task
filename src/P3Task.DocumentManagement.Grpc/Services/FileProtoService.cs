using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using P3Task.DocumentManagement.Application.Interfaces;
using P3Task.DocumentManagement.Core.Entities;
using P3Task.DocumentManagement.Core.Helpers;

namespace P3Task.DocumentManagement.Grpc.Services;

public class FileProtoService : FileService.FileServiceBase
{
    private readonly IFileService _fileService;
    private readonly ILogger<FileProtoService> _logger;

    public FileProtoService(ILogger<FileProtoService> logger,
        IFileService fileService)
    {
        _logger = logger;
        _fileService = fileService;
    }

    public override async Task<CreateFileResponse> CreateFile(CreateFileRequest request, ServerCallContext context)
    {
        Guid? folderId = null;
        if (!string.IsNullOrEmpty(request.FolderId) && NullableHelper.TryParseNullable(request.FolderId, out folderId, Guid.TryParse) == false)
            throw new ArgumentException("FolderId should be a valid guid");

        var resp = await _fileService.AddAsync(new Core.Entities.File { Name = request.Name, FolderId = folderId },
            context.CancellationToken);
        
        return new CreateFileResponse
        {
            File = new File
            {
                Id = resp.Id.ToString(),
                Name = resp.Name
            }
        };
    }

    public override async Task<GetFilesByNameResponse> GetFilesByName(GetFilesByNameRequest request,
        ServerCallContext context)
    {
        Guid? folderId = null;
        if (!string.IsNullOrEmpty(request.FolderId) && NullableHelper.TryParseNullable(request.FolderId, out folderId, Guid.TryParse) == false)
            throw new ArgumentException("FolderId should be a valid guid");
        
        var files = await _fileService.GetByNameAsync(request.Name, folderId, context.CancellationToken);

        return new GetFilesByNameResponse
        {
            Files = { files.Select(x => new File { Id = x.Id.ToString(), Name = x.Name, FolderId = x.FolderId?.ToString() ?? string.Empty }).ToList() }
        };
    }

    public override async Task<SearchFilesResponse> SearchFiles(SearchFilesRequest request, ServerCallContext context)
    {
        var files = await _fileService.SearchAsync(request.Name, context.CancellationToken);

        return new SearchFilesResponse
        {
            Files = { files.Select(x => new File { Id = x.Id.ToString(), Name = x.Name, FolderId = x.FolderId?.ToString() ?? string.Empty }).ToList() }
        };
    }

    public override async Task<Empty> DeleteFile(DeleteFileRequest request, ServerCallContext context)
    {
        if (Guid.TryParse(request.Id, out var fileId) == false)
            throw new ArgumentException("File id should be a valid guid");

        await _fileService.DeleteAsync(fileId, context.CancellationToken);

        return new Empty();
    }
}