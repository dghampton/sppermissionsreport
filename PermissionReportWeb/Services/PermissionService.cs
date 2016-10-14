using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PermissionReportWeb.Repositories;
using PermissionReportWeb.Models.DTOs;

namespace PermissionReportWeb.Services
{
    public interface IPermissionService
    {
        AdminReportItem GetAdminReport(string siteCollectionUrl);
        SubwebReport GetAllWebs(string siteCollectionUrl);
        PermissionReport GetPermissionsReportForSiteCollection(string siteCollectionUrl);
    }
    public class PermissionService : IPermissionService
    {
        IPermissionRepository _repo;
        public PermissionService(IPermissionRepository repo)
        {
            _repo = repo;
        }

        public AdminReportItem GetAdminReport(string siteCollectionUrl)
        {
            var owner = _repo.GetSiteCollectionOwner(siteCollectionUrl);
            var admins = _repo.GetSiteCollectionAdmins(siteCollectionUrl);
            return new AdminReportItem
            {
                SiteCollectionOwner = Helpers.Helpers.FormatUserId(owner),
                SiteCollectionAdmins = Helpers.Helpers.FormatUserList(admins)
            };
        }

        public SubwebReport GetAllWebs(string siteCollectionUrl)
        {
            var webInfoList = new List<SubwebReportItem>();
            var webCollection = _repo.GetAllWebsInSiteCollection(siteCollectionUrl);
            webCollection.ForEach(x => webInfoList.Add(Helpers.Helpers.GetReportItemFromWeb(x)));
            return new SubwebReport
            {
                List = webInfoList
            };
        }

        public PermissionReport GetPermissionsReportForSiteCollection(string siteCollectionUrl)
        {
            var webCollection = _repo.GetAllWebsInSiteCollection(siteCollectionUrl);
            var reportList = _repo.GetAllUniquelyPermissionedObjects(webCollection);
            return new PermissionReport
            {
                List = reportList
            };
        }
    }
}