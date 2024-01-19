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
        // serilog - log context for correlation id
        try
        {
            return await continuation(request, context);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "CorrelationId: {CorrelationId} - Exception occurred", _correlationId);

            // handle exceptions
            throw;
        }
    }
}