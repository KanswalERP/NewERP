﻿using NtierMvc.Areas.Admin.Models;
using NtierMvc.Common;
using NtierMvc.Infrastructure;
using NtierMvc.Model;
using NtierMvc.Model.Admin;
using NtierMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NtierMvc.Areas.Admin.Controllers
{
    [SessionExpire]
    public class AdminController : Controller
    {
        BaseModel model;
        private AdminManager objManager;

        public AdminController()
        {
            model = new BaseModel();
            objManager = new AdminManager();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AdminMaster()
        {
            ViewBag.ListDeptName = model.GetMasterTableStringList("Master.Department", "Id", "DeptName", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListEmployee = model.GetMasterTableStringList("Master.Employee", "Id", "EmpName", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListMainMenu = model.GetMasterTableStringList("MenuTable", "Id", "UrlName", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListSubMenu = model.GetMasterTableStringList("SubMenuTable", "Id", "Name", "", "", GeneralConstants.ListTypeD);
            ViewBag.ListReadWrite = model.GetDropDownList("Master.Taxonomy",GeneralConstants.ListTypeD,"DropDownId", "DropDownValue", "ReadWrite", "Property");

            return View();
        }

        [HttpGet]
        public ActionResult PartialAdminDashboard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetRoleURLDetails(string deptName = null, string mainMenu = null, string subMenu = null, string access = null)
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var totalRecords = 0;
            var objList = new List<RoleAssignEntity>();
            try
            {
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var search = Request.Form.GetValues("search[value]").FirstOrDefault();


                objList = objManager.GetRoleURLDetails(skip, pageSize, sortColumn, sortColumnDir, search, deptName, mainMenu, subMenu, access);
                totalRecords = objList[0].totalcount;
                var v = (from a in objList select a);
                objList = v.ToList();
            }
            catch (Exception ex)
            {
                //ex.Message;
            }

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = objList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SaveRoleAssigns(string Role, string EmpId, string MainMenu, string SubMenu, string ReadWrite)
        {
            RoleAssignEntity raEntity = new RoleAssignEntity();
            raEntity.DeptName = Role;
            raEntity.EmpId = EmpId;
            raEntity.MainMenu = MainMenu;
            raEntity.SubMenu = SubMenu;
            raEntity.ReadWrite = ReadWrite;
            string result = objManager.SaveRoleAssigns(raEntity);

            string data = string.Empty;
            if (!string.IsNullOrEmpty(result) && (result == GeneralConstants.Inserted || result == GeneralConstants.Updated))
            {
                data = GeneralConstants.SavedSuccess;
            }
            else
            {
                data = GeneralConstants.NotSavedError + ". Reason: " + result;
            }

            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetSubMenus(string mainMenu)
        {
            try
            {
                List<DropDownEntity> ddl = objManager.GetSubMenus(mainMenu);
                return new JsonResult { Data = ddl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                throw;
            }

        }

        public JsonResult GetEmployeeForDept(string deptId)
        {
            try
            {
                var ddl = model.GetDropDownList("Master.Employee", GeneralConstants.ListTypeD, "ID", "EMPNAME", deptId, "Dept");
                return new JsonResult { Data = ddl, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            catch (Exception ex)
            {
                return new JsonResult { Data = ex, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                throw;
            }

        }

        [HttpGet]
        public ActionResult PartialAssignAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveAdminAssigns(string DeptId, string EmpId)
        {
            RoleAssignEntity raEntity = new RoleAssignEntity();
            raEntity.DeptId = DeptId;
            raEntity.EmpId = EmpId;
            string result = objManager.SaveAdminAssigns(raEntity);

            string data = string.Empty;
            if (!string.IsNullOrEmpty(result) && (result == GeneralConstants.Inserted || result == GeneralConstants.Updated))
            {
                data = GeneralConstants.SavedSuccess;
            }
            else
            {
                data = GeneralConstants.NotSavedError + ". Reason: " + result;
            }

            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public ActionResult GetAdminAssigns()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var totalRecords = 0;
            var objList = new List<RoleAssignEntity>();
            try
            {
                var sortColumn = Request.Form.GetValues("columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]").FirstOrDefault();
                var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
                var pageSize = length != null ? Convert.ToInt32(length) : 0;
                var skip = start != null ? Convert.ToInt32(start) : 0;
                var search = Request.Form.GetValues("search[value]").FirstOrDefault();


                objList = objManager.GetAdminAssigns(skip, pageSize, sortColumn, sortColumnDir, search);
                var v = (from a in objList select a);
                objList = v.ToList();
                totalRecords = Convert.ToInt32(objList[0].totalcount);
            }
            catch (Exception ex)
            {
                //ex.Message;
            }

            return Json(new { draw = draw, recordsFiltered = totalRecords, recordsTotal = totalRecords, data = objList }, JsonRequestBehavior.AllowGet);
        }

    }
}