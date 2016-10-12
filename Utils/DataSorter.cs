using System;
using System.Data;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace Utils
{
	/// <summary>
	/// Summary description for DataHelper.
	/// </summary>
	public class DataSorter
	{
        public DataSorter() { }

        public enum SortDirection 
        {
        }

        //public static IEnumerable<T> Sort(IEnumerable<T> source, string SortColumn)
        //{
        //    return DataSorter.Sort(source, SortColumn, Utils.Reflector.Direction.Ascending);
        //}
        public static IEnumerable<T> Sort(IEnumerable<T> source, string SortColumn, Utils.Reflector.Direction sortDirection)
        {
            bool SortDescending = false;

            return null;
        //    if (SortExpression.Contains("DESC"))
        //    {
        //    SortDescending = true;
        //    SortExpression = SortExpression.Replace(“DESC”, “”).Trim();
        //    }

        //var param = Expression.Parameter(typeof(T), “item”);

        //var sortExpression = Expression.Lambda<Func<T, object>>
        //(Expression.Convert(Expression.Property(param, SortExpression), typeof(object)), param);

        //if (!SortDescending)
        //return source.AsQueryable<T>().OrderBy<T, object>(sortExpression);
        //else
        //return source.AsQueryable<T>().OrderByDescending<T, object>(sortExpression);
        }
	}
}
