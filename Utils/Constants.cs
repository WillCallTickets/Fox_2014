using System;
using System.Reflection;
using System.Collections.Generic;

namespace Utils
{
	public class Constants
	{
        public class Bootstrap
        {
            public class DateTimePicker
            {
                /// <summary>
                /// Sep 02 2013
                /// </summary>
                public const string Date_FormatString = "MMM dd yyyy";

                /// <summary>
                /// Sep 02 2012 04:30 pm
                /// </summary>
                public const string DateTime_FormatString = "MMM dd yyyy hh:mm tt";

                /// <summary>
                /// 04:30 pm
                /// </summary>
                public const string Time_FormatString = "hh:mm tt";
            }
        }

        private static DateTime _minDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
        public static DateTime _MinDate { get { return _minDate; } }
        private static System.Data.SqlTypes.SqlDateTime _nullDate = System.Data.SqlTypes.SqlDateTime.Null;        
        public static System.Data.SqlTypes.SqlDateTime _NullDate { get { return _nullDate; } }

        public static string SqlSeparator { get { return "/**************************************************************/"; } }
		public static char Separator { get { return '~'; } }

		/// <summary>
		/// returns the tab character
		/// </summary>
		public const string Tab = "\t";
        public static string Tabs(int depth)
        {
            System.Text.StringBuilder tabs = new System.Text.StringBuilder();
            for (int i = 0; i < depth; i++)
                tabs.Append(Tab);

            return tabs.ToString();
        }

		/// <summary>
		/// returns the carriage return character - appropraite for the system
		/// </summary>
		public static string NewLine = System.Environment.NewLine;
        public static string NewLines(int numberOfInstances)
        {
            //Only return specified # of newLines
            System.Text.StringBuilder lines = new System.Text.StringBuilder();
            for (int i = 0; i < numberOfInstances; i++)
                lines.Append(NewLine);

            return lines.ToString();
        }
	}
}
