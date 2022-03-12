using System;
using System.Collections.Generic;

namespace Backend.WebApi.Models
{
  public class SortOrder
  {
    public enum Order
    {
      ASCENDING, DESCENDING
    }
    /// <summary>
    /// Pair columnName, order
    /// </summary>
    public List<KeyValuePair<string, Order>> ColumnsOrder { get; set; } = new List<KeyValuePair<string, Order>>();

    public void AddSortOrder(string sort, bool ascending)
    {
      ColumnsOrder.Add(new KeyValuePair<string, Order>(sort, ascending ? Order.ASCENDING : Order.DESCENDING));
    }
  }
}