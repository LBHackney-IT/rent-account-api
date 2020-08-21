using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using RentAccountApi.V1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Infrastructure
{
    public class DynamoDBContext<T> : DynamoDBContext, IDynamoDBContext<T>
    where T : class
    {
        private DynamoDBOperationConfig _config;

        public DynamoDBContext(IAmazonDynamoDB client, string tableName)  : base(client)
        {
            _config = new DynamoDBOperationConfig()
            {
                OverrideTableName = tableName
            };
        }

        public void Save(T item)
        {
            var response = SaveAsync(item, _config);
            if (response.Exception != null)
            {
                throw new AuditNotInsertedException(response.Exception.Message);
            }
        }
    }
}
