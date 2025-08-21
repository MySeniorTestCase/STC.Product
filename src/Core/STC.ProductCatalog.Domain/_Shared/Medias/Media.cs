using STC.ProductCatalog.Domain._Shared.Types.Concretes;
using STC.ProductCatalog.Domain.Constants;

namespace STC.ProductCatalog.Domain._Shared.Medias;

public class Media : ValueObjectBase
{
    private Media()
    {
    }

    private Media(MediaProvider provider, string fileName)
    {
        SetProvider(provider: provider);
        SetFileName(fileName: fileName);
    }

    public MediaProvider Provider { get; private set; }
    public string FileName { get; private set; } = null!;

    public void SetFileName(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException(message: Messages.MediaFileNameCanNotBeEmpty);

        FileName = fileName;
    }

    public void SetProvider(MediaProvider provider)
    {
        if (provider is MediaProvider.Unknown)
            throw new ArgumentException(message: Messages.MediaProviderCouldNotBeDetected);

        Provider = provider;
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Provider;
        yield return FileName;
    }

    public static Media Create(MediaProvider provider, string fileName)
    {
        return new Media(provider: provider, fileName: fileName);
    }
}