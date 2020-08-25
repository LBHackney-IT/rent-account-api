using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Boundary
{
    public class MyRentAccountAudit
    {
        public string User { get; set; }
        public string RentAccountNumber { get; set; }
        public string TimeStamp { get; set; }
        public string CSSOLogin { get; set; }
    }
}
