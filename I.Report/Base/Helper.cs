using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

public static class Helper
{
    public static List<SelectListItem> ToSelectItem(this IEnumerable<I.MES.Models.SelectListItem> items)
    {
        if (items == null)
        {
            return null;
        }
        else
        {
            List<SelectListItem> rtn = new List<SelectListItem>();
            foreach (var item in items)
            {
                rtn.Add(new SelectListItem()
                {
                    Selected = item.Selected,
                    Text = item.Text,
                    Value = item.Value
                });
            }

            return rtn;
        }
    }
}
