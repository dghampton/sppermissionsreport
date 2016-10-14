using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermissionReportWeb.Models.DTOs
{
    public class SubwebReportItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string HasUniquePermissions { get; set; }
    }

    public class SubwebReport
    {
        public List<SubwebReportItem> List { get; set; }
    }
}