using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Boundary
{
    public class MyRentAccountResidentAudit
    {
        public string hackney_accountnumber { get; set; }
        public string hackney_accounttype { get; set; }
        public string hackney_name { get; set; }
        public string hackney_postcode { get; set; }
        public string hackney_tagreferencenumber { get; set; }
    }
}
