using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint.Client;
using PermissionReportWeb.Models.DTOs;

namespace PermissionReportWeb.Helpers
{
    public static class Helpers
    { 
        public static string FormatUserId(string rawId)
        {
            return rawId.Split('\\')[1];
        }

        public static List<string> FormatUserList(List<string> userList)
        {
            return userList.Select(x => FormatUserId(x)).ToList();
        }

        public static SubwebReportItem GetReportItemFromWeb(Web web)
        {
            return new SubwebReportItem
            {
                Name = web.Title,
                Url = web.Url,
                HasUniquePermissions = web.HasUniqueRoleAssignments.ToString()
            };
        }

        public static PermissionReportItem GetPermissionReportItemFromWeb(Web web)
        {
            return new PermissionReportItem
            {
                Web = web.Title,
                ListName = "",
                ItemName = "",
                Url = web.Url
            };
        }

        public static PermissionReportItem GetPermissionReportItemFromList(Web web, List list)
        {
            return new PermissionReportItem
            {
                Web = web.Title,
                ListName = list.Title,
                ItemName = "",
                Url = web.Url + list.RootFolder.ServerRelativeUrl
            };
        }

        public static PermissionReportItem GetPermissionReportItemFromItem(Web web, List list, ListItem item)
        {
            return new PermissionReportItem
            {
                Web = web.Title,
                ListName = list.Title,
                //ItemName = item["Title"].ToString(),
                ItemName = item.DisplayName,
                Url = web.Url + list.RootFolder.ServerRelativeUrl
            };
        }
    }
}