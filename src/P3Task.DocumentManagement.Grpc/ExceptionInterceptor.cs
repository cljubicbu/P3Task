using System.Data;
using Grpc.Core;
using Grpc.Core.Interceptors;

namespace P3Task.DocumentManagement.Grpc;

public class ExceptionInterceptor : Interceptor
{
    
    private readonly ILogger<ExceptionInterceptor> _logger;
    private readonly Guid _correlationId;

    public ExceptionInterceptor(ILogger<ExceptionInterceptor> logger)
    {
        _logger = logger;
        _correlationId = Guid.NewGuid();
    }
    
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
        TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            _logger.LogDebug("CorrelationId: {CorrelationId} - Invoking {Method} with {Request}", _correlationId, context.Method, request);

            return await continuation(request, context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CorrelationId: {CorrelationId} - Exception occurred", _correlationId);

            if (e is KeyNotFoundException)
                throw new RpcException(new Status(StatusCode.NotFound, "Entry not found"));
            else if (e is DuplicateNameException)
                throw new RpcException(
                    new Status(StatusCode.AlreadyExists,"File with same name in folder already exists"));
            else
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"));
        }
    }
}