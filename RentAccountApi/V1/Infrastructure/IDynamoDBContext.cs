using Amazon.DynamoDBv2.DataModel;
using RentAccountApi.V1.Boundary;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Infrastructure
{
    public interface IDynamoDBContext<T> where T : class
    {
        Task Save(T myRentAccountAudit);
    }
}
