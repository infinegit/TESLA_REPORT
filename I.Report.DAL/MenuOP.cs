using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DAL;

namespace I.Report.DAL
{
    public class MenuOP
    {

        public IEnumerable<RPT_Menu> GetList()
        {
            using (ReportEntities db = new ReportEntities())
            {
                var data = db.RPT_Menu.ToList();
                return data;
            }
        }

        public int Create(RPT_Menu data)
        {
            using (ReportEntities db = new ReportEntities())
            {
                db.RPT_Menu.Add(data);
                return db.SaveChanges();
            }
        }

        public RPT_Menu GetData(int id)
        {
            using (ReportEntities db = new ReportEntities())
            {
                return db.RPT_Menu.FirstOrDefault(p => p.ID == id);
            }
        }

        public int Update(RPT_Menu data)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var exsit = db.RPT_Menu.FirstOrDefault(p => p.MenuID == data.MenuID);
                if (exsit != null)
                {
                    int id = exsit.ID;
                    exsit.CopyFrom(data);
                    exsit.ID = id;
                }
                return db.SaveChanges();
            }
        }

        public int Delete(int id)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var exsit = db.RPT_Menu.FirstOrDefault(p => p.ID == id);
                if (exsit != null)
                {
                    db.RPT_Menu.Remove(exsit);
                }
                return db.SaveChanges();
            }
        }
    }
}
