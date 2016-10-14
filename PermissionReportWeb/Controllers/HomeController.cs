using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PermissionReportWeb.Services;

namespace PermissionReportWeb.Controllers
{
    public class HomeController : Controller
    {
        IPermissionService _service;

        public HomeController(IPermissionService service)
        {
            _service = service;
        }

        [SharePointContextFilter]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult AllWebs()
        {
            ViewBag.Message = "A list of all webs in the current site collection.";
            var siteCollectionUrl = Request.QueryString["SPHostUrl"];
            var webInfo = _service.GetAllWebs(siteCollectionUrl);
            return View(webInfo);
        }

        public ActionResult UniquePermissions()
        {
            ViewBag.Message = "A list of all objects in the current site collection that have unique permissions.";
            var siteCollectionUrl = Request.QueryString["SPHostUrl"];
            var permissionReportData = _service.GetPermissionsReportForSiteCollection(siteCollectionUrl);
            return View(permissionReportData);
        }

        public ActionResult OwnersAdmins()
        {
            ViewBag.Message = "See the Site Collection owner and a list of the site collection administrators.";
            var siteCollectionUrl = Request.QueryString["SPHostUrl"];
            var adminReportData = _service.GetAdminReport(siteCollectionUrl);
            return View(adminReportData);
        }
    }
}