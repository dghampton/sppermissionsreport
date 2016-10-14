using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermissionReportWeb.Models.DTOs
{
    public class AdminReportItem
    {
        public string SiteCollectionOwner { get; set; }
        public List<string> SiteCollectionAdmins { get; set; }
    }
}