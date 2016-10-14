using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using PermissionReportWeb.Models.DTOs;
using System.Linq;
using System.Web;

namespace PermissionReportWeb.Repositories
{
    public interface IPermissionRepository
    {
        string GetSiteCollectionOwner(string siteCollectionUrl);
        List<string> GetSiteCollectionAdmins(string siteCollectionUrl);
        List<Web> GetAllWebsInSiteCollection(string siteCollectionUrl);
        List<PermissionReportItem> GetAllUniquelyPermissionedObjects(List<Web> websList);
    }
    public class PermissionRepository : IPermissionRepository
    {
        public PermissionRepository()
        {

        }

        public string GetSiteCollectionOwner(string siteCollectionUrl)
        {
            using (var clientContext = new ClientContext(siteCollectionUrl))
            {
                var site = clientContext.Site;
                clientContext.Load(site.Owner);
                clientContext.ExecuteQuery();
                return site.Owner.LoginName;
            }
        }

        public List<string> GetSiteCollectionAdmins(string siteCollectionUrl)
        {
            var adminList = new List<string>();
            using (var clientContext = new ClientContext(siteCollectionUrl))
            {
                var userCollection = clientContext.Site.RootWeb.SiteUsers;
                clientContext.Load(userCollection);
                clientContext.ExecuteQuery();

                foreach (var user in userCollection)
                {
                    if (user.IsSiteAdmin)
                    {
                        adminList.Add(user.LoginName);
                    }
                }
            }
            return adminList;
        }

        public List<Web> GetAllWebsInSiteCollection(string siteCollectionUrl)
        {
            using (var clientContext = new ClientContext(siteCollectionUrl))
            {
                var web = clientContext.Site.RootWeb;
                clientContext.Load(web, w=>w.Title,w=>w.Url,w=>w.HasUniqueRoleAssignments);
                clientContext.ExecuteQuery();
                return GetAllSubwebs(web);
            }
        }

        private List<Web> GetAllSubwebs(Web web)
        {
            List<Web> returnList = new List<Web>();
            returnList.Add(web);
            using (var ctx = new ClientContext(web.Url))
            {
                var webs = ctx.Web.Webs;
                ctx.Load(webs);
                ctx.ExecuteQuery();
                foreach (var subWeb in webs)
                {
                    ctx.Load(subWeb, w => w.Title, w => w.Url, w => w.HasUniqueRoleAssignments);
                    ctx.ExecuteQuery();
                    returnList.AddRange(GetAllSubwebs(subWeb));
                }
            }
            return returnList;
        }

        public List<PermissionReportItem> GetAllUniquelyPermissionedObjects(List<Web> websList)
        {
            var reportList = new List<PermissionReportItem>();

            foreach (var subWeb in websList)
            {
                var ctx = new ClientContext(subWeb.Url);
                var web = ctx.Web;
                ctx.Load(web, w => w.Title, w => w.HasUniqueRoleAssignments, w => w.Lists, w=>w.Url);
                ctx.ExecuteQuery();
                reportList.Add(Helpers.Helpers.GetPermissionReportItemFromWeb(web));
                foreach (var list in web.Lists)
                {
                    ctx.Load(list, l => l.Title, l => l.HasUniqueRoleAssignments, l => l.Hidden, l=>l.RootFolder);
                    ctx.ExecuteQuery();
                    if (!list.Hidden)
                    {
                        if (list.HasUniqueRoleAssignments)
                        {
                            reportList.Add(Helpers.Helpers.GetPermissionReportItemFromList(web, list));
                        }
                        CamlQuery query = new CamlQuery { ViewXml = "" };

                        var items = list.GetItems(query);
                        ctx.Load(items);
                        ctx.ExecuteQuery();

                        foreach (var item in items)
                        {
                            ctx.Load(item, i => i.DisplayName, i => i.HasUniqueRoleAssignments);
                            ctx.ExecuteQuery();
                            if (item.HasUniqueRoleAssignments)
                            {
                                reportList.Add(Helpers.Helpers.GetPermissionReportItemFromItem(web, list, item));
                            }
                        }
                    }
                }
            }
            return reportList;
        }
    }
}