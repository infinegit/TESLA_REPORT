using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using DAL;

namespace I.Report.DAL
{
    public class RptCompanyOP
    {

        public IEnumerable<RPT_Company> GetList(string txtSearch, int pageNumber, int pageSize, out int total)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var data = db.RPT_Company.Where(p => txtSearch == "" || p.CompanyCode.Contains(txtSearch) || p.CompanyName.Contains(txtSearch) || p.CompanyAddress.Contains(txtSearch));
                total = data.Count();
                data = data.OrderBy(p => p.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize);

                return data.ToList();

            }
        }

        public int Create(RPT_Company data)
        {
            using (ReportEntities db = new ReportEntities())
            {
                db.RPT_Company.Add(data);
                return db.SaveChanges();
            }
        }

        public RPT_Company GetData(int id)
        {
            using (ReportEntities db = new ReportEntities())
            {
                return db.RPT_Company.FirstOrDefault(p => p.ID == id);
            }
        }

        public RPT_Company GetData(string systemId)
        {
            using (ReportEntities db = new ReportEntities())
            {
                return db.RPT_Company.FirstOrDefault(p => p.SystemID == systemId);
            }
        }
        public int Update(RPT_Company data)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var exsit = db.RPT_Company.FirstOrDefault(p => p.CompanyCode == data.SystemID);
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
                var exsit = db.RPT_Company.FirstOrDefault(p => p.ID == id);
                if (exsit != null)
                {
                    db.RPT_Company.Remove(exsit);
                }
                return db.SaveChanges();
            }
        }

        public List<RPT_Company> GetCompanyList(string txtSearch, int PageNumber, int PageSize, out int total)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var data = db.RPT_Company.Where(p => txtSearch == "" || p.CompanyCode.Contains(txtSearch) || p.CompanyName.Contains(txtSearch) || p.CompanyAddress.Contains(txtSearch));

                total = data.Count();

                data = data.OrderBy(p => p.ID).Skip((PageNumber - 1) * PageSize).Take(PageSize);

                return data.ToList();
            }
        }
        public List<RPT_Company> GetList()
        {
            using (ReportEntities db = new ReportEntities())
            {
                return db.RPT_Company.ToList();
            }
        }
    }
}

