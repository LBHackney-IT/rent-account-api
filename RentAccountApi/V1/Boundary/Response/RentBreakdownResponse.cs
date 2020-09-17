using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Boundary.Response
{
    public class RentBreakdownResponse
    {
        public List<RentBreakdown> RentBreakdowns { get; set; }
    }

    public class RentBreakdown
    {
        public string description { get; set; }
        public string code { get; set; }
        public string value { get; set; }
        public string effectiveDate { get; set; }
        public string lastChargeDate { get; set; }
    }
}
