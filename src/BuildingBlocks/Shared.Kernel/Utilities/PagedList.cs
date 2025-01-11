namespace AWC.Shared.Kernel.Utilities;

public class PagedList<T>(MetaData metaData, List<T> data)
{
    public MetaData? MetaData { get; set; } = metaData;
    public List<T> Data { get; set; } = data;
}
