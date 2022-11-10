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
    }
}
