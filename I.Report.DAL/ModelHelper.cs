using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;


namespace I.Report.DAL
{

    public class ModelHelper
    {
        /// <summary>
        /// 现在前台值传后台,空值会变成null，现在后台处理一下
        /// </summary>
        /// <param name="obj"></param>
        public static void TrimModel(Object instance)
        {
            PropertyInfo[] propertyInfo = instance.GetType().GetProperties();
            foreach (PropertyInfo pi in propertyInfo)
            {
                //  object value = pi.GetValue(instance);
                if (pi.CanWrite && pi.PropertyType.ToString() == "System.String")
                {
                    if (pi.GetValue(instance) == null)
                    {
                        pi.SetValue(instance, string.Empty);
                    }
                }
            }
        }

        /// <summary>
        /// dataset转对象list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_DataSet"></param>
        /// <param name="p_TableIndex"></param>
        /// <returns></returns>
        public static IList<T> DataSetToIList<T>(DataSet p_DataSet, int p_TableIndex)
        {
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (p_TableIndex > p_DataSet.Tables.Count - 1)
                return null;
            if (p_TableIndex < 0)
                p_TableIndex = 0;

            DataTable p_Data = p_DataSet.Tables[p_TableIndex];
            // 返回值初始化 
            IList<T> result = new List<T>();
            for (int j = 0; j < p_Data.Rows.Count; j++)
            {
                T _t = (T)Activator.CreateInstance(typeof(T));
                PropertyInfo[] propertys = _t.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    for (int i = 0; i < p_Data.Columns.Count; i++)
                    {
                        // 属性与字段名称一致的进行赋值 
                        if (pi.Name.Equals(p_Data.Columns[i].ColumnName))
                        {
                            // 数据库NULL值单独处理 
                            if (p_Data.Rows[j][i] != DBNull.Value)
                            {
                                if (pi.GetMethod.ReturnParameter.ParameterType.Name == "Int32" || pi.GetMethod.ReturnParameter.ParameterType.FullName.Contains("Int32"))
                                {
                                    int value = Convert.ToInt32(p_Data.Rows[j][i]);
                                    pi.SetValue(_t, value, null);
                                }
                                else if (pi.GetMethod.ReturnParameter.ParameterType.FullName.Contains("Boolean"))
                                {
                                    bool value = p_Data.Rows[j][i].ToString() == "0" ? false : true;
                                    pi.SetValue(_t, value, null);
                                }
                                else if (pi.GetMethod.ReturnParameter.ParameterType.FullName.Contains("Decimal"))
                                {
                                    decimal value = Convert.ToDecimal(p_Data.Rows[j][i]);
                                    pi.SetValue(_t, value, null);
                                }
                                else
                                {
                                    pi.SetValue(_t, p_Data.Rows[j][i], null);
                                }
                            }
                            else
                                pi.SetValue(_t, null, null);
                            break;
                        }
                    }
                }
                result.Add(_t);
            }
            return result;
        }

        /// <summary>
        /// dataset转对象list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="p_DataSet"></param>
        /// <param name="p_TableIndex"></param>
        /// <returns></returns>
        public static IList<T> DataSetToIList<T>(DataSet p_DataSet, string p_TableName)
        {
            int _TableIndex = 0;
            if (p_DataSet == null || p_DataSet.Tables.Count < 0)
                return null;
            if (string.IsNullOrEmpty(p_TableName))
                return null;
            for (int i = 0; i < p_DataSet.Tables.Count; i++)
            {
                // 获取Table名称在Tables集合中的索引值 
                if (p_DataSet.Tables[i].TableName.Equals(p_TableName))
                {
                    _TableIndex = i;
                    break;
                }
            }
            return DataSetToIList<T>(p_DataSet, _TableIndex);
        }



        public static IList<T> DataSetToIList<T>(DataSet p_DataSet)
        {
            return DataSetToIList<T>(p_DataSet, 0);
        }



    }
}