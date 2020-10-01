using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Domain
{
    public class UsageReportRequestException : Exception
    {
        public UsageReportRequestException()
        {

        }

        public UsageReportRequestException(string message) : base(message)
        {

        }
    }
}
