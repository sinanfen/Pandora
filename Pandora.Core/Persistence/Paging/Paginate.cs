using Pandora.Core.Domain.Paging;

namespace Pandora.Core.Persistence.Paging;

public class Paginate<T> : IPaginate<T>
{
    public int From { get; set; }

    public int Index { get; set; }

    public int Size { get; set; }

    public int Count { get; set; }

    public int Pages { get; set; }

    public IList<T> Items { get; set; }

    public bool HasPrevious => Index - From > 0;

    public bool HasNext => Index - From + 1 < Pages;

    public Paginate(IEnumerable<T> source, int index, int size, int from)
    {
        if (from > index)
        {
            throw new ArgumentException($"indexFrom: {from} > pageIndex: {index}, must indexFrom <= pageIndex");
        }

        Index = index;
        Size = size;
        From = from;
        Pages = (int)Math.Ceiling((double)Count / (double)Size);
        if (source is IQueryable<T> source2)
        {
            Count = source2.Count();
            Items = source2.Skip((Index - From) * Size).Take(Size).ToList();
        }
        else
        {
            T[] source3 = (source as T[]) ?? source.ToArray();
            Count = source3.Count();
            Items = source3.Skip((Index - From) * Size).Take(Size).ToList();
        }
    }

    public Paginate()
    {
        Items = Array.Empty<T>();
    }
}