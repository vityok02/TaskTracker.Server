using Application.Abstract.Interfaces;
using Azure.Storage.Blobs;
using Infrastructure.Authentication;
using Infrastructure.BlobStorage;
using Infrastructure.Configuration;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var blobStorageConnectionString = configuration
            .GetConnectionString("AzureBlobStorage");

        services
            .AddSignalR(options => options.EnableDetailedErrors = true)
                .AddMessagePackProtocol();

        services
            .AddScoped<IJwtProvider, JwtProvider>()
            .AddScoped<IDateTimeProvider, DateTimeService>()
            .AddScoped<IPasswordHasher, PasswordHasher>()
            .AddScoped<IResetTokenService, ResetTokenProvider>()
            .AddScoped<IEmailService, EmailService>()
            .AddScoped<IUserManager, UserManager>()
            .AddSingleton(x => new BlobServiceClient(blobStorageConnectionString))
            .AddSingleton<IBlobService, BlobService>()
            .AddSingleton<TwilioService>()
            .AddResponseCompression(opts =>
                opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
                    ["application/octet-stream"]))

            ;

        services
            .Configure<TwilioSettings>(configuration.GetSection("Twilio"));

        return services;
    }
}
