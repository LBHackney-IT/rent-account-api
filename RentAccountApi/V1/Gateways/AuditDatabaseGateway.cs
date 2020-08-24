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

        public async Task GenerateAuditRecord(MyRentAccountAudit generateAuditRequest)
        {
            LambdaLogger.Log(string.Format("Saving to DB - {0}", JsonConvert.SerializeObject(generateAuditRequest)));
            var documentItem = ConstructDynamoDocument(generateAuditRequest);
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
        private static Document ConstructDynamoDocument(MyRentAccountAudit generateAuditRequest)
        {
            return new Document
            {
                ["User"] = generateAuditRequest.User,
                ["TimeStamp"] = generateAuditRequest.TimeStamp,
                ["RentAccountNumber"] = generateAuditRequest.RentAccountNumber
            };
        }
    }
}
