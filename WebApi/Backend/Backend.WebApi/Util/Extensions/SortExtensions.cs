using Backend.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Backend.WebApi.Util.Extensions
{
  public static class SortExtensions
  {
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> query, SortOrder sortOrder, Dictionary<string, Expression<Func<T, object>>> orderSelectors)
    {
      if (sortOrder?.ColumnsOrder != null)
      {
        bool first = true;
        foreach (var sort in sortOrder.ColumnsOrder)
        {
          if (orderSelectors.TryGetValue(sort.Key.ToUpper(), out var orderSelector))
          {
            if (first)
            {
              query = sort.Value == SortOrder.Order.ASCENDING ?
                                          query.OrderBy(orderSelector)
                                        : query.OrderByDescending(orderSelector);
              first = false;
            }
            else
            {
              IOrderedQueryable<T> oquery = (IOrderedQueryable<T>)query;
              query = sort.Value == SortOrder.Order.ASCENDING ?
                                          oquery.ThenBy(orderSelector)
                                        : oquery.ThenByDescending(orderSelector);
            }
          }
        }
      }
      return query;
    }  
  }
}
