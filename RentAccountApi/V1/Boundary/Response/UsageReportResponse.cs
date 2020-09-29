using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Boundary.Response
{
    public class UsageReportResponse
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; } 
        public int? UniqueAnonymousUsers { get; set; }
        public int? TotalAnonymousLogins { get; set; }
        public int? UniqueCSSOUsers { get; set; }
        public int? TotalCSSOLogins { get; set; }
        public int? NewCSSOLinkedAccounts { get; set; }
        public int? TotalLogins { get; set; }
    }
}
