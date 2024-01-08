using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _301270677_prathan__Lab02
{
    public static class Connection
    {
        public readonly static IAmazonS3 s3Client;
        public readonly static AmazonDynamoDBClient dynamoClient;
        private static DynamoDBContext _context;
        public static DynamoDBContext Context
        {
            get
            {
                if (_context == null)
                {
                    // Initialize the DynamoDBContext when access for the first time
                    _context = GetDynamoDBContext();
                }
                return _context;
            }
        }

        static Connection()
        {
            s3Client = GetS3Client();
            dynamoClient = GetDynamoDBClient();
        }
        private static IAmazonS3 GetS3Client()
        {
            return new AmazonS3Client(GetBasicAWSCredentials(), RegionEndpoint.CACentral1);
        }
        private static AmazonDynamoDBClient GetDynamoDBClient()
        {
            return new AmazonDynamoDBClient(GetBasicAWSCredentials(), RegionEndpoint.CACentral1);
        }
        private static DynamoDBContext GetDynamoDBContext() 
        {
            return new DynamoDBContext(GetDynamoDBClient());
        }

        private static BasicAWSCredentials GetBasicAWSCredentials()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            string awsAccessKey = builder.Build().GetSection("AWS").GetSection("AccessKeyId").Value;
            string awsSecretKey = builder.Build().GetSection("AWS").GetSection("SecretAccessKey").Value;

            return new BasicAWSCredentials(awsAccessKey, awsSecretKey);
        }

        //Load content of a DynamoDb table.
        public static Table ContentTableLoad(IAmazonDynamoDB dynamoClient, string tableName)
        {
            Table ContentTable = Table.LoadTable(dynamoClient, tableName);
            return ContentTable;
        }

    }
}
