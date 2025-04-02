using Application.Abstract.Interfaces;
using Azure.Storage.Blobs;
using Infrastructure.Authentication;
using Infrastructure.BlobStorage;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var blobStorageConnectionString = configuration
            .GetConnectionString("AzureBlobStorage");

        return services
            .AddScoped<IJwtProvider, JwtProvider>()
            .AddScoped<IDateTimeProvider, DateTimeService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IResetTokenService, ResetTokenProvider>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IUserManager, UserManager>()
            .AddSingleton(x => new BlobServiceClient(blobStorageConnectionString))
            .AddSingleton<IBlobService, BlobService>()
            //.AddBlobServiceClient();
            ;
    }
}
