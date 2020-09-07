using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Boundary.Response
{
    public class LinkedAccountResponse
    {
        public string LinkedAccountId { get; set; }
        public string CSSOId { get; set; }
        public string AccountNumber { get; set; }
    }
}
