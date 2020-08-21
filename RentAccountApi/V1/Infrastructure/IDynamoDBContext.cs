using Amazon.DynamoDBv2.DataModel;
using RentAccountApi.V1.Boundary;

namespace RentAccountApi.V1.Infrastructure
{
    public interface IDynamoDBContext<T> where T : class
    {
        void Save(T myRentAccountAudit);
    }
}
