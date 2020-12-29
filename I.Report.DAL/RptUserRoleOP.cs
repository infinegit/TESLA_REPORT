using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace I.Report.DAL
{
  public  class RptUserRoleOP
    {
      public IEnumerable<RPT_UserRole> GetListByUser(string UserAccount)
      {
          using (ReportEntities db = new ReportEntities())
          {
              var item = db.RPT_UserRole.Where(p=>p.UserAccount==UserAccount).ToList();
              return item;
          }
      }
      public int Update(string Account, string RoList, string CreateUser = "")
      {
          if (Account.Equals(""))
              return 0;
          using (ReportEntities db = new ReportEntities())
          {
              var items = db.RPT_UserRole.Where(p => p.UserAccount == Account);
              if (items == null)
                  return 0;
              foreach (var v in items)
                  db.RPT_UserRole.Remove(v);
              string UserName = "";
              if (CreateUser != "")
              {
                  UserName = db.RPT_User.Where(p => p.UserAccount == Account).FirstOrDefault().UserName;
              }
              if (RoList.Equals(""))
                  return 0;
              string[] sArray = RoList.Split(';');
              RptRoleOP op = new RptRoleOP();
              List<RPT_Role> RoleLst = op.GetList().ToList();
              Hashtable data = new Hashtable();
              foreach (string i in sArray)
              {
                  foreach (RPT_Role j in RoleLst)
                  {
                      if (i.Equals(j.RoleCode)) {
                          data[i] = j.RoleName;
                      }
                  }
              }
              foreach (string i in sArray)
              {
                  if (!i.Equals(""))
                  {
                      RPT_UserRole UserRole = new RPT_UserRole()
                      {
                          UserAccount = Account,
                          UserName = UserName,
                          RoleName = data[i].ToString(),
                          RoleCode = i,
                          CreateUser = CreateUser,
                          CreateTime = DateTime.Now
                      };
                      db.RPT_UserRole.Add(UserRole);
                  }
              }
              return db.SaveChanges();
          }
      }

    }
}
