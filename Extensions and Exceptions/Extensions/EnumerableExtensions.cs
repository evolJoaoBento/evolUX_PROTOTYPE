using System.Data;
using System.Text.Json;

namespace evolUX.API.Extensions
{
    public static class EnumerableExtensions
    {
        public static string toCommaSeperatedString(this IEnumerable<string> list)
        {
            return string.Join(",",list);
        }
        public static string toCommaSeperatedString(this IEnumerable<int> list)
        {
            return string.Join(",", list);
        }
        public static string toCommaSeperatedFormatedString(this IEnumerable<string> list)
        {
            return "'" + string.Join("','", list) + "'";
        }
        public static T ConvertFromDBVal<T>(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }

        public static DataTable toDataTable(this IEnumerable<int> list)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("int");
            foreach(int i in list)
            {
                dt.Rows.Add(i);
            }
            return dt;
        }
        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            int index = 0;
            var comparer = EqualityComparer<T>.Default; // or pass in as a parameter
            foreach (T item in source)
            {
                if (comparer.Equals(item, value)) return index;
                index++;
            }
            return -1;
        }
        public static IEnumerable<T> Replace<T>(this IEnumerable<T> enumerable, int index, T value)
        {
            return enumerable.Select((x, i) => index == i ? value : x);
        }
    }
}
