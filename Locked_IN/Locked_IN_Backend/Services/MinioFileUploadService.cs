using Locked_IN_Backend.Interfaces;
using Locked_IN_Backend.Misc;
using Minio;
using Minio.DataModel.Args;

namespace Locked_IN_Backend.Services;

public class MinioFileUploadService : IFileUploadService
{
    private readonly IMinioClient _minioClient;
    private const string UserAvatarsBucket = "useravatars";
    private const string TeamIconsBucket = "teamicons";
    private const string AttachmentsBucket = "attachments";

    public MinioFileUploadService(IMinioClient minioClient)
    {
        _minioClient = minioClient;
    }

    public async Task<string> UploadUserAvatarAsync(IFormFile file)
    {
        var fileName = await UploadFileAsync(file, UserAvatarsBucket);
        return $"{UserAvatarsBucket}/{fileName}";
    }

    public async Task<string> UploadTeamIconAsync(IFormFile file)
    {
        var fileName = await UploadFileAsync(file, TeamIconsBucket);
        return $"{TeamIconsBucket}/{fileName}";
    }
    public async Task<string> UploadAttachmentAsync(IFormFile file)
    {
        var fileName = await UploadFileAsync(file, AttachmentsBucket);
        return $"{AttachmentsBucket}/{fileName}";
    }

    public async Task<IFormFile?> GetFileAsync(string bucketName, string fileName)
    {
        var statObjectArgs = new StatObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName);
        var stat = await _minioClient.StatObjectAsync(statObjectArgs).ConfigureAwait(false);

        var memoryStream = new MemoryStream();
        var getObjectArgs = new GetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
            });

        await _minioClient.GetObjectAsync(getObjectArgs).ConfigureAwait(false);
        memoryStream.Position = 0;
        
        return new CustomFormFile(memoryStream, "file", fileName, stat.ContentType);
    }

    public async Task DeleteFileAsync(string bucketName, string fileName)
    {
        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName);
        await _minioClient.RemoveObjectAsync(removeObjectArgs).ConfigureAwait(false);
    }

    private async Task<string> UploadFileAsync(IFormFile file, string bucketName)
    {
        if (file == null || file.Length == 0)
        {
            throw new ArgumentException("File is empty or null", nameof(file));
        }

        var becketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
        var found = await _minioClient.BucketExistsAsync(becketExistsArgs).ConfigureAwait(false);
        if (!found)
        {
            var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs).ConfigureAwait(false);
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        using var stream = file.OpenReadStream();
        
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(fileName)
            .WithStreamData(stream)
            .WithObjectSize(file.Length)
            .WithContentType(file.ContentType);

        await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);

        return fileName;
    }
}