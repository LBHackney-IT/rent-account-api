using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Domain
{
    public class AuditRecord
    {
        public string User { get; set; }
        public string RentAccountNumber { get; set; }
        public string TimeStamp { get; set; }
        public string CSSOLogin { get; set; }
        public string AuditAction { get; set; }
    }
}
