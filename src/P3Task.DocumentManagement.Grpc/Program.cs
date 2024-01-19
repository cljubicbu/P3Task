using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using P3Task.DocumentManagement.Application.Interfaces;
using P3Task.DocumentManagement.Core.Interfaces;
using P3Task.DocumentManagement.Grpc;
using P3Task.DocumentManagement.Grpc.Services;
using P3Task.DocumentManagement.Repository.Database;
using P3Task.DocumentManagement.Repository.Repositories;
using FileService = P3Task.DocumentManagement.Application.Services.FileService;
using FolderService = P3Task.DocumentManagement.Application.Services.FolderService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
});

builder.Services.AddDbContext<DocumentManagementDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DocumentManagementDbConnection")));

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IFolderService, FolderService>();

builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IFolderRepository, FolderRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<FileProtoService>();
app.MapGrpcService<FolderProtoService>();
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();