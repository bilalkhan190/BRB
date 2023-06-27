using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BusinessObjects.Helper
{
    public static class SqlHelper
    {
        public static DataSet GetDataSet(string cs, string command, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(command, con);
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(sqlParameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        public static DataTable GetDataTable(string cs, string command, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(command, con);
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(sqlParameters);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetDataTable(string cs, string command, CommandType commandType)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(command, con);
                cmd.CommandType = commandType;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public static int ExecuteNonQuery(string cs, string command, CommandType commandType, params SqlParameter[] sqlParameters)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(command, con);
                cmd.CommandType = commandType;
                cmd.Parameters.AddRange(sqlParameters);
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }
        public static int ExecuteNonQuery(string cs, string command, CommandType commandType)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                SqlCommand cmd = new SqlCommand(command, con);
                cmd.CommandType = commandType;
                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }


        public static int ToInt_(this object val)
        {
            if (val == null) return 0;
            return Convert.ToInt32(val);
        }
        public static long ToLong_(this object val)
        {
            if (val == null) return 0;
            return Convert.ToInt64(val);
        }
        public static decimal ToDecimal_(this object val)
        {
            if (val == null) return 0;
            return Convert.ToDecimal(val);
        }
        public static double ToDouble_(this object val)
        {
            if (val == null) return 0;
            return Convert.ToDouble(val);
        }
        public static bool ToBool_(this object val)
        {
            if (val == null) return false;
            return Convert.ToBoolean(val);
        }
        public static DateTime? ToDateTime_(this object val)
        {
            if (val == null) return null;
            return Convert.ToDateTime(val, CultureInfo.InvariantCulture);
        }

        //public static DateTime? ToDateTime_(this object val,bool isCultureInfo)
        //{
        //    if (val == null) return null;
        //    return Convert.ToDateTime(val);
        //}

        //


        public static List<T> ToList_<T>(this DataTable dt) =>
             dt.SerializeObjectJson_(ReferenceLoopHandling.Ignore).DeserializeObjectJson_<List<T>>();

        public static string SerializeObjectJson_(this object obj, ReferenceLoopHandling referenceLoopHandling)
        {
            string JsonStr = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented,
                            new JsonSerializerSettings
                            {
                                ReferenceLoopHandling = referenceLoopHandling
                            });
            return JsonStr;
        }
        public static string SerializeObjectJson_(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T DeserializeObjectJson_<T>(this string JsonStr)
        {
            return JsonConvert.DeserializeObject<T>(JsonStr);
        }

        public static To MapTo_<To>(this object FromModel)
        {
            string JsonStr = FromModel.SerializeObjectJson_(ReferenceLoopHandling.Ignore);
            return JsonConvert.DeserializeObject<To>(JsonStr);
        }

        public static T IgnoreSelfReferencing_<T>(this T model)
        {
            return model.SerializeObjectJson_(ReferenceLoopHandling.Ignore).DeserializeObjectJson_<T>();
        }

    }
}
