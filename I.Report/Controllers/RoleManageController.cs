using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using I.MES.Library;
using I.MES.Models;
using I.MES.Tools;
using I.Report.Config;

namespace I.Report.Controllers
{
    public class RoleManageController : BaseController
    {
        RoleManageOP op = new RoleManageOP();

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            return View();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult Modify(string ID)
        {
            var data = op.GetCfgByID(Convert.ToInt32(ID));
            return View(data);
        }

        /// <summary>
        /// 获取所有的菜单列表
        /// </summary>
        /// <param name="RoleCode"></param>
        /// <returns></returns>
        public ActionResult GetAllMenu()
        {
            var all = op.GetAllMenu();
            return Json(all);
        }

        /// <summary>
        /// 根据角色编号获取左边菜单列表
        /// </summary>
        /// <param name="RoleCode"></param>
        /// <returns></returns>
        public ActionResult GetLeftMenu(string RoleCode)
        {
            var all = op.GetAllMenu();
            List<I.MES.Models.SelectListItem> itemList = new List<I.MES.Models.SelectListItem>();
            var right = op.GetMenuByRoleCode(RoleCode);

            foreach (var a in all)
            {
                if (right.Where(p => p.Value.Trim() == a.Value.Trim()).ToList().Count == 0)
                    itemList.Add(new I.MES.Models.SelectListItem()
                    {
                        Text = a.Text,
                        Value = a.Value
                    });
            }
            return Json(itemList);
        }

        

        /// <summary>
        /// 根据角色编号获取右边菜单列表
        /// </summary>
        /// <param name="RoleCode"></param>
        /// <returns></returns>
        public ActionResult GetRightMenu(string RoleCode)
        {
            var right = op.GetMenuByRoleCode(RoleCode);
            return Json(right);
        }
        /// <summary>
        /// 新增角色以及角色对应菜单
        /// </summary>
        /// <param name="RoleCode">角色编号</param>
        /// <param name="RoleName">角色名称</param>
        /// <param name="RoleDesc">角色描述</param>
        /// <param name="strPrivCode">菜单编号</param>
        /// <returns></returns>
        public ActionResult AddCfg(string RoleCode, string RoleName, string RoleDesc, string strPrivCode)
        {
            try
            {
                var data = op.GetRoleByRoleCode(RoleCode);
                if (data != null)
                    return Json(new { state = "error", message = string.Format("Submit failed! Reason: role [{0}] already exists。", RoleCode) });

                #region 1.新增角色信息基础表
                SYS_Role role = new SYS_Role();
                role.RoleCode = RoleCode;
                role.RoleName = RoleName;
                role.RoleDesc = RoleDesc;

                role.CrtUser = "";
                role.CrtTime = "";
                role.CrtMachine = "";

                role.LatestModifyUser = this.UserID;
                role.LatestModifyTime = DateTime.Now;
                role.LatestModifyMachine = this.MachineName;

                role.CreateUserAccount = this.UserID;
                role.CreateMachine = this.MachineName;
                role.CreateTime = DateTime.Now;
                role.RoleID = "";

                op.InsertRole(role);
                #endregion

                #region 2.新增角色对应菜单关系表
                string[] privCodeList = strPrivCode.Split(';');
                foreach (string privCode in privCodeList)
                {
                    if (string.IsNullOrEmpty(privCode))
                        continue;
                    var progPriv = op.GetProgPrivByProgCode(privCode);
                    if (progPriv == null)
                        return Json(new { state = "error", message = string.Format("Submit failed! Reason: the menu number [{0}] is in the SYS_ProgPrivDoes not exist in the table", privCode) });

                    SYS_RoleProgPriv RoleProgPriv = new SYS_RoleProgPriv();
                    RoleProgPriv.RoleCode = RoleCode;
                    RoleProgPriv.PrivCode = progPriv.PrivCode;

                    RoleProgPriv.LatestModifyUserAccount = this.UserID;
                    RoleProgPriv.LatestModifyTime = DateTime.Now;
                    RoleProgPriv.LatestModifyMachine = this.MachineName;

                    RoleProgPriv.CreateUserAccount = this.UserID;
                    RoleProgPriv.CreateMachine = this.MachineName;
                    RoleProgPriv.CreateTime = DateTime.Now;

                    op.InsertRoleProgPriv(RoleProgPriv);
                }
                #endregion
                return Json(new { state = "success", message = "Submitted successfully！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

        /// <summary>
        /// 修改角色以及角色对应菜单
        /// </summary>
        /// <param name="RoleCode">角色编号</param>
        /// <param name="RoleName">角色名称</param>
        /// <param name="RoleDesc">角色描述</param>
        /// <param name="strPrivCode">菜单编号</param>
        /// <returns></returns>
        public ActionResult ModifyCfg(string RoleCode, string RoleName, string RoleDesc, string strPrivCode)
        {
            try
            {
                #region 1.修改角色信息基础表
                SYS_Role role = new SYS_Role();
                role.RoleCode = RoleCode;
                role.RoleName = RoleName;
                role.RoleDesc = RoleDesc;

                op.UpdateRole(role);
                #endregion

                #region 2.更新角色对应菜单关系表
                //1.先把该角色下的权限全部删除
                op.DeleteRoleProgPrivByRoleCode(RoleCode);
                //2.新增角色对应菜单关系
                string[] privCodeList = strPrivCode.Split(';');
                foreach (string privCode in privCodeList)
                {
                    if (string.IsNullOrEmpty(privCode))
                        continue;
                    var progPriv = op.GetProgPrivByProgCode(privCode);
                    if (progPriv == null)
                        return Json(new { state = "error", message = string.Format("Modification failed! Reason: the menu number [{0}] is in the SYS_ProgPriv Does not exist in the table", privCode) });

                    SYS_RoleProgPriv RoleProgPriv = new SYS_RoleProgPriv();
                    RoleProgPriv.RoleCode = RoleCode;
                    RoleProgPriv.PrivCode = progPriv.PrivCode;

                    RoleProgPriv.LatestModifyUserAccount = this.UserID;
                    RoleProgPriv.LatestModifyTime = DateTime.Now;
                    RoleProgPriv.LatestModifyMachine = this.MachineName;

                    RoleProgPriv.CreateUserAccount = this.UserID;
                    RoleProgPriv.CreateMachine = this.MachineName;
                    RoleProgPriv.CreateTime = DateTime.Now;

                    op.InsertRoleProgPriv(RoleProgPriv);
                }
                #endregion
                return Json(new { state = "success", message = "Submitted successfully！" });
            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

        /// <summary>
        /// 根据ID删除配置信息
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult DeleteCfg(string ID)
        {
            try
            {
                string mes = "";
                mes = op.DeleteCfg(Convert.ToInt32(ID));
                if (mes == "")
                {
                    return Json(new { state = "success", message = "Deletion succeeded！" });
                }
                else
                {
                    return Json(new { state = "error", message = mes });
                }

            }
            catch (Exception ex)
            {
                return Json(new { state = "error", message = ex.Message });
            }
        }

    }
}
