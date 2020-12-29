using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

using System.Reflection;
using I.MES.ClientCore.ChannelWCF;


namespace I.Report.DAL
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
                    //db.RPT_User.Remove(item);

                    SqlParameter par = new SqlParameter("UserAccount", item.UserAccount);
                    db.Database.ExecuteSqlCommand(
                        @"DELETE RPT_UserCompany WHERE UserAccount=@UserAccount
                        DELETE dbo.RPT_UserRole WHERE UserAccount=@UserAccount
                        DELETE dbo.RPT_UserRolePriv WHERE Type='U' AND UserRoleCode = @UserAccount
                        DELETE dbo.RPT_User WHERE UserAccount=@UserAccount", par);
                    return db.SaveChanges();
                }
                else
                {
                    return 0;
                }
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

        #region <<根据用户得到菜单权限>>
        /// <summary>
        /// 根据用户得到菜单权限
        /// </summary>
        /// <param name="UserAccount"></param>
        /// <returns></returns>
        public List<RPT_Menu> GetUserPermission(string UserAccount)
        {
            using (ReportEntities db = new ReportEntities())
            {
                string strSql = @"SELECT DISTINCT RPT_Menu.* 
                                    FROM RPT_UserRole
                                    INNER JOIN RPT_UserRolePriv ON RPT_UserRole.RoleCode=RPT_UserRolePriv.UserRoleCode AND RPT_UserRolePriv.[Type] ='R'
                                    INNER JOIN RPT_Menu ON dbo.RPT_Menu.MenuID = RPT_UserRolePriv.MenuID
                                    WHERE RPT_UserRole.UserAccount = @UserAccount
                                    ";
                SqlParameter sp = new SqlParameter("UserAccount", UserAccount);
                var menuList = db.Database.SqlQuery<RPT_Menu>(strSql, sp).ToList();
                return menuList;
            }
        }
        #endregion

        #region <<得到用户的公司权限>>
        /// <summary>
        /// 得到用户的公司权限
        /// </summary>
        /// <param name="UserAccount"></param>
        /// <returns></returns>
        public List<RPT_UserCompany> GetUserCompany(string UserAccount)
        {
            using (ReportEntities db = new ReportEntities())
            {
                var ucList = db.RPT_UserCompany.Where(p => p.UserAccount == UserAccount);
                //var userCompanyList = (from c in db.RPT_Company
                //                       join uc in ucList on
                //                            new { CompanyCode = c.CompanyCode, UserAccount = UserAccount }
                //                            equals
                //                            new { CompanyCode = uc.CompanyCode, UserAccount = uc.UserAccount }
                //                            into temp
                //                       from t in temp.DefaultIfEmpty()
                //                       select new
                //                       {
                //                           UserAccount = t != null ? t.UserAccount : null,
                //                           UserName = t != null ? t.UserName : null,
                //                           CompanyCode = c.CompanyCode,
                //                           CompanyName = c.CompanyName,
                //                           IsSync = t != null ? t.IsSync : false,
                //                           SyncTime = t != null ? t.SyncTime : null,
                //                           IsEnable = t != null ? t.IsEnable : false
                //                       }).ToList();
                //var returnList = userCompanyList.Select(p => new RPT_UserCompany()
                //{
                //    UserAccount = p.UserAccount,
                //    UserName = p.UserName,
                //    CompanyCode = p.CompanyCode,
                //    CompanyName = p.CompanyName,
                //    IsSync = p.IsSync,
                //    SyncTime = p.SyncTime,
                //    IsEnable = p.IsEnable
                //}).Distinct().ToList();
                string sql = @"SELECT RPT_Company.id, RPT_Company.CompanyName,RPT_Company.SystemID AS CompanyCode,RPT_Company.CreateUser,RPT_Company.CreateTime,
                            ISNULL(dbo.RPT_UserCompany.IsEnable,0) AS IsEnable,
                            ISNULL(dbo.RPT_UserCompany.IsSync,0) AS IsSync,SyncTime,UserName,UserAccount
                            FROM dbo.RPT_Company
                            LEFT JOIN dbo.RPT_UserCompany ON dbo.RPT_UserCompany.CompanyCode = dbo.RPT_Company.SystemID
                            AND dbo.RPT_UserCompany.UserAccount = @UserAccount ";
                SqlParameter par = new SqlParameter("UserAccount", UserAccount);
                var returnList = db.Database.SqlQuery<RPT_UserCompany>(sql, par).ToList();
                return returnList;
            }
        }
        #endregion

        #region <<保存用户>>
        public void SaveUser(RPT_User modUser, List<RPT_UserCompany> CompanyList, string currentUser)
        {
            try
            {
                using (ReportEntities db = new ReportEntities())
                {
                    #region <<保存用户>>
                    if (modUser.ID >= 1)
                    {
                        //更新
                        var item = db.RPT_User.FirstOrDefault(p => p.UserAccount == modUser.UserAccount);

                        if (item != null)
                        {
                            item.UserName = modUser.UserName;
                            item.IsActive = modUser.IsActive;
                            item.Password = modUser.Password;

                        }

                    }
                    else
                    {
                        var userList = db.RPT_User.Where(p => p.UserAccount == modUser.UserAccount);
                        if (userList != null && userList.Count() >= 1)
                        {
                            throw new Exception(string.Format("用户账号{0}已存在", modUser.UserAccount));
                        }
                        modUser.CreateUser = currentUser;
                        modUser.CreateTime = DateTime.Now;
                        //新增
                        db.RPT_User.Add(modUser);
                    }
                    #endregion

                    #region <<所有公司>>
                    var companyList = db.RPT_Company.Where(p => true).ToList();
                    #endregion

                    #region <<同步到其它公司>>
                    SqlParameter par = new SqlParameter("UserAccount", modUser.UserAccount);
                    db.Database.ExecuteSqlCommand("DELETE RPT_UserCompany WHERE UserAccount=@UserAccount", par);
                    foreach (RPT_UserCompany userCompany in CompanyList)
                    {
                        int intCompanyId = 0;
                        int.TryParse(userCompany.CompanyCode, out intCompanyId);
                        var url =
                            companyList.Where(p => p.ID == intCompanyId).Select(p => p.ConnectStr).FirstOrDefault();
                        bool bSave = false;
                        try
                        {
                            bool IsActive = false;
                            if (modUser.IsActive && userCompany.IsEnable == true)
                            {
                                IsActive = true;
                            }
                            var wcf = WcfHelper.CreateWcfServiceByUrl<IServiceBase>(url, "basicHttpBinding");
                            bSave = wcf.SaveUser(modUser.UserAccount, modUser.UserName, "", modUser.Password, IsActive, currentUser);
                        }
                        catch (Exception ex)
                        {
                            bSave = false;
                        }
                        userCompany.IsSync = bSave;
                        if (bSave)
                        {
                            userCompany.SyncTime = DateTime.Now;
                        }
                        userCompany.CreateTime = DateTime.Now;
                        userCompany.CreateUser = currentUser;
                        userCompany.UserAccount = modUser.UserAccount;
                        userCompany.UserName = modUser.UserName;
                        db.RPT_UserCompany.Add(userCompany);
                    }
                    #endregion
                    db.SaveChanges();
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion
    }
}
