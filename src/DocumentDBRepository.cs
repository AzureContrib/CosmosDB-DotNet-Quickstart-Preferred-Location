﻿namespace CosmosDb.QuickStart
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;

    public static class DocumentDBRepository<T> where T : class
    {
        private static readonly string DatabaseId = Constants.database;
        private static readonly string CollectionId = Constants.collection;
        private static DocumentClient client;
        private static ConnectionPolicy connectionPolicy;

        public static async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public static async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public static async Task<Document> CreateItemAsync(T item)
        {
            return await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId), item);
        }

        public static async Task<Document> UpdateItemAsync(string id, T item)
        {
            return await client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id), item);
        }

        public static async Task DeleteItemAsync(string id)
        {
            await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(DatabaseId, CollectionId, id));
        }

        public static void Initialize()
        {
           
            //Default: Direct Mode, based on appSettings configuration, application would pick up right connection mode in Run Time.
            var connectionMode = Constants.connectionMode.Trim().Equals("direct", StringComparison.OrdinalIgnoreCase) ? ConnectionMode.Direct : ConnectionMode.Gateway; 
                                                                                                                                                                        
            //Default: HTTPS, based on appSettings configuration, application would pick up right connection mode in Run Time.
            var connectionProtocol = Constants.connectionProtocol.Trim().Equals("tcp", StringComparison.OrdinalIgnoreCase) ? Protocol.Tcp : Protocol.Https;

            var maxConnectionLimit = Constants.maxConnectionLimit;

            

            //Setting up a Connection Policy to be passed on while creating DocumentClient
            ConnectionPolicy connectionPolicy = new ConnectionPolicy
            {
                ConnectionMode = connectionMode,
                ConnectionProtocol = connectionProtocol,
                MaxConnectionLimit = maxConnectionLimit,
                RetryOptions = new RetryOptions() { MaxRetryAttemptsOnThrottledRequests = 5, MaxRetryWaitTimeInSeconds = 5 } // setting a retry for Connection Throttling and Retry.
            };

            //Setting read region selection preference
            connectionPolicy.PreferredLocations.Add(LocationNames.EastUS); // applications first preference
            connectionPolicy.PreferredLocations.Add(LocationNames.WestEurope); // applications second preference


            client = new DocumentClient(new Uri(Constants.endpoint), Constants.authKey, connectionPolicy);
            CreateDatabaseIfNotExistsAsync().Wait();
            CreateCollectionIfNotExistsAsync().Wait();
        }

        private static async Task CreateDatabaseIfNotExistsAsync()
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(DatabaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = DatabaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync()
        {
            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(DatabaseId, CollectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(DatabaseId),
                        new DocumentCollection { Id = CollectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}