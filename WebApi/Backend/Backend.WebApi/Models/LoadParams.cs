using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.WebApi.Models
{
  /// <summary>
  /// Map lazy loading parameters (e.g. from PrimeNG table)
  /// </summary>   
  public class LoadParams
  {    
    /// <summary>
    /// Starting row. The row are counted from 0 (i.e. skips First rows)
    /// </summary>
    public int? First { get; set; }
    /// <summary>
    /// Number of elements to return
    /// </summary>
    public int? Rows { get; set; }
    /// <summary>
    /// Name of a column. Must be same as in corresponding DTO object, case insensitive
    /// </summary>
    public string Sort { get; set; }
    /// <summary>
    /// 1 ascending, -1 descending
    /// </summary>
    public int? SortOrder { get; set; }

    [BindNever] public bool Ascending => SortOrder == null || SortOrder != -1;

    /// <summary>
    /// Search filters. Each filter is like  key(operator)value, e.g. client(contains)Bob, agency(=)TNT, ...
    /// </summary>
    public string[] Filter { get; set; }

    /// <summary>
    /// To allow multiple sort in the future (i.e. we can replace Sort and SortOrder arguments, and introduce something else
    /// that would be parsed in this method)
    /// </summary>
    /// <returns></returns>
    public SortOrder ToSortOrder()
    {
      SortOrder order = null;
      if (!string.IsNullOrWhiteSpace(this.Sort))
      {
        order = new SortOrder();
        order.AddSortOrder(this.Sort, ascending: this.Ascending);
      }
      return order;
    }

  }
}
