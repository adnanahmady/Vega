namespace Vega.Dtos;

public class PaginationDto<TEntity>
{
    public int Count { get; set; }
    public IEnumerable<TEntity> Items { get; set; }
}
