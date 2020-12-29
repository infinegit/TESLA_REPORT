using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using I.MES.Models;

namespace I.Report.DAL
{
    public class RptRoleOP
    {
        public IEnumerable<RPT_Role> GetList(string txtSearch = "")
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_Role.Where(p => txtSearch == "" || p.RoleName.Contains(txtSearch) || p.RoleDesc.Contains(txtSearch)).ToList();
                return item;
            }
        }
        public IEnumerable<RPT_Role> GetList(List<RPT_UserRole> RoleCode)
        {
            using (ReportEntities db = new ReportEntities())
            {
                int count = RoleCode.Count();
                if (count == 0)
                {
                    var  Role = db.RPT_Role.ToList();
                    return Role;
                }
                else
                {
                    List<RPT_Role> Role = db.RPT_Role.ToList();
                    List<RPT_Role> Roledate = db.RPT_Role.ToList();
                    foreach (RPT_UserRole i in RoleCode)
                    {
                        foreach (RPT_Role j in Role) {
                            if (i.RoleCode.Equals(j.RoleCode)) {
                                Roledate.Remove(j);
                            }
                        }
                    }
                    return Roledate;
                }
            }
        }
        public RPT_Role GetData(int id)
        {

            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_Role.FirstOrDefault(p => p.ID == id);
                return item;
            }
        }
        public RPT_Role GetDataByCode(string RoleCode)
        {

            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_Role.FirstOrDefault(p => p.RoleCode == RoleCode);
                return item;
            }
        }
        public bool Exists(string RoleCode)
        {

            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_Role.FirstOrDefault(p => p.RoleCode == RoleCode);
                return item!=null;
            }
        }

        public int Delete(int id)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_Role.FirstOrDefault(p => p.ID == id);
                db.RPT_UserRolePriv.Where(p => p.UserRoleCode == item.RoleCode).ToList().ForEach(k =>
                {
                    db.RPT_UserRolePriv.Remove(k);
                });
                if (item != null)
                {
                    db.RPT_Role.Remove(item);
                }
                
                return db.SaveChanges();
            }
        }

        public int Create(RPT_Role data,List<RPT_UserRolePriv> list)
        {
            using (ReportEntities db = new ReportEntities())
            {
                db.RPT_Role.Add(data);
                foreach (RPT_UserRolePriv userRole in list)
                {
                    db.RPT_UserRolePriv.Add(userRole);
                }
                return db.SaveChanges();


            }
        }
        public int Update(RPT_Role data, List<RPT_UserRolePriv> list)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_Role.FirstOrDefault(p => p.RoleCode == data.RoleCode);
                if (item != null)
                {
                    item.RoleName = data.RoleName;
                    item.IsActive = data.IsActive;
                    item.RoleDesc = data.RoleDesc;
                }
                var items = db.RPT_UserRolePriv.Where(p => p.UserRoleCode == data.RoleCode);
                foreach (var v in items)
                    db.RPT_UserRolePriv.Remove(v);
                foreach (RPT_UserRolePriv userRole in list)
                {
                    db.RPT_UserRolePriv.Add(userRole);
                }
                return db.SaveChanges();
            }
        }

        public IEnumerable<RPT_Role> GetList(string txtSearch, int PageNumber, int PageSize, out int total)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_Role.Where(p => txtSearch == "" || p.RoleName.Contains(txtSearch) || p.RoleDesc.Contains(txtSearch));
                total = item.Count();
                var data = item.OrderBy(p => p.RoleCode).Skip((PageNumber - 1) * PageSize).Take(PageSize);
                return data.ToList();
            }
        }
        public List<RPT_Menu> GetAllMenu()
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_Menu.OrderBy(p =>p.ID);
                return item.ToList();
            }
        }
        public List<SelectListItem> GetExistsMenu(string RoleCode)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item =  from menu in db.RPT_Menu
                       join role in db.RPT_UserRolePriv on menu.MenuID equals role.MenuID
                       where role.UserRoleCode == RoleCode
                            select new SelectListItem
                            {
                                Value = menu.MenuID,
                                Text = menu.MenuName
                            };
                return item.ToList();
            }
        }
        public List<RPT_Role> GetUserList(string txtSearch, int PageNumber, int PageSize, out int total)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var item = db.RPT_Role.Where(p => txtSearch == "" || p.RoleName.Contains(txtSearch) || p.RoleDesc.Contains(txtSearch));

                total = item.Count();

                var data = item.OrderBy(p => p.RoleCode).Skip((PageNumber - 1) * PageSize).Take(PageSize);

                return data.ToList();

            }
        }
    }
}
