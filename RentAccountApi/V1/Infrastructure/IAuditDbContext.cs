using Amazon.DynamoDBv2.DataModel;
using RentAccountApi.V1.Boundary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Infrastructure
{
    public interface IAuditDbContext<T> where T : class
    {
        void Save(MyRentAccountAudit myRentAccountAudit, DynamoDBOperationConfig config);
    }
}
