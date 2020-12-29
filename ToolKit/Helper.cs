using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using System.ComponentModel;

public static class Helper
{
    /// <summary>
    /// 获取实体中某字段的值
    /// </summary>
    /// <param name="obj">实体</param>
    /// <param name="propertyName">字段</param>
    /// <returns>值</returns>
    public static object GetPropertyValue(this object obj, string propertyName)
    {
        var p = obj.GetType().GetProperty(propertyName);
        return p.GetValue(obj, null);
    }

    /// <summary>
    /// 输出各属性的值
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string ToString2(this object obj)
    {
        if (obj == null)
        {
            return "";
        }
        if (obj is string || obj is char[])
        {
            return obj.ToString();
        }
        if (obj.ToString() != obj.GetType().ToString())
        {
            return obj.ToString();
        }

        try
        {
            StringBuilder rtn;
            if (obj is IEnumerable)
            {
                IEnumerable IEnum = (IEnumerable)obj;
                rtn = new StringBuilder();
                rtn.Append(obj.GetType().Name + "(");
                foreach (var v in IEnum)
                {
                    rtn.Append("{" + v.ToString2() + "}");
                }
                rtn.Append(")");
                return rtn.ToString();
            }

            rtn = new StringBuilder(obj.GetType().Name + "(");
            var pros = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var pro in pros)
            {
                rtn.Append("[" + pro.Name + "=" + pro.GetValue(obj, null) + "]");
            }
            rtn.Append(")");
            return rtn.ToString();
        }
        catch
        {
            return obj.ToString();
        }
    }

    /// <summary>
    /// 输入异常的值
    /// </summary>
    /// <param name="ex"></param>
    /// <returns></returns>
    public static string ToString2(this Exception ex)
    {
        string rtn = ex.ToString();
        if (ex.InnerException != null)
        {
            rtn += "\nInnerException" + ex.InnerException.ToString2();
        }

        return rtn;
    }

    /// <summary>
    /// 截取长度为length的字符串（一个汉字占两个字符）
    /// </summary>
    /// <param name="source">初始字符串</param>
    /// <param name="length">截取长度（如果该长度内，最后一个字符为汉字，取整个汉字）</param>
    /// <returns></returns>
    public static string SubString2(this string source, int length = 20)
    {
        if (string.IsNullOrEmpty(source))
        {
            return "";
        }
        Encoding En = Encoding.GetEncoding("gb2312");
        var bs = En.GetBytes(source);
        if (bs.Length - 3 < length)
        {
            return source;
        }
        else
        {
            var s1 = En.GetString(bs, 0, length);
            var s2 = En.GetString(bs, 0, length + 1);
            if (s2.Length > s1.Length)
            {
                return s1 + "...";
            }
            else
            {
                return s2 + "...";
            }
        }
    }

    /// <summary>
    /// 把可为null的时间类型转换为字符串
    /// </summary>
    /// <param name="date">DateTime?</param>
    /// <param name="formate">输入格式（默认为yyyy-MM-dd）</param>
    /// <returns>转换后的字符串</returns>
    public static string ToFormateString(this DateTime? date, string formate = "yyyy-MM-dd")
    {
        if (date == null)
        {
            return "";
        }
        else
        {
            return date.Value.ToString(formate);
        }
    }

    /// <summary>
    /// 判断字符串是否为NULL,如果为NULL返回""，防止报错
    /// </summary>
    /// <param name="source">本源字符串</param>
    /// <returns>转换后的字符串</returns>
    public static string Null(this string source)
    {
        return source ?? "";
    }

    /// <summary>
    /// 根据DataRow获取列值（如果列不存在返回默认值）
    /// 谢彬    2013-10-14    创建
    /// </summary>
    /// <param name="dr">DataRow</param>
    /// <param name="columnName">列名</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>列值</returns>
    public static object Value(this DataRow dr, string columnName, object defaultValue = null)
    {
        try
        {
            var v = dr[columnName];
            if (v == null || v == DBNull.Value)
            {
                return defaultValue;
            }
            return v;
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// 从另一个实体中复制各相同属性的值
    /// (注：必须类型相同，属性名相同）
    /// </summary>
    /// <param name="Entity">本实体</param>
    /// <param name="Source">来源实体</param>
    public static void CopyFrom(this object Entity, object Source)
    {
        var pros = Entity.GetType().GetProperties();
        foreach (var p in pros)
        {
            try
            {
                string field = p.Name;
                if (Source.GetType().GetProperty(field) != null)
                    if (Source.GetType().GetProperty(field).PropertyType == Entity.GetType().GetProperty(field).PropertyType)
                    {
                        object value = Source.GetType().GetProperty(field).GetValue(Source, null);
                        //if (value != null)
                        //{
                        Entity.GetType().GetProperty(field).SetValue(Entity, value, null);
                        //}
                    }
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// 从另一个实体中复制各相同属性的(不为null)值
    /// (注：必须类型相同，属性名相同）
    /// </summary>
    /// <param name="Entity">本实体</param>
    /// <param name="Source">来源实体</param>
    public static void CopyNoNull(this object Entity, object Source)
    {
        var pros = Entity.GetType().GetProperties();
        foreach (var p in pros)
        {
            try
            {
                string field = p.Name;
                if (Source.GetType().GetProperty(field) != null)
                    if (Source.GetType().GetProperty(field).PropertyType == Entity.GetType().GetProperty(field).PropertyType)
                    {
                        object value = Source.GetType().GetProperty(field).GetValue(Source, null);
                        if (value != null)
                        {
                            Entity.GetType().GetProperty(field).SetValue(Entity, value, null);
                        }
                    }
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// 从页面提交请求中获取Model（ext不为空则为后缀）
    /// 谢彬    2013-10-10    创建
    /// </summary>
    /// <typeparam name="T">Model类型</typeparam>
    /// <param name="request">页面提交请求</param>
    /// <param name="ext">对Model字段进行扩展的字符串</param>
    /// <returns></returns>
    public static T GetModel<T>(this HttpRequestBase request, string ext = "") where T : new()
    {
        var pros = typeof(T).GetProperties();
        T rtn = new T();
        foreach (var p in pros)
        {
            try
            {
                string field = p.Name;

                if (!string.IsNullOrEmpty(request[field + ext]))
                {
                    string v = request[field + ext];
                    var pro = rtn.GetType().GetProperty(field);
                    try
                    {
                        //System.Nullable`1[[System.Decimal, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]

                        string protype = pro.PropertyType.FullName.Split(',').First().Split('[').Last();
                        object value;
                        if (pro.PropertyType.Name.Contains("Nullable`1"))
                        {
                            if (v == "")
                            {
                                value = null;
                            }
                            else
                            {
                                value = Convert.ChangeType(v, Type.GetType(protype));
                            }
                        }
                        else
                        {
                            value = Convert.ChangeType(v, pro.PropertyType);
                        }
                        pro.SetValue(rtn, value);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
            }
        }
        return rtn;
    }

    /// <summary>
    /// 从页面提交请求中获取Model（ext不为空则为前缀）
    /// 谭正华    2013-10-24    创建
    /// </summary>
    /// <typeparam name="T">Model类型</typeparam>
    /// <param name="request">页面提交请求</param>
    /// <param name="ext">对Model字段进行扩展的字符串</param>
    /// <returns></returns>
    public static T GetModel_Prefix<T>(this HttpRequestBase request, string ext = "") where T : new()
    {
        var pros = typeof(T).GetProperties();
        T rtn = new T();
        foreach (var p in pros)
        {
            try
            {
                string field = p.Name;

                if (!string.IsNullOrEmpty(request[ext + field]))
                {
                    string v = request[ext + field];
                    var pro = rtn.GetType().GetProperty(field);
                    try
                    {
                        string protype = pro.PropertyType.FullName.Split(',').First().Split('[').Last();
                        object value;
                        if (pro.PropertyType.Name.Contains("Nullable`1"))
                        {
                            if (v == "")
                            {
                                value = null;
                            }
                            else
                            {
                                value = Convert.ChangeType(v, Type.GetType(protype));
                            }
                        }
                        else
                        {
                            value = Convert.ChangeType(v, pro.PropertyType);
                        }
                        pro.SetValue(rtn, value);
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
            }
        }
        return rtn;
    }

    /// <summary>
    /// 从DataTable中获取Model
    /// 谢彬    2013-10-10    创建
    /// </summary>
    /// <typeparam name="T">Model类型</typeparam>
    /// <param name="dataTable">DataTable</param>
    /// <param name="ext">对Model字段进行扩展的字符串</param>
    /// <returns></returns>
    public static List<T> GetModel<T>(this DataTable dataTable, string ext = "") where T : new()
    {
        if (dataTable == null)
        {
            return null;
        }

        List<T> rtn = new List<T>();

        if (dataTable.Rows.Count == 0)
        {
            return rtn;
        }

        var pros = typeof(T).GetProperties();
        foreach (DataRow dr in dataTable.Rows)
        {
            T t = new T();
            foreach (var p in pros)
            {
                try
                {
                    string field = p.Name;
                    object v = dr[field + ext];
                    if (v != null)
                    {
                        try
                        {
                            var value = Convert.ChangeType(v, p.PropertyType);
                            p.SetValue(t, value);
                        }
                        catch
                        {

                        }
                    }
                }
                catch
                {
                }
            }
            rtn.Add(t);
        }

        return rtn;
    }

    /// <summary>
    /// 转换为文件大小的字符串格式
    /// </summary>
    /// <param name="length">文件大小</param>
    /// <returns></returns>
    public static string ToFileSizeString(this int length)
    {
        string[] units = new string[] { "B", "KB", "MB", "GB", "TB" };
        int i = 0;
        float size = length;
        while (size >= 1024)
        {
            size = size / 1024;
            i++;
        }
        return size.ToString("0.##") + units[i];
    }

    /// <summary>
    /// EasyUI的下拉选择树
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="html"></param>
    /// <param name="expression"></param>
    /// <param name="url"></param>
    /// <returns></returns>
    public static MvcHtmlString ComboTreeFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string url)
    {
        var a = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
        StringBuilder str = new StringBuilder();

        str.Append("<input id='" + a.PropertyName + "' name='" + a.PropertyName + "' value='" + a.Model + "' />");
        str.Append("<script type='text/javascript'>");
        str.Append("$(function(){$('#" + a.PropertyName + @"').combotree({
                            url: '" + url + @"',
                            lines : true,
                            onBeforeExpand: function (node, param) {
                                $('#" + a.PropertyName + @"').combotree('tree').tree('options').url = node.attributes.url;
                            }
                        });});");
        str.Append("</script>");

        return new MvcHtmlString(str.ToString());

    }

    /// <summary>
    /// 严智远，将前台提价的json数据转换成对象   2013年12月18日13:08:26
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="jsonString"></param>
    /// <returns></returns>
    public static T JsonDeserialize<T>(string jsonString)
    {
        if (string.IsNullOrEmpty(jsonString))
        {
            return default(T);
        }
        else
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonString);
        }
    }

    /// <summary>
    /// 显示区块字符串
    /// </summary>
    /// <typeparam name="TModel">Model类型</typeparam>
    /// <param name="html"></param>
    /// <param name="expression">区块字符串所在的字段</param>
    /// <returns></returns>
    public static IHtmlString DisplayBlockFor<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, string>> expression)
    {
        var a = ModelMetadata.FromLambdaExpression<TModel, string>(expression, html.ViewData);
        string svalue = (a.Model ?? "").ToString();

        svalue = svalue.Replace("<", "&lt;");
        svalue = svalue.Replace(">", "&gt;");

        svalue = svalue.Replace("\r\n", "<br/>");

        svalue = svalue.Replace("\n", "<br/>");

        svalue = svalue.Replace(" ", "&nbsp;");

        return html.Raw(svalue);

    }

    /// <summary>
    /// 信息比较呈现
    /// 注：Old不是必填的，但未填时，必须在ViewBag或ViewData里有Old属性或索引
    /// </summary>
    /// <typeparam name="TModel">Model类型</typeparam>
    /// <typeparam name="TValue">值类型</typeparam>
    /// <param name="html">当前Html</param>
    /// <param name="expression">表达式</param>
    /// <param name="old">原有值</param>
    /// <returns></returns>
    public static IHtmlString CompareFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object old = null, string diffClass = "") where TModel : class
    {
        if (old == null)
        {
            old = html.ViewBag.Old;
        }
        if (old == null)
        {
            old = html.ViewData["Old"];
        }
        if (old == null)
        {
            return html.DisplayFor(expression);
        }

        var a = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
        object svalue = a.Model;
        object ovalue = old.GetPropertyValue(a.PropertyName);

        if (diffClass != "")
        {
            diffClass = "class='diffStyle'";
        }
        else
        {
            diffClass = "style='color:red;'";
        }

        string rtn = "";
        if (svalue is bool || svalue is bool?)
        {
            rtn = "<input type='checkbox' " + (((bool)svalue) ? "checked='checked'" : "") + " " + (svalue == ovalue ? "" : "style='border:1px solid red;'") + " disabled='disabled' />";
        }
        else
        {

            rtn = "<span " + (svalue == ovalue || svalue.Equals(ovalue) ? "" : " title='" + ovalue + "' " + diffClass) + ">" + svalue + "</span>";
        }

        return new HtmlString(rtn);
    }


    public static IHtmlString Compare<TModel>(this HtmlHelper<TModel> html, string PropertyName, object NewValue, object old = null, string diffClass = "")
    {
        if (old == null)
        {
            old = html.ViewBag.Old;
        }
        if (old == null)
        {
            old = html.ViewData["Old"];
        }
        if (old == null)
        {
            return new HtmlString(NewValue.ToString());
        }
        object svalue = NewValue;
        object ovalue = old.GetPropertyValue(PropertyName);

        if (diffClass != "")
        {
            diffClass = "class='diffStyle'";
        }
        else
        {
            diffClass = "style='color:red;'";
        }

        string rtn = "";
        if (svalue is bool || svalue is bool?)
        {
            rtn = "<input type='checkbox' " + (((bool)svalue) ? "checked='checked'" : "") + " " + (svalue == ovalue ? "" : "style='border:1px solid red;'") + " disabled='disabled' />";
        }
        else
        {

            rtn = "<span " + (svalue == ovalue || svalue.Equals(ovalue) ? "" : " title='" + ovalue + "' " + diffClass) + ">" + svalue + "</span>";
        }

        return new HtmlString(rtn);
    }

    /// <summary>
    /// 显示区块字符串
    /// </summary>
    /// <param name="html"></param>
    /// <param name="blockString">区块字符串</param>
    /// <returns></returns>
    public static IHtmlString DisplayBlock(this HtmlHelper html, string blockString)
    {
        if (string.IsNullOrEmpty(blockString))
        {
            return html.Raw("");
        }
        blockString = blockString.Replace("<", "&lt;");
        blockString = blockString.Replace(">", "&gt;");

        blockString = blockString.Replace("\r\n", "<br/>");
        blockString = blockString.Replace("\n", "<br/>");

        blockString = blockString.Replace(" ", "&nbsp;");

        return html.Raw(blockString);
    }

    public static Highcharts HighCharts<T, Tx, Ty, Ts>(this HtmlHelper<IEnumerable<T>> html, string renderTo,
        Expression<Func<T, Tx>> xColumn,
        Expression<Func<T, Ty>> yColumn,
        Expression<Func<T, Ts>> sColumn
        )
    {
        Highcharts hc = new Highcharts(renderTo);
        return hc.BindData(html.ViewData.Model, xColumn, yColumn, sColumn);
    }

    public static Highcharts HighCharts<T, Tx, Ty, Ts>(this HtmlHelper html, string renderTo, IEnumerable<T> data,
        Expression<Func<T, Tx>> xColumn, Expression<Func<T, Ty>> yColumn, Expression<Func<T, Ts>> sColumn)
    {
        Highcharts hc = new Highcharts(renderTo);
        return hc.BindData(data, xColumn, yColumn, sColumn);
    }

    #region 以下是对EasyUI 做简单的封装，如需添加请以"Easy"开头

    public static MvcHtmlString EasyComboBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string url, object htmlAttributes = null, string relatedID = "")
    {
        var strHtml = html.TextBoxFor(expression, htmlAttributes);
        string strRelated = "";
        var a = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
        StringBuilder str = new StringBuilder(strHtml.ToHtmlString());
        str.Append("<input type='hidden' id='Easy" + a.PropertyName + "' name='Easy" + a.PropertyName + "' value='" + a.Model + "' />");
        str.Append("<script type='text/javascript'>");
        str.Append(@"$(function(){");
        if (!string.IsNullOrWhiteSpace(relatedID))
        {
            str.Append(@"var " + a.PropertyName + @"relatedV=$('#" + relatedID + @"').val();
                               $('#" + relatedID + @"').change(function () {
                                                    var v = $(this).val();
                                                    $('#" + a.PropertyName + @"').combobox('reload', '" + url + "?" + relatedID + @"=' + v);
                                                });");
            strRelated = "+'?" + relatedID + @"='+" + a.PropertyName + @"relatedV";
        }
        str.Append(@"$('#" + a.PropertyName + @"').combobox({
                            url: '" + url + @"'" + strRelated + @",
                            valueField:'Value',
                            textField:'Text',
                            filter: function (q, row) {
                                var opts = $(this).combobox('options');
                                return row[opts.textField].toLocaleLowerCase().indexOf(q.toLocaleLowerCase()) >= 0
                                        || row[opts.valueField].toLocaleLowerCase().indexOf(q.toLocaleLowerCase()) >= 0;
                            }
                        });});");

        str.Append("</script>");
        return new MvcHtmlString(str.ToString());
    }

    public static MvcHtmlString EasyComboBox(this HtmlHelper html, string name, string value, string url, object htmlAttributes = null, string relatedID = "")
    {
        var strHtml = html.TextBox(name, value, htmlAttributes);
        string strRelated = "";
        StringBuilder str = new StringBuilder(strHtml.ToHtmlString());
        str.Append("<input type='hidden' id='Easy" + name + "' name='Easy" + name + "' value='" + value + "' />");
        str.Append("<script type='text/javascript'>");
        str.Append(@"$(function(){");
        if (!string.IsNullOrWhiteSpace(relatedID))
        {
            //            str.Append(@"var " + name + @"relatedV=$('#" + relatedID + @"').val();
            //                               $('#" + relatedID + @"').change(function () {
            //                                                    var v = $(this).val();
            //                                                    $('#" + name + @"').combobox('reload', '" + url + "?" + relatedID + @"=' + v);
            //                                                });");

            str.Append("var " + name + "relatedV=$('[name =\"" + relatedID + "\"]').val();" +
                       "$('[name =\"" + relatedID + "\"]'" + @").change(function () {
                                                    var v = $(this).val();
                                                    $('#" + name + @"').combobox('reload', '" + url + "?" + relatedID + @"=' + v);
                                                });");
            strRelated = "+'?" + relatedID + @"='+" + name + @"relatedV";
        }
        str.Append(@"$('#" + name + @"').combobox({
                            url: '" + url + @"'" + strRelated + @",
                            valueField:'Value',
                            textField:'Text',
                            filter: function (q, row) {
                                var opts = $(this).combobox('options');
                                return row[opts.textField].toLocaleLowerCase().indexOf(q.toLocaleLowerCase()) >= 0
                                        || row[opts.valueField].toLocaleLowerCase().indexOf(q.toLocaleLowerCase()) >= 0;
                            }
                               });});");

        str.Append("</script>");
        return new MvcHtmlString(str.ToString());
    }

    /// <summary>
    /// 日期选择框   严智远  2013年7月23日17:53:06
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="html"></param>
    /// <param name="expression"></param>
    /// <param name="disabled">默认为false,不可编辑，可设置为true</param>moren
    /// <returns></returns>
    public static MvcHtmlString EasyDateBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, bool disabled = false)
    {
        var a = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
        StringBuilder tmpHtml = new StringBuilder();
        tmpHtml.AppendFormat("<input id=\"{0}\" name=\"{1}\" value=\"{2}\" editable=\"{3}\"", a.PropertyName, a.PropertyName, (a.Model as DateTime?).ToFormateString(), disabled);
        tmpHtml.Append(" class=\"easyui-datebox\" type=\"text\" ></input>");
        tmpHtml.Append("<input type='hidden' id='Easy" + a.PropertyName + "' name='Easy" + a.PropertyName + "' value='" + (a.Model as DateTime?).ToFormateString() + "' />");

        return new MvcHtmlString(tmpHtml.ToString());
    }

    /// <summary>
    /// 严智远 
    /// 对日期选择框
    /// </summary>
    /// <param name="html"></param>
    /// <param name="name"></param>
    /// <param name="dt"></param>
    /// <param name="disabled"></param>
    /// <returns></returns>
    public static MvcHtmlString EasyDateBox(this HtmlHelper html, string name, DateTime? dt, string width = "145px;", bool disabled = false)
    {
        StringBuilder tmpHtml = new StringBuilder();
        tmpHtml.AppendFormat("<input id=\"{0}\" name=\"{1}\" value=\"{2}\" style=\"width:{3}\" editable=\"{4}\"", name, name, dt.ToFormateString(), width, disabled);
        tmpHtml.Append(" class=\"easyui-datebox\" type=\"text\" ></input>");
        tmpHtml.Append("<input type='hidden' id='Easy" + name + "' name='Easy" + name + "' value='" + dt.ToFormateString() + "' />");

        return new MvcHtmlString(tmpHtml.ToString());
    }

    /// <summary>
    /// 日期时间选择框   严智远  2013年7月23日17:53:06
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="html"></param>
    /// <param name="expression"></param>
    /// <param name="disabled">默认为false,不可编辑，可设置为true</param>moren
    /// <returns></returns>
    public static MvcHtmlString EasyDateTimeBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string width = "150px", bool disabled = false)
    {
        var a = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
        StringBuilder tmpHtml = new StringBuilder();
        tmpHtml.AppendFormat("<input id=\"{0}\" name=\"{1}\" value=\"{2}\" style='width:{4}' editable=\"{3}\"", a.PropertyName, a.PropertyName, (a.Model as DateTime?).ToFormateString("yyyy-MM-dd HH:mm:ss"), disabled, width);
        tmpHtml.Append(" class=\"easyui-datetimebox\" type=\"text\" ></input>");
        tmpHtml.Append("<input type='hidden' id='Easy" + a.PropertyName + "' name='Easy" + a.PropertyName + "' value='" + (a.Model as DateTime?).ToFormateString("yyyy-MM-dd HH:mm:ss") + "' />");

        return new MvcHtmlString(tmpHtml.ToString());
    }

    /// <summary>
    /// 严智远 
    /// 对方法EasyDateTimeBoxFor重载
    /// </summary>
    /// <param name="html"></param>
    /// <param name="name"></param>
    /// <param name="dt"></param>
    /// <param name="width"></param>
    /// <param name="disabled"></param>
    /// <returns></returns>
    public static MvcHtmlString EasyDateTimeBox(this HtmlHelper html, string name, DateTime? dt, string width, bool disabled = false)
    {
        StringBuilder tmpHtml = new StringBuilder();
        tmpHtml.AppendFormat("<input id=\"{0}\" name=\"{1}\" value=\"{2}\" style='width:{4}' editable=\"{3}\"", name, name, (dt as DateTime?).ToFormateString("yyyy-MM-dd HH:mm:ss"), disabled, width);
        tmpHtml.Append(" class=\"easyui-datetimebox\" type=\"text\" ></input>");
        tmpHtml.Append("<input type='hidden' id='Easy" + name + "' name='Easy" + name + "' value='" + (dt as DateTime?).ToFormateString("yyyy-MM-dd HH:mm:ss") + "' />");

        return new MvcHtmlString(tmpHtml.ToString());
    }

    /// <summary>
    /// 数字文本框NumberBox 严智远  2013年7月30日12:26:04
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="html"></param>
    /// <param name="expression"></param>
    /// <param name="format"></param>
    /// <param name="disabled"></param>
    /// <returns></returns>
    public static MvcHtmlString EasyNumberBoxFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string format = "min:0", bool disabled = false, string width = "120px")
    {
        var a = ModelMetadata.FromLambdaExpression<TModel, TValue>(expression, html.ViewData);
        StringBuilder tmpHtml = new StringBuilder();
        tmpHtml.AppendFormat("<input id=\"{0}\" name=\"{1}\" value=\"{2}\" editable=\"{3}\" style=\"width:{4};\" ", a.PropertyName, a.PropertyName, a.Model, disabled, width);
        tmpHtml.AppendFormat(" class=\"easyui-numberbox\" type=\"text\" data-options=\"{0}\" ></input>", format);
        tmpHtml.Append("<input type='hidden' id='Easy" + a.PropertyName + "' name='Easy" + a.PropertyName + "' value='" + a.Model + "' />");
        return new MvcHtmlString(tmpHtml.ToString());
    }

    /// <summary>
    /// 数字文本框NumberBox 严智远  2013年12月26日9:14:46
    /// </summary>
    /// <param name="html"></param>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="format"></param>
    /// <param name="disabled"></param>
    /// <param name="width"></param>
    /// <param name="IsHid">考评数据录入中无需隐藏字段(字符序列化对象时，该字段没有)</param>
    /// <returns></returns>
    public static MvcHtmlString EasyNumberBox(this HtmlHelper html, string name, string value, string format = "min:0", bool disabled = false, string width = "120px", bool IsHid = true)
    {
        StringBuilder tmpHtml = new StringBuilder();
        tmpHtml.AppendFormat("<input id=\"{0}\" name=\"{1}\" value=\"{2}\" editable=\"{3}\" style=\"width:{4};\" ", name, name, value, disabled, width);
        tmpHtml.AppendFormat(" class=\"easyui-numberbox\" type=\"text\" data-options=\"{0}\" ></input>", format);
        if (IsHid)
        {
            tmpHtml.Append("<input type='hidden' id='Easy" + name + "' name='Easy" + name + "' value='" + value + "' />");
        }
        return new MvcHtmlString(tmpHtml.ToString());
    }

    #endregion

    #region Link扩展方法

    /// <summary>
    /// 返回包含指定操作的虚拟路径的定位点元素（a 元素）
    /// </summary>
    /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例</param>
    /// <param name="linkText"> 定位点元素的内部文本</param>
    /// <param name="actionName">操作的名称</param>
    /// <returns>一个定位点元素（a 元素）</returns>
    /// <exception cref="System.ArgumentException">linkText 参数为 null 或为空</exception>
    public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName)
    {
        return htmlHelper.ActionLink(linkText, actionName, new { HistoryUrl = htmlHelper.ViewBag.CurrentUrl });
    }

    /// <summary>
    /// 返回包含指定操作的虚拟路径的定位点元素（a 元素）
    /// </summary>
    /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例</param>
    /// <param name="linkText">定位点元素的内部文本</param>
    /// <param name="actionName">操作的名称</param>
    /// <param name="routeValues">一个包含路由参数的对象。通过检查对象的属性，利用反射检索参数。该对象通常是使用对象初始值设定项语法创建的</param>
    /// <returns>一个定位点元素（a 元素）</returns>
    /// <exception cref="System.ArgumentException">linkText 参数为 null 或为空</exception>
    public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
    {
        RouteValueDictionary routeValuesNew = new RouteValueDictionary(routeValues);

        routeValuesNew.Add("HistoryUrl", htmlHelper.ViewBag.CurrentUrl);

        return htmlHelper.ActionLink(linkText, actionName, routeValuesNew);
    }

    /// <summary>
    /// 返回包含指定操作的虚拟路径的定位点元素（a 元素）
    /// </summary>
    /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例</param>
    /// <param name="linkText">定位点元素的内部文本</param>
    /// <param name="actionName">操作的名称</param>
    /// <param name="routeValues">一个包含路由参数的对象</param>
    /// <returns>一个定位点元素（a 元素）</returns>
    /// <exception cref="System.ArgumentException">linkText 参数为 null 或为空</exception>
    public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
    {
        return htmlHelper.ActionLink(linkText, actionName, routeValues);
    }

    /// <summary>
    /// 返回包含指定操作的虚拟路径的定位点元素（a 元素）
    /// </summary>
    /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例</param>
    /// <param name="linkText">定位点元素的内部文本</param>
    /// <param name="actionName">操作的名称</param>
    /// <param name="controllerName">控制器的名称</param>
    /// <returns>一个定位点元素（a 元素）</returns>
    /// <exception cref="System.ArgumentException">linkText 参数为 null 或为空</exception>
    public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
    {
        return htmlHelper.ActionLink(linkText, actionName, controllerName, new { HistoryUrl = htmlHelper.ViewBag.CurrentUrl }, null);
    }

    /// <summary>
    /// 返回包含指定操作的虚拟路径的定位点元素（a 元素）
    /// </summary>
    /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例</param>
    /// <param name="linkText">定位点元素的内部文本</param>
    /// <param name="actionName">操作的名称</param>
    /// <param name="routeValues">一个包含路由参数的对象</param>
    /// <param name="htmlAttributes">包含元素 HTML 特性的对象。通过检查对象的属性，利用反射检索特性。该对象通常是使用对象初始值设定项语法创建的</param>
    /// <returns>一个定位点元素（a 元素）</returns>
    /// <exception cref="System.ArgumentException">linkText 参数为 null 或为空</exception>
    public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
    {
        RouteValueDictionary routeValuesNew = new RouteValueDictionary(routeValues);
        routeValuesNew.Add("HistoryUrl", htmlHelper.ViewBag.CurrentUrl);

        Dictionary<string, object> htmlAttributesNew = new Dictionary<string, object>();

        if (htmlAttributes != null)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(htmlAttributes))
            {
                object obj2 = descriptor.GetValue(htmlAttributes);
                htmlAttributesNew.Add(descriptor.Name, obj2);
            }
        }
        return htmlHelper.ActionLink(linkText, actionName, routeValuesNew, htmlAttributesNew);
    }

    /// <summary>
    /// 返回包含指定操作的虚拟路径的定位点元素（a 元素）
    /// </summary>
    /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例</param>
    /// <param name="linkText">定位点元素的内部文本</param>
    /// <param name="actionName">操作的名称</param>
    /// <param name="routeValues">一个包含路由参数的对象</param>
    /// <param name="htmlAttributes">包含元素 HTML 特性的对象。通过检查对象的属性，利用反射检索特性。该对象通常是使用对象初始值设定项语法创建的</param>
    /// <returns>一个定位点元素（a 元素）</returns>
    /// <exception cref="System.ArgumentException">linkText 参数为 null 或为空</exception>
    public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
    {
        routeValues.Add("HistoryUrl", htmlHelper.ViewBag.CurrentUrl);
        return htmlHelper.ActionLink(linkText, actionName, routeValues, htmlAttributes);
    }

    /// <summary>
    /// 返回包含指定操作的虚拟路径的定位点元素（a 元素）
    /// </summary>
    /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例</param>
    /// <param name="linkText">定位点元素的内部文本</param>
    /// <param name="actionName">操作的名称</param>
    /// <param name="controllerName">控制器的名称</param>
    /// <param name="routeValues">一个包含路由参数的对象</param>
    /// <param name="htmlAttributes">包含元素 HTML 特性的对象。通过检查对象的属性，利用反射检索特性。该对象通常是使用对象初始值设定项语法创建的</param>
    /// <returns>一个定位点元素（a 元素）</returns>
    /// <exception cref="System.ArgumentException">linkText 参数为 null 或为空</exception>
    public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
    {
        RouteValueDictionary routeValuesNew = new RouteValueDictionary(routeValues);
        routeValuesNew.Add("HistoryUrl", htmlHelper.ViewBag.CurrentUrl);

        Dictionary<string, object> htmlAttributesNew = new Dictionary<string, object>();

        if (htmlAttributes != null)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(htmlAttributes))
            {
                object obj2 = descriptor.GetValue(htmlAttributes);
                htmlAttributesNew.Add(descriptor.Name, obj2);
            }
        }

        return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValuesNew, htmlAttributesNew);

    }

    /// <summary>
    /// 返回包含指定操作的虚拟路径的定位点元素（a 元素）
    /// </summary>
    /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例</param>
    /// <param name="linkText">定位点元素的内部文本</param>
    /// <param name="actionName">操作的名称</param>
    /// <param name="controllerName">控制器的名称</param>
    /// <param name="routeValues">一个包含路由参数的对象</param>
    /// <param name="htmlAttributes">包含元素 HTML 特性的对象。通过检查对象的属性，利用反射检索特性。该对象通常是使用对象初始值设定项语法创建的</param>
    /// <returns>一个定位点元素（a 元素）</returns>
    /// <exception cref="System.ArgumentException">linkText 参数为 null 或为空</exception>
    public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
    {
        routeValues.Add("HistoryUrl", htmlHelper.ViewBag.CurrentUrl);
        return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
    }

    /// <summary>
    /// 返回包含指定操作的虚拟路径的定位点元素（a 元素）
    /// </summary>
    /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例</param>
    /// <param name="linkText">定位点元素的内部文本</param>
    /// <param name="actionName">操作的名称</param>
    /// <param name="controllerName">控制器的名称</param>
    /// <param name="fragment">URL 片段名称（定位点名称）</param>
    /// <param name="hostName">URL 的主机名</param>
    /// <param name="protocol">URL 协议，如“http”或“https”</param>
    /// <param name="routeValues">一个包含路由参数的对象</param>
    /// <param name="htmlAttributes">包含元素 HTML 特性的对象。通过检查对象的属性，利用反射检索特性。该对象通常是使用对象初始值设定项语法创建的</param>
    /// <returns>一个定位点元素（a 元素）</returns>
    /// <exception cref="System.ArgumentException">linkText 参数为 null 或为空</exception>
    public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
    {
        RouteValueDictionary routeValuesNew = new RouteValueDictionary(routeValues);
        routeValuesNew.Add("HistoryUrl", htmlHelper.ViewBag.CurrentUrl);

        Dictionary<string, object> htmlAttributesNew = new Dictionary<string, object>();

        if (htmlAttributes != null)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(htmlAttributes))
            {
                object obj2 = descriptor.GetValue(htmlAttributes);
                htmlAttributesNew.Add(descriptor.Name, obj2);
            }
        }
        return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValuesNew, htmlAttributesNew);
    }

    /// <summary>
    /// 返回包含指定操作的虚拟路径的定位点元素（a 元素）
    /// </summary>
    /// <param name="htmlHelper">此方法扩展的 HTML 帮助器实例</param>
    /// <param name="linkText">定位点元素的内部文本</param>
    /// <param name="actionName">操作的名称</param>
    /// <param name="controllerName">控制器的名称</param>
    /// <param name="fragment">URL 片段名称（定位点名称）</param>
    /// <param name="hostName">URL 的主机名</param>
    /// <param name="protocol">URL 协议，如“http”或“https”</param>
    /// <param name="routeValues">一个包含路由参数的对象</param>
    /// <param name="htmlAttributes">包含元素 HTML 特性的对象。通过检查对象的属性，利用反射检索特性。该对象通常是使用对象初始值设定项语法创建的</param>
    /// <returns>一个定位点元素（a 元素）</returns>
    /// <exception cref="System.ArgumentException">linkText 参数为 null 或为空</exception>
    public static MvcHtmlString Link(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
    {
        routeValues.Add("HistoryUrl", htmlHelper.ViewBag.CurrentUrl);
        return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
    }

    public static string GetHtml(this Controller controller, string TemplateViewName)
    {
        string html = string.Empty;
        IView v = ViewEngines.Engines.FindView(controller.ControllerContext, TemplateViewName, "").View;
        using (StringWriter sw = new StringWriter())
        {
            ViewContext vc = new ViewContext(controller.ControllerContext, v, controller.ViewData, controller.TempData, sw);
            vc.View.Render(vc, sw);
            html = sw.ToString();
        }
        return html;
    }

    #endregion

    /// <summary>
    /// 根据控件名渲染控件
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="name"></param>
    /// <param name="longText">是否是长文本</param>
    /// <param name="rows"></param>
    /// <param name="cols"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static IHtmlString EditorFromName(this HtmlHelper htmlHelper, string name, int width = 0, bool longText = false, int rows = 3, int cols = 40)
    {
        StringBuilder sb = new StringBuilder();
        var model = htmlHelper.ViewData.Model;
        var modelType = model.GetType();
        var property = modelType.GetProperty(name);
        var value = property.GetValue(model);
        var propertyTypeName = property.PropertyType.Name;
        if (propertyTypeName == typeof(DateTime).Name || propertyTypeName == typeof(DateTime?).Name)
        {
            sb.Append("<input class='easyui-datetimebox' data-options='formatter:myformatter,parser:myparser'");
            sb.Append(" id= '" + name + "' name='" + name + "' value='" + (value == null ? "" : ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")) + "'");
            sb.Append(width == 0 ? "" : " style='width:" + width + "px;'");
            sb.Append(" />");
        }
        else if (propertyTypeName == typeof(bool).Name || propertyTypeName == typeof(bool?).Name)
        {
            bool tempValue = false;
            if (value != null)
            {
                tempValue = (bool)value;
            }
            sb.Append("<input type='checkbox'");
            sb.Append(" id= '" + name + "' name='" + name + "' value='" + (tempValue ? "" : "checked") + "'");
            sb.Append(" />");
        }
        else if (propertyTypeName == typeof(int).Name || propertyTypeName == typeof(int?).Name ||
                 propertyTypeName == typeof(short).Name || propertyTypeName == typeof(short?).Name ||
                 propertyTypeName == typeof(long).Name || propertyTypeName == typeof(long?).Name ||
                 propertyTypeName == typeof(float).Name || propertyTypeName == typeof(float?).Name ||
                 propertyTypeName == typeof(double).Name || propertyTypeName == typeof(double?).Name ||
                 propertyTypeName == typeof(decimal).Name || propertyTypeName == typeof(decimal?).Name)
        {
            sb.Append("<input type='number'");
            sb.Append(" id= '" + name + "' name='" + name + "' value='" + (value ?? "") + "'");
            sb.Append(width == 0 ? "" : " style='width:" + width + "px;'");
            sb.Append(" />");
        }
        else if (longText)
        {
            sb.Append("<textarea");
            sb.Append(" id= '" + name + "' name='" + name + "'");
            sb.Append(" rows='" + rows + "' cols='" + cols + "'");
            sb.Append(">" + (value ?? "") + "</textarea>");
        }
        else
        {
            sb.Append("<input type='text'");
            sb.Append(" id= '" + name + "' name='" + name + "' value='" + (value ?? "") + "'");
            sb.Append(width == 0 ? "" : " style='width:" + width + "px;'");
            sb.Append(" />");
        }
        return new MvcHtmlString(sb.ToString());
    }

    /// <summary>
    /// 根据空间名生成下拉选项
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="name"></param>
    /// <param name="dropSource"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static MvcHtmlString DropDownFromName(this HtmlHelper htmlHelper, string name, List<SelectListItem> dropSource, int width = 150)
    {
        var model = htmlHelper.ViewData.Model;
        var modelType = model.GetType();
        var property = modelType.GetProperty(name);
        var value = property.GetValue(model);
        StringBuilder sb = new StringBuilder();
        sb.Append("<select style='width:" + width + "px;'");
        sb.Append(" id='" + name + "' name='" + name + "'>");
        if (dropSource != null)
        {
            foreach (var item in dropSource)
            {
                sb.AppendLine(item.Value == (string)value
                    ? " <option value='" + item.Value + "' selected='true'>" + item.Text + "</option>"
                    : " <option value='" + item.Value + "'>" + item.Text + "</option>");
            }
        }
        sb.Append("</select>");

        return new MvcHtmlString(sb.ToString());
    }

    /// <summary>
    /// 只显示信息，不能更改
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="name">空间名</param>
    /// <param name="width">宽度</param>
    /// <param name="longText">是否是长文本</param>
    /// <param name="rows">长文本行数</param>
    /// <param name="cols">长文本宽数</param>
    /// <returns></returns>
    public static IHtmlString DisplayFromName(this HtmlHelper htmlHelper, string name, int width = 0, bool longText = false, int rows = 3, int cols = 40)
    {
        StringBuilder sb = new StringBuilder();
        var model = htmlHelper.ViewData.Model;
        var modelType = model.GetType();
        var property = modelType.GetProperty(name);
        var value = property.GetValue(model);
        var propertyTypeName = property.PropertyType.Name;
        if (propertyTypeName == typeof(DateTime).Name || propertyTypeName == typeof(DateTime?).Name)
        {
            sb.Append("<input class='easyui-datetimebox' data-options='formatter:myformatter,parser:myparser'");
            sb.Append(" disabled='disabled'");
            sb.Append(" id= 'Dis" + name + "' name='Dis" + name + "' value='" + (value == null ? "" : ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")) + "'");
            sb.Append(width == 0 ? "" : " style='width:" + width + "px;'");
            sb.Append(" />");
        }
        else if (propertyTypeName == typeof(bool).Name || propertyTypeName == typeof(bool?).Name)
        {
            bool tempValue = false;
            if (value != null)
            {
                tempValue = (bool)value;
            }
            sb.Append("<input type='checkbox'");
            sb.Append(" disabled='disabled'");
            sb.Append(" id= 'Dis" + name + "' name='Dis" + name + "' value='" + (tempValue ? "" : "checked") + "'");
            sb.Append(" />");
        }
        else if (propertyTypeName == typeof(int).Name || propertyTypeName == typeof(int?).Name ||
                 propertyTypeName == typeof(short).Name || propertyTypeName == typeof(short?).Name ||
                 propertyTypeName == typeof(long).Name || propertyTypeName == typeof(long?).Name ||
                 propertyTypeName == typeof(float).Name || propertyTypeName == typeof(float?).Name ||
                 propertyTypeName == typeof(double).Name || propertyTypeName == typeof(double?).Name ||
                 propertyTypeName == typeof(decimal).Name || propertyTypeName == typeof(decimal?).Name)
        {
            sb.Append("<input type='number'");
            sb.Append(" disabled='disabled'");
            sb.Append(" id= 'Dis" + name + "' name='Dis" + name + "' value='" + (value ?? "") + "'");
            sb.Append(width == 0 ? "" : " style='width:" + width + "px;'");
            sb.Append(" />");
        }
        else if (longText)
        {
            sb.Append("<textarea");
            sb.Append(" id= '" + name + "' name='" + name + "'");
            sb.Append(" rows='" + rows + "' cols='" + cols + "'");
            sb.Append(">" + (value ?? "") + "</textarea>");
        }
        else
        {
            sb.Append("<input type='text'");
            sb.Append(" disabled='disabled'");
            sb.Append(" id= 'Dis" + name + "' name='Dis" + name + "' value='" + (value ?? "") + "'");
            sb.Append(width == 0 ? "" : " style='width:" + width + "px;'");
            sb.Append(" />");
        }
        return new MvcHtmlString(sb.ToString());
    }

    /// <summary>
    /// 根据空间名生成下拉选项
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="name"></param>
    /// <param name="dropSource"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public static MvcHtmlString DisplayDropDownFromName(this HtmlHelper htmlHelper, string name, List<SelectListItem> dropSource, int width = 150)
    {
        var model = htmlHelper.ViewData.Model;
        var modelType = model.GetType();
        var property = modelType.GetProperty(name);
        var value = property.GetValue(model);
        StringBuilder sb = new StringBuilder();
        sb.Append("<select disabled='disabled' style='width:" + width + "px;'");
        sb.Append(" id=Dis'" + name + "' name='Dis" + name + "'>");
        if (dropSource != null)
        {
            foreach (var item in dropSource)
            {
                sb.AppendLine(item.Value == (string)value
                    ? "<option value='" + item.Value + "' selected='true'>" + item.Text + "</option>"
                    : "<option value='" + item.Value + "'>" + item.Text + "</option>");
            }
        }
        sb.Append("</select>");

        return new MvcHtmlString(sb.ToString());
    }

    /// <summary>
    /// 根据名称隐藏
    /// </summary>
    /// <param name="htmlHelper"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static IHtmlString HiddenFromName(this HtmlHelper htmlHelper, string name)
    {
        StringBuilder sb = new StringBuilder();
        var model = htmlHelper.ViewData.Model;
        var modelType = model.GetType();
        var property = modelType.GetProperty(name);
        var value = property.GetValue(model);
        sb.Append("<input type='hidden'");
        sb.Append(" id= '" + name + "' name='" + name + "' value='" + (value ?? "") + "'");
        sb.Append(" />");
        return new MvcHtmlString(sb.ToString());
    }

}
