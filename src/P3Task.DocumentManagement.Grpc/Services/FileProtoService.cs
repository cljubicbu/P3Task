using Grpc.Core;

namespace P3Task.DocumentManagement.Grpc.Services;

public class FileProtoService : FileService.FileServiceBase
{
    private readonly ILogger<FileProtoService> _logger;

    public FileProtoService(ILogger<FileProtoService> logger)
    {
        _logger = logger;
    }

    public override Task<CreateFileResponse> CreateFile(CreateFileRequest request, ServerCallContext context)
    {
        
        return base.CreateFile(request, context);
    }
}