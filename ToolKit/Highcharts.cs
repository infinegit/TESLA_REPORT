using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using BL.Svg;

public class Highcharts
{
    private string title = "";
    private string subtitle = "";

    private string xtitle = "";
    private string ytitle = "";

    private List<string> xAxis = new List<string>();
    private Dictionary<string, object[]> series = new Dictionary<string, object[]>();

    private List<string> addtionOption = new List<string>();

    private string chartType = HightChartType.line.ToString();

    private string renderTo = "";

    public Highcharts(string renderTo)
    {
        this.renderTo = renderTo;
    }

    public Highcharts AddChartTitle(string title, string subtitle = "")
    {
        this.title = title;
        this.subtitle = subtitle;
        return this;
    }

    public Highcharts AddCoordinateTitle(string xtitle = "", string ytitle = "")
    {
        this.xtitle = xtitle;
        this.ytitle = ytitle;

        return this;
    }

    public Highcharts AddOption(string option)
    {
        addtionOption.Add(option);
        return this;
    }

    public Highcharts SetChartType(HightChartType charttype)
    {
        this.chartType = charttype.ToString();
        return this;
    }

    public Highcharts SetChartType(string charttype)
    {
        this.chartType = charttype;
        return this;
    }

    public Highcharts AddSeries(string name, object[] series)
    {
        this.series.Add(name, series);
        return this;
    }

    public Highcharts BindData<T>(IEnumerable<T> Entitys, string XColumn, string YColumn, object YDefault, string SerilColumn = "")
    {
        Dictionary<string, List<KeyValuePair<string, object>>> yvalues = new Dictionary<string, List<KeyValuePair<string, object>>>();
        object serilName = "ALL";

        foreach (T dr in Entitys)
        {
            object x = GetEntityValue(dr, XColumn);

            if (x != null)
            {
                string xv = x.ToString();
                if (!string.IsNullOrWhiteSpace(SerilColumn))
                {
                    serilName = GetEntityValue(dr, SerilColumn);
                }
                if (!yvalues.ContainsKey(serilName.ToString()))
                {
                    yvalues.Add(serilName.ToString(), new List<KeyValuePair<string, object>>());
                }
                yvalues[serilName.ToString()].Add(new KeyValuePair<string, object>(xv, GetEntityValue(dr, YColumn)));

                if (!this.xAxis.Contains(xv))
                {
                    this.xAxis.Add(xv);
                }
            }
        }

        //this.xAxis.Sort();

        foreach (var key in yvalues.Keys)
        {
            List<object> yvalue = new List<object>();
            foreach (string xv in this.xAxis)
            {
                var yl = yvalues[key].Where(p => p.Key == xv || p.Key.Equals(xv));
                if (yl.Count() > 0)
                {
                    yvalue.Add(yl.ElementAt(0).Value);
                }
                else
                {
                    yvalue.Add(YDefault);
                }
            }

            this.AddSeries(name: RowValue<string>(key), series: yvalue.ToArray());
        }

        return this;
    }

    public Highcharts BindData<T, Tx, Ty, Ts>(IEnumerable<T> Entitys, Expression<Func<T, Tx>> xColumn, Expression<Func<T, Ty>> yColumn, Expression<Func<T, Ts>> sColumn)
    {
        string xcolumnName = GetEntityName(xColumn);
        string ycolumnName = GetEntityName(yColumn);
        string scolumnName = GetEntityName(sColumn);

        return BindData(Entitys, xcolumnName, ycolumnName, default(Ty), scolumnName);

    }

    public Highcharts BindData<T, Tx, Ty>(IEnumerable<T> Entitys, Expression<Func<T, Tx>> xColumn, Expression<Func<T, Ty>> yColumn)
    {
        string xclumnName = GetEntityName(xColumn);
        string yclumnName = GetEntityName(yColumn);
        string sclumnName = "";
        return BindData(Entitys, xclumnName, yclumnName, default(Ty), sclumnName);
    }

    private string GetEntityName<TModel, TValue>(Expression<Func<TModel, TValue>> expression)
    {
        try
        {
            if (expression.Body.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression body = (MemberExpression)expression.Body;
                if (body.Member is PropertyInfo)
                {
                    return body.Member.Name;
                }
            }
            return "";
        }
        catch
        {
            return "";
        }
    }

    private object GetEntityValue<T>(T dr, string propertyName)
    {
        var ps = typeof(T).GetProperties();
        var pro = ps.FirstOrDefault(p => p.Name == propertyName);
        if (pro != null)
        {
            return pro.GetValue(dr);
        }
        return null;
    }

    public static void Write(string svg, Stream stream, ImageFormat format)
    {
        SvgDocument svgDoc;
        using (MemoryStream streamSvg = new MemoryStream(
          Encoding.UTF8.GetBytes(svg)))
        {
            // Create and return SvgDocument from stream.
            svgDoc = SvgDocument.Open(streamSvg);
            var d = svgDoc.Draw();
            d.Save(stream, format);
        }
    }

    public static byte[] GetBytes(string svg, ImageFormat format)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            Write(svg, ms, format);
            return ms.ToArray();
        }
    }

    private T RowValue<T>(object rowValue)
    {
        if (rowValue == null || rowValue == DBNull.Value)
        {
            if (typeof(T) == typeof(string))
            {
                return (T)((object)"");
            }
            return default(T);
        }
        else
        {
            return (T)rowValue;
        }
    }

    public override string ToString()
    {
        StringBuilder rtn = new StringBuilder();
        rtn.AppendLine("<script type='text/javascript'>");
        rtn.AppendLine("$(function () {");
        rtn.Append("$('#" + renderTo + "').highcharts({");

        foreach (var item in addtionOption)
        {
            rtn.AppendLine(item + ",");
        }

        rtn.AppendLine(@"chart: {type: '" + chartType + "'},");
        rtn.AppendLine(@"title: {text: '" + title + "',x: -20 },");
        rtn.AppendLine(@"subtitle: {text: '" + subtitle + "',x: -20 },");
        rtn.AppendLine("yAxis: { title: { text: '" + ytitle + "'},plotLines: [{ value: 0,  width: 1, color: '#808080'}]},");
        rtn.AppendLine("xAxis: { title:{ text: '" + xtitle + "'}, categories: [");

        bool first = true;
        foreach (var x in xAxis)
        {
            if (!first)
            {
                rtn.Append(",");
            }
            else
            {
                first = !first;
            }
            rtn.AppendLine("'" + x + "'");
        }
        rtn.AppendLine("]},");
        rtn.AppendLine("legend: {layout: 'vertical',align: 'right',verticalAlign: 'middle',borderWidth: 0},");
        rtn.AppendLine("series: [");

        first = true;
        foreach (var item in series)
        {
            if (!first)
            {
                rtn.Append(",");
            }
            else
            {
                first = !first;
            }
            rtn.AppendLine("{name: '" + item.Key + "',");
            rtn.AppendLine("data: [");
            bool subitemfirst = true;
            foreach (var subitem in item.Value)
            {
                if (!subitemfirst)
                {
                    rtn.Append(",");
                }
                else
                {
                    subitemfirst = !subitemfirst;
                }
                rtn.AppendLine(subitem.ToString());
            }
            rtn.AppendLine("]}");
        }
        rtn.AppendLine("]");
        rtn.AppendLine("});});");

        rtn.AppendLine("</script>");

        return rtn.ToString();
    }

    public IHtmlString ToHtml()
    {
        return new HtmlString(this.ToString());
    }
}

public enum HightChartType
{
    /// <summary>
    /// 点线图
    /// </summary>
    line,
    /// <summary>
    /// 平滑曲线图
    /// </summary>
    spline,
    /// <summary>
    /// 区域图
    /// </summary>
    area,
    /// <summary>
    /// 条状图
    /// </summary>
    bar,
    /// <summary>
    /// 柱状图
    /// </summary>
    column,
    /// <summary>
    /// 气泡图
    /// </summary>
    bubble,
    /// <summary>
    /// 饼图
    /// </summary>
    pie,
    /// <summary>
    /// 分布图
    /// </summary>
    scatter,
    /// <summary>
    /// 系列图
    /// </summary>
    series

}


