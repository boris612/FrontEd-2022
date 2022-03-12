using Backend.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Backend.WebApi.Util.Extensions
{
  public static class FilterExtensions
  {
    private static readonly MethodInfo Contains = typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) });
    private static readonly MethodInfo StartsWith = typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) });
    private static readonly MethodInfo EndsWith = typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) });

    private class SearchOperators
    {
      public const string EqualSign = "=";
      public const string Equal = "equals";
      public const string NotEqual = "<>";
      public const string Less = "<";
      public const string Greater = ">";
      public const string LessOrEqual = "<=";
      public const string GreaterOrEqual = ">=";

      public const string Contains = "contains";
      public const string StartsWith = "startswith";
      public const string EndsWith = "endswith";
      public const string IsNull = "isnull";
      public const string NotNull = "notnull";
    }

    public static IQueryable<T> ApplyFilter<T>(this IQueryable<T> query, List<Filter> filters, Dictionary<string, Expression<Func<T, object>>> orderSelectors)
    {
      if (filters != null)
      {
        foreach (var filter in filters)
        {
          if (orderSelectors.TryGetValue(filter.Column.ToUpper(), out Expression<Func<T, object>> orderSelector))
          {
            var predicate = BuildWhereExpression(orderSelector, filter.Operator, filter.Value);
            if (predicate != null)
            {
              query = query.Where(predicate);
            }
          }
          else
          {
            throw new Exception("Missing order selector for " + filter.Column);
          }
        }
      }
      return query;
    }

    private static Expression<Func<T, bool>> BuildWhereExpression<T>(Expression<Func<T, object>> columnSelector, string @operator, string value)
    {
      if (value == null && @operator != SearchOperators.IsNull && @operator != SearchOperators.NotNull)
      {
        throw new ArgumentException("Value must be non-null except when used with isnull and notnull");
      }
      else if (value != null && (@operator == SearchOperators.IsNull || @operator == SearchOperators.NotNull))
      {
        throw new ArgumentException("Value must be null when used with isnull and notnull");
      }

      MemberExpression left;
      if (columnSelector.Body is UnaryExpression unaryExpression)
      {
        left = unaryExpression.Operand as MemberExpression; //due to value types
      }
      else
      {
        left = columnSelector.Body as MemberExpression;
      }

      PropertyInfo propertyInfo = left.Member as PropertyInfo;
      ConstantExpression constant;
      if (propertyInfo.PropertyType.IsValueType)
      {
        try
        {
          if (propertyInfo.PropertyType == typeof(Guid)
            || propertyInfo.PropertyType == typeof(Guid?))
          {
            constant = Expression.Constant(value == null ? null : Guid.Parse(value), propertyInfo.PropertyType);
          }
          else if (Nullable.GetUnderlyingType(propertyInfo.PropertyType) != null)
          {
            Type typeInNullable = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
            object converted = value == null ? null :
              Convert.ChangeType(value, typeInNullable);
            constant = Expression.Constant(converted, propertyInfo.PropertyType);
          }
          else
          {
            //cannot be null, thus there is no reason to check
            object converted = Convert.ChangeType(value, propertyInfo.PropertyType);
            constant = Expression.Constant(converted);
          }
        }
        catch (Exception exc)
        {
          string message = $"Cannot convert value {value} for field {columnSelector} to {propertyInfo.PropertyType.Name} in {typeof(T).Name} : {exc.Message}";
          //this must throws an exception, otherwise someone could send a value for tenantId in invalidFormat, and it would not added as a filter                   
          throw new ArgumentException(message);
        }
      }
      else
      {
        constant = Expression.Constant(value);
      }

      if (constant == null) return null;

      Expression body;
      switch (@operator)
      {
        case SearchOperators.EqualSign:
        case SearchOperators.Equal:
        case SearchOperators.IsNull:
          body = Expression.Equal(left, constant);
          break;
        case SearchOperators.Less:
          body = Expression.LessThan(left, constant);
          break;
        case SearchOperators.LessOrEqual:
          body = Expression.LessThanOrEqual(left, constant);
          break;
        case SearchOperators.GreaterOrEqual:
          body = Expression.GreaterThanOrEqual(left, constant);
          break;
        case SearchOperators.Greater:
          body = Expression.GreaterThan(left, constant);
          break;
        case SearchOperators.NotEqual:
        case SearchOperators.NotNull:
          body = Expression.NotEqual(left, constant);
          break;
        case SearchOperators.Contains:
          body = Expression.Call(left, Contains, constant);
          break;
        case SearchOperators.StartsWith:
          body = Expression.Call(left, StartsWith, constant);
          break;
        case SearchOperators.EndsWith:
          body = Expression.Call(left, EndsWith, constant);
          break;
        default:
          throw new Exception($"Invalid operator ({@operator}) for path {columnSelector} and value={value} for type {typeof(T).Name}");
      }

      // if (negate) body = Expression.Not(ret);

      Expression<Func<T, bool>> predicate = null;
      if (body != null)
      {
        predicate = Expression.Lambda<Func<T, bool>>(body, columnSelector.Parameters.First());
      }
      return predicate;
    }
  }
}
