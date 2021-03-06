using System.Collections.Generic;
using RentAccountApi.V1.Boundary.Request;
using Amazon.DynamoDBv2.DataModel;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.Factories;
using RentAccountApi.V1.Infrastructure;
using System;
using Amazon.DynamoDBv2.DocumentModel;
using RentAccountApi.V1.Boundary;
using Amazon.DynamoDBv2;
using Gateways;
using System.Threading.Tasks;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Amazon.Runtime;
using System.Linq;
using RentAccountApi.V1.Gateways.Helpers;

namespace RentAccountApi.V1.Gateways
{
    public class AuditDatabaseGateway : IAuditDatabaseGateway
    {
        private readonly Table _documentsTable;
        private AmazonDynamoDBClient _databaseClient;

        public AuditDatabaseGateway(IDynamoDBHandler database)
        {
            _documentsTable = database.DocumentTable;
            _databaseClient = database.DynamoDBClient;
        }

        public async Task GenerateAdminAuditRecord(MyRentAccountAdminAudit generateAuditRequest)
        {
            LambdaLogger.Log(string.Format("Saving to DB - {0}", JsonConvert.SerializeObject(generateAuditRequest)));
            var documentItem = DynamoHelper.ConstructAdminAuditDynamoDocument(generateAuditRequest);
            try
            {
                await _documentsTable.PutItemAsync(documentItem).ConfigureAwait(true);
            }
            catch (AmazonServiceException ase)
            {
                LambdaLogger.Log("Could not complete operation");
                LambdaLogger.Log("Error Message:  " + ase.Message);
                LambdaLogger.Log("HTTP Status:    " + ase.StatusCode);
                LambdaLogger.Log("AWS Error Code: " + ase.ErrorCode);
                LambdaLogger.Log("Error Type:     " + ase.ErrorType);
                LambdaLogger.Log("Request ID:     " + ase.RequestId);

            }
            catch (AmazonClientException ace)
            {
                LambdaLogger.Log("Internal error occurred communicating with DynamoDB");
                LambdaLogger.Log("Error Message:  " + ace.Message);
            }
        }

        public async Task<List<AdminAuditRecord>> GetAuditByUser(string user, int recordLimit)
        {
            var tableName = _documentsTable.TableName;
            var queryRequest = new QueryRequest
            {
                TableName = tableName,
                Limit = recordLimit,
                ScanIndexForward = false,
                KeyConditionExpression = "#User = :value",
                ExpressionAttributeNames = new Dictionary<string, string> { { "#User", "User" } },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                {
                    {":value", new AttributeValue {S = user}},
                },
            };
            var results = await _databaseClient.QueryAsync(queryRequest);
            return results.Items.Select(entry => new AdminAuditRecord
            {
                User = entry["User"].S?.ToString(),
                RentAccountNumber = entry["RentAccountNumber"].S?.ToString(),
                TimeStamp = entry["TimeStamp"].S?.ToString(),
                CSSOLogin = entry["CSSOLogin"].S?.ToString(),
                AuditAction = entry["AuditAction"].S?.ToString()
            }).ToList(); //TODO: how do we deal with database entries that have empty columns? i.e. records created prior to a new column being added.
        }
    }
}
