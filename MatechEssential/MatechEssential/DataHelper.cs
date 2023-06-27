using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;


namespace MatechEssential
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute()
        {
        }
    }
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class AutoIncrementAttribute : Attribute
    {
        public AutoIncrementAttribute()
        {
        }
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NonDbAttribute : Attribute
    {
        public NonDbAttribute()
        {
        }
    }
    public class DataHelper
    {
        #region MyRegion
        public static Dictionary<string, TValue> ToDictionary<TValue>(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var dictionary = JsonConvert.DeserializeObject<Dictionary<string, TValue>>(json);
            return dictionary;
        }
        #endregion
        public static Dictionary<string, object> ObjectToDictionary(object o, bool IncludeAutoId = true)
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            Type objType = o.GetType();
            foreach (PropertyInfo propertyInfo in objType.GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    bool OkToAdd = true;
                    if (propertyInfo.GetCustomAttribute(typeof(AutoIncrementAttribute)) != null && IncludeAutoId != false)
                    {
                        OkToAdd = false;
                    }
                    if (OkToAdd)
                        ret.Add(propertyInfo.Name, propertyInfo.GetValue(o));
                }
            }
            return ret;
        }

        public static T DictionaryToObject<T>(Dictionary<string, object> o)
        {
            T obj = (T)Activator.CreateInstance(typeof(T));
            foreach (KeyValuePair<string, object> i in o)
            {
                PropertyInfo prop = FindProperty<T>(i.Key);
                if (prop != null)
                {
                    if (prop.CanWrite)
                    {
                        prop.SetValue(obj, i.Value);
                    }
                }
            }
            return obj;
        }

        //public static List<T> DataTableToList<T>(DataTable table)
        //{
        //    List<T> list = new List<T>();

        //    for (int j = 0; j <= table.Rows.Count - 1; j++)
        //    {
        //        T obj = (T)Activator.CreateInstance(typeof(T));
        //        for (int i = 0; i <= table.Columns.Count - 1; i++)
        //        {
        //            PropertyInfo prop = FindProperty<T>(table.Columns[i].ColumnName);
        //            prop.SetValue(obj, table.Rows[j][i]);
        //        }
        //        list.Add(obj);
        //    }
        //    return list;
        //}

        public static List<T> DataTableToList<T>(DataTable table)
        {
            List<T> list = new List<T>();

            for (int j = 0; j <= table.Rows.Count - 1; j++)
            {
                T obj = DataRowToObject<T>(table.Rows[j]);
                list.Add(obj);
            }
            return list;
        }

        public static T DataRowToObject<T>(DataRow dr)
        {
            DataTable table = dr.Table;

           T obj = (T)Activator.CreateInstance(typeof(T));
            for (int i = 0; i <= table.Columns.Count - 1; i++)
            {
                PropertyInfo prop = FindProperty<T>(table.Columns[i].ColumnName);
                if (prop != null)
                {
                    prop.SetValue(obj, (dr[i] == DBNull.Value ? null : prop.PropertyType == typeof(bool) ? Convert.ToBoolean(dr[i]) : dr[i]));
                }
            }

            return obj;
        }

        public static PropertyInfo FindProperty<T>(string name)
        {
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                if (propertyInfo.Name.ToLower() == name.ToLower())
                {
                    return propertyInfo;
                }
            }
            return null;
        }

        public static string[] ObjectToArray(object obj)
        {
            Type t = obj.GetType();
            List<string> a = new List<string>();
            foreach (PropertyInfo propertyInfo in t.GetProperties())
            {
                object val = propertyInfo.GetValue(obj);
                a.Add((val == null) ? "" : val.ToString());
            }
            return a.ToArray();
        }

        public static object[] RowToArray(DataRow row)
        {

            List<object> a = new List<object>();
            object[] arr = row.ItemArray.ToList().ToArray();
            foreach (object i in arr)
            {
                a.Add(i.ToString());
            }
            return a.ToArray();
        }

        public static object GetPropertyValue(string propertyName, object o)
        {
            object ret = null;
            foreach (PropertyInfo propertyInfo in o.GetType().GetProperties())
            {
                if (propertyInfo.Name.ToLower() == propertyName.ToLower())
                {
                    ret = propertyInfo.GetValue(o);
                    break;
                }
            }
            return ret;
        }

        public static List<object[]> ListOfArray<T>(List<T> items)
        {
            List<object[]> l = new List<object[]>();
            foreach (T i in items)
            {
                l.Add(DataHelper.ObjectToArray(i));
            }
            return l;
        }

        public static List<object[]> ListOfArray(IEnumerable<DataRow> rows)
        {
            List<object[]> l = new List<object[]>();
            foreach (DataRow dr in rows)
            {
                l.Add(DataHelper.RowToArray(dr));
            }
            return l;
        }

        public static List<object[]> ListOfArray(DataTable table)
        {
            return ListOfArray(table.AsEnumerable());
        }

        public static object DataTableToObjectArray(DataTable dt)
        {
            return dt.AsDynamicEnumerable();
        }

        //public static object JQDataTable(DataTable dt, JQDTRequest request)
        //{
        //    var obj = new { 
        //        iTotalRecords = dt.Rows.Count,
        //        iTotalDisplayRecords = dt.Rows.Count,
        //        aaData = DataHelper.ListOfArray(dt).Skip(request.Start).Take(request.Length)
        //    };
        //    return obj;
        //}


    }

    public static class DataRowX
    {
        public static T ToObject<T>(this DataRow dr)
        {
            return DataHelper.DataRowToObject<T>(dr);
        }
        public static object ToJSON(this DataRow row)
        {
            var rowObj = new Dictionary<string, object>();
            foreach (DataColumn col in row.Table.Columns)
            {

                if (col.DataType == typeof(DateTime))
                {
                    if (row[col] != DBNull.Value)
                    {
                        rowObj.Add(col.ColumnName, Convert.ToDateTime(row[col].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                    }
                    else
                    {
                        rowObj.Add(col.ColumnName, row[col]);
                    }

                }
                else
                {
                    if (row[col].ToString() == "True" || row[col].ToString() == "False")
                    {
                        rowObj.Add(col.ColumnName, Convert.ToBoolean(row[col].ToString()));
                    }
                    else
                    {
                        rowObj.Add(col.ColumnName, row[col]);
                    }
                }
            }
            return rowObj;
            
            
            ////JavaScriptSerializer serializer = new JavaScriptSerializer();
            //var _row = new Dictionary<string, object>();
            //foreach (DataColumn col in row.Table.Columns)
            //{
            //    _row.Add(col.ColumnName, row[col]);
            //}
            //return JsonConvert.SerializeObject(_row);
        }
    }

    public static class DataSetX
    {
        public static Dictionary<string, object> ToJson(this DataSet ds, bool ArrayForSingleRow = false)
        {
            var tables = new Dictionary<string, object>();
            List<string> TableNames = new List<string>();
            if (ds.Tables[0].Columns[0].ColumnName == "TableName")
            {
                foreach(DataRow dr in ds.Tables[0].Rows)
                {
                    TableNames.Add(dr["TableName"].ToString());
                }
                ds.Tables.RemoveAt(0);
            }
            else
            {
                for(int i = 0; i < ds.Tables.Count; i++)
                {
                    TableNames.Add("TableName" + i.ToString());
                }
            }
            foreach (DataTable dt in ds.Tables)
            {
                tables.Add(TableNames[ds.Tables.IndexOf(dt)], dt.ToJson(ArrayForSingleRow));
            }
            return tables;
        }

        public static DataResult ToDataResult(this DataSet ds, bool ArrayForRow = false)
        {
            DataResult result = new DataResult();
            result = ds.Tables[0].Rows[0].ToObject<DataResult>();
            if (ds.Tables.Count > 1)
            {
                ds.Tables.RemoveAt(0);
                result.Data = ds.ToJson(ArrayForRow);
            }
            return result;
        }

        public static object JQDTResult(this DataSet ds, JQDTRequest jqRequest)
        {
            var data = new
            {
                draw = jqRequest.draw,
                recordsTotal = Convert.ToInt32(ds.Tables[1].Rows[0]["TotalRecords"].ToString()),
                recordsFiltered = Convert.ToInt32(ds.Tables[1].Rows[0]["FilteredRecords"].ToString()),
                data = ds.Tables[0].ToListOfArray()
            };
            return data;
        }
    }
    public class DataResult
    {
        public Int64 ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
    }
    public static class DataTableX
    {
        public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable table)
        {
            // Validate argument here..

            return table.AsEnumerable().Select(row => new DynamicRow(row));
        }

        //public static object ToJQDataTable(this DataTable table, JQDTRequest request)
        //{
        //    return DataHelper.JQDataTable(table, request);
        //}

        public static object ToListOfArray(this DataTable table)
        {
            return DataHelper.ListOfArray(table);
        }

        public static List<T> ToList<T>(this DataTable table)
        {
            return DataHelper.DataTableToList<T>(table);
        }

        public static object ToJson(this DataTable table, bool ArrayForSingleRow = false)
        {
            var rows = new List<object>();
            if (table.Rows.Count == 1 && ArrayForSingleRow == false)
            {
                return table.Rows[0].ToJSON();
            }
            else if(table.Rows.Count==0 && ArrayForSingleRow == false)
            {
                return null;
            }
            else
            {
                foreach (DataRow dr in table.Rows)
                {
                    var row = new Dictionary<string, object>();
                    foreach (DataColumn col in table.Columns)
                    {
                        if(col.DataType == typeof(DateTime))
                        {
                            if (dr[col] != DBNull.Value)
                            {
                                row.Add(col.ColumnName, Convert.ToDateTime(dr[col].ToString()).ToString("yyyy-MM-dd HH:mm:ss"));
                            }
                            else
                            {
                                row.Add(col.ColumnName, dr[col]);
                            }
                        }
                        else
                        {
                            if (dr[col].ToString() == "True" || dr[col].ToString() == "False")
                            {
                                row.Add(col.ColumnName, Convert.ToBoolean(dr[col].ToString()));
                            }
                            else
                            {

                                row.Add(col.ColumnName, dr[col]);
                            }
                        }
                        
                    }
                    rows.Add(row);
                }
                return rows;
            }
        }


        public static Dictionary<string,object> ToDictionary(this DataRow row)
        {
            var _row = new Dictionary<string, object>();
            foreach (DataColumn col in row.Table.Columns)
            {
                _row.Add(col.ColumnName, row[col].ToString());
            }
            return _row;
        }

        public static object[] JsonObjects(this DataTable table) {
            List<object> retObjects = new List<object>();

            foreach (DataRow dr in table.Rows) {
                var _row = new Dictionary<string, object>();
                foreach (DataColumn col in dr.Table.Columns)
                {
                    _row.Add(col.ColumnName, dr[col]);
                }
                retObjects.Add(_row);
            }

            return retObjects.ToArray();
        }

        private sealed class DynamicRow : DynamicObject
        {
            private readonly DataRow _row;

            internal DynamicRow(DataRow row) { _row = row; }

            // Interprets a member-access as an indexer-access on the 
            // contained DataRow.
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var retVal = _row.Table.Columns.Contains(binder.Name);
                result = retVal ? _row[binder.Name] : null;
                return retVal;
            }
        }


    }

    public class JQDTRequest
    {
        public string Param1 { get; set; }
        public string Param2 { get; set; }
        public string Param3 { get; set; }
        public string Param4 { get; set; }
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public JQDTSearch search { get; set; } = new JQDTSearch();
    }

    public class JQDTSearch
    {
        public string value { get; set; }
        public Boolean regex { get; set; }
    }
   

}
