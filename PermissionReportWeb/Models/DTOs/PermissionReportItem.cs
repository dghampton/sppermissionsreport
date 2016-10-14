using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermissionReportWeb.Models.DTOs
{
    public class PermissionReportItem
    {
        //public string SiteCollection { get; set; }
        public string Web { get; set; }
        //public string WebUrl { get; set; }
        public string ListName { get; set; }
        public string ItemName { get; set; }
        //public string User { get; set; }
        //public string PermissionLevel { get; set; }
        public string Url { get; set; }
    }

    public class PermissionReport
    {
        public List<PermissionReportItem> List { get; set; }
    }
}