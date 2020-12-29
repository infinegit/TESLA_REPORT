using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserOP
    {
        public IEnumerable<RPT_User> GetList(string txtSearch = "")
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_User.Where(p => txtSearch == "" || p.UserAccount.Contains(txtSearch) || p.UserName.Contains(txtSearch)).ToList();
                return item;
            }
        }

        public RPT_User GetData(int id)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_User.FirstOrDefault(p => p.ID == id);
                return item;
            }
        }
        public RPT_User GetUser(string userAccount)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_User.FirstOrDefault(p => p.UserAccount == userAccount);
                return item;
            }
        }
        public int Update(RPT_User data)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_User.FirstOrDefault(p => p.UserAccount == data.UserAccount);
                //item.UserName = data.UserName;
                //return db.SaveChanges();

                if (item != null)
                {
                    item.UserName = data.UserName;
                    item.IsActive = data.IsActive;
                    item.Password = data.Password;

                }

                return db.SaveChanges();
            }
        }

        public int Delete(int id)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_User.FirstOrDefault(p => p.ID == id);
                if (item != null)
                {
                    db.RPT_User.Remove(item);
                }
                return db.SaveChanges();
            }
        }

        public int Create(RPT_User data)
        {
            using (ReportEntities db = new ReportEntities())
            {
                db.RPT_User.Add(data);
                return db.SaveChanges();

             
            }
        }

        public IEnumerable<RPT_User> GetList(string txtSearch, int PageNumber, int PageSize, out int total)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_User.Where(p => txtSearch == "" || p.UserAccount.Contains(txtSearch) || p.UserName.Contains(txtSearch));
                total = item.Count();
                var data = item.OrderBy(p => p.UserAccount).Skip((PageNumber - 1) * PageSize).Take(PageSize);
                return data.ToList();
            }
        }

        public List<RPT_User> GetUserList(string txtSearch, int PageNumber, int PageSize, out int total)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_User.Where(p => txtSearch == "" || p.UserAccount.Contains(txtSearch) || p.UserName.Contains(txtSearch));

                total = item.Count();

                var data = item.OrderBy(p => p.UserAccount).Skip((PageNumber - 1) * PageSize).Take(PageSize);

                return data.ToList();

            }
        }

    }
}
