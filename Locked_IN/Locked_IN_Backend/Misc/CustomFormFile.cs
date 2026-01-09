using Microsoft.AspNetCore.Http;

namespace Locked_IN_Backend.Misc;

public class CustomFormFile : IFormFile
{
    private readonly Stream _baseStream;

    public CustomFormFile(Stream baseStream, string name, string fileName, string contentType)
    {
        _baseStream = baseStream;
        ContentDisposition = $"form-data; name=\"{name}\"; filename=\"{fileName}\"";
        ContentType = contentType;
        FileName = fileName;
        Name = name;
        Length = _baseStream.Length;
    }

    public string ContentType { get; }
    public string ContentDisposition { get; }
    public IHeaderDictionary Headers => new HeaderDictionary();
    public long Length { get; }
    public string Name { get; }
    public string FileName { get; }

    public void CopyTo(Stream target)
    {
        _baseStream.Position = 0;
        _baseStream.CopyTo(target);
    }

    public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default)
    {
        _baseStream.Position = 0;
        await _baseStream.CopyToAsync(target, cancellationToken);
    }

    public Stream OpenReadStream()
    {
        _baseStream.Position = 0;
        return _baseStream;
    }
}
