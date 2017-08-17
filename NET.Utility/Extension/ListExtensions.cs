using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace System
{
    public static class ListExtensions
    {
        /// <summary>
        ///     空集合转换为DataTable结构
        /// </summary>
        /// <param name="list">空集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this IList list)
        {
            var result = new DataTable();
            if (list.Count <= 0) return result;
            var propertys = list[0].GetType().GetProperties();
            foreach (var pi in propertys)
            {
                if (pi != null)
                {
                    result.Columns.Add(pi.Name, pi.PropertyType);
                }
            }
            foreach (var t in list)
            {
                var tempList = new ArrayList();
                foreach (var pi in propertys)
                {
                    var obj = pi.GetValue(t, null);
                    tempList.Add(obj);
                }
                var array = tempList.ToArray();
                result.LoadDataRow(array, true);
            }
            return result;
        }

        /// <summary>
        ///     非空集合转换为DataTable结构
        /// </summary>
        /// <typeparam name="T">集合类型</typeparam>
        /// <param name="list">非空集合</param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(this IList<T> list)
        {
            var ds = new DataSet();
            var dt = new DataTable(typeof (T).Name);
            var myPropertyInfo =
                typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var t in list)
            {
                if (t == null) continue;
                var row = dt.NewRow();
                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    var pi = myPropertyInfo[i];
                    var name = pi.Name;
                    if (dt.Columns[name] != null) continue;
                    DataColumn column;
                    if (pi.PropertyType.UnderlyingSystemType.ToString() == "System.Nullable`1[System.Int32]")
                    {
                        column = new DataColumn(name, typeof (int));
                        dt.Columns.Add(column);
                        if (pi.GetValue(t, null) != null)
                            row[name] = pi.GetValue(t, null);
                        else
                            row[name] = DBNull.Value;
                    }
                    else
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                        row[name] = pi.GetValue(t, null);
                    }
                }
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            return ds.Tables[0];
        }

        /// <summary>
        ///     表中有数据或无数据时使用,可排除DATASET不支持System.Nullable错误
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(this IList<T> list)
        {
            if (list == null || list.Count <= 0)
            {
                var result = new DataTable();
                if (list == null || list.Count <= 0) return result;
                var propertys = list[0].GetType().GetProperties();
                foreach (var pi in propertys)
                {
                    {
                        result.Columns.Add(pi.Name, pi.PropertyType);
                    }
                }
                foreach (var t in list)
                {
                    var tempList = new ArrayList();
                    foreach (var pi in propertys)
                    {
                        var obj = pi.GetValue(t, null);
                        tempList.Add(obj);
                    }
                    var array = tempList.ToArray();
                    result.LoadDataRow(array, true);
                }
                return result;
            }
            var ds = new DataSet();
            var dt = new DataTable(typeof (T).Name);
            var myPropertyInfo =
                typeof (T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var t in list)
            {
                if (t == null) continue;
                var row = dt.NewRow();
                for (int i = 0, j = myPropertyInfo.Length; i < j; i++)
                {
                    var pi = myPropertyInfo[i];
                    var name = pi.Name;
                    if (dt.Columns[name] != null) continue;
                    DataColumn column;
                    if (pi.PropertyType.UnderlyingSystemType.ToString() == "System.Nullable`1[System.Int32]")
                    {
                        column = new DataColumn(name, typeof (int));
                        dt.Columns.Add(column);
                        if (pi.GetValue(t, null) != null)
                            row[name] = pi.GetValue(t, null);
                        else
                            row[name] = DBNull.Value;
                    }
                    else
                    {
                        column = new DataColumn(name, pi.PropertyType);
                        dt.Columns.Add(column);
                        row[name] = pi.GetValue(t, null);
                    }
                }
                dt.Rows.Add(row);
            }
            ds.Tables.Add(dt);
            return ds.Tables[0];
        }

        /// <summary>
        ///     合并相同的DataTable
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns></returns>
        public static DataTable MergeSameDatatable(this DataTable dt1, DataTable dt2)
        {
            var dt = dt1.Clone();
            var obj = new object[dt.Columns.Count];
            for (var i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }
            for (var i = 0; i < dt2.Rows.Count; i++)
            {
                dt2.Rows[i].ItemArray.CopyTo(obj, 0);
                dt.Rows.Add(obj);
            }
            return dt;
        }

        /// <summary>
        ///     将两个列不同的DataTable合并成一个新的DataTable
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <param name="dtName"></param>
        /// <returns></returns>
        public static DataTable UniteDataTable(this DataTable dt1, DataTable dt2, string dtName)
        {
            var dt3 = dt1.Clone();
            for (var i = 0; i < dt2.Columns.Count; i++)
            {
                dt3.Columns.Add(dt2.Columns[i].ColumnName);
            }
            var obj = new object[dt3.Columns.Count];

            for (var i = 0; i < dt1.Rows.Count; i++)
            {
                dt1.Rows[i].ItemArray.CopyTo(obj, 0);
                dt3.Rows.Add(obj);
            }

            if (dt1.Rows.Count >= dt2.Rows.Count)
            {
                for (var i = 0; i < dt2.Rows.Count; i++)
                {
                    for (var j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            else
            {
                for (var i = 0; i < dt2.Rows.Count - dt1.Rows.Count; i++)
                {
                    var dr3 = dt3.NewRow();
                    dt3.Rows.Add(dr3);
                }
                for (var i = 0; i < dt2.Rows.Count; i++)
                {
                    for (var j = 0; j < dt2.Columns.Count; j++)
                    {
                        dt3.Rows[i][j + dt1.Columns.Count] = dt2.Rows[i][j].ToString();
                    }
                }
            }
            dt3.TableName = dtName;
            return dt3;
        }

        /// <summary>
        ///     Datatable 转 List Dictionary<string object="" />
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<Dictionary<string, object>> DataTableToListDictory(this DataTable dt)
        {
            var ld = new List<Dictionary<string, object>>();
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                var dic = new Dictionary<string, object>();
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    dic.Add(dt.Columns[j].ColumnName, dt.Rows[i][j]);
                }
                ld.Add(dic);
            }
            return ld;
        }
    }
}