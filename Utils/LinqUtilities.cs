using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Utils
{
    //https://subversion.assembla.com/svn/safeenergyweb/Datos/Servicios/LinqUtilities.cs
    public static class LinqUtilities
    {
        public enum SortDirection
        {
            Ascending,
            Descending
        }

        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string sortColumn, SortDirection direction)
        {
            return ApplyOrdering(query, ref sortColumn, direction, "OrderBy");
        }

        public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> query, string sortExpression)
        {
            return ApplyOrdering(query, ref sortExpression, GetSortDirection(ref sortExpression), "OrderBy");
        }

        public static IQueryable<T> ThenBy<T>(this IQueryable<T> query, string sortColumn, SortDirection direction)
        {
            return ApplyOrdering(query, ref sortColumn, direction, "ThenBy");
        }

        public static IQueryable<T> ThenBy<T>(this IQueryable<T> query, string sortExpression)
        {
            return ApplyOrdering(query, ref sortExpression, GetSortDirection(ref sortExpression), "ThenBy");
        }

        private static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, ref string attribute, SortDirection direction, string orderMethodName)
        {
            try
            {
                if (direction == SortDirection.Descending) orderMethodName += "Descending";

                Type t = typeof(T);

                var param = Expression.Parameter(t);
                var property = t.GetProperty(attribute);

                return query.Provider.CreateQuery<T>(
                    Expression.Call(
                        typeof(Queryable),
                        orderMethodName,
                        new Type[] { t, property.PropertyType },
                        query.Expression,
                        Expression.Quote(
                            Expression.Lambda(
                                Expression.Property(param, property),
                                param))
                    ));
            }
            catch (Exception) // Probably invalid input, you can catch specifics if you want
            {
                return query; // Return unsorted query
            }
        }

        private static SortDirection GetSortDirection(ref string sortExpression)
        {
            bool sortDescending = false;
            if (!String.IsNullOrEmpty(sortExpression))
            {
                string[] values = sortExpression.Split(' ');
                sortExpression = values[0];
                if (values.Length > 1)
                {
                    sortDescending = values[1] == "DESC";
                }
            }
            return sortDescending ? SortDirection.Descending : SortDirection.Ascending;
        }
    }
}
