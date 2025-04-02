namespace Application.Abstract.Interfaces;

public interface IBlobService
{
    Task<string> UploadAsync(string fileName, Stream content);
}
