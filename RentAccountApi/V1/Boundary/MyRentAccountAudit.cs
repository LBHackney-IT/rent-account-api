using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Boundary
{
    [DynamoDBTable("MyRentAccountAudit")]
    public class MyRentAccountAudit
    {
        [DynamoDBHashKey]
        public string User { get; set; }
        [DynamoDBProperty]
        public string RentAccountNumber { get; set; }
        [DynamoDBProperty]
        public string TimeStamp { get; set; }
    }
}
