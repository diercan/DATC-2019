using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureFunction.Storage
{
    public class TableStorageService<T> : IDisposable where T : TableEntity, new()
    {
        private readonly CloudTable _table;
        private readonly ILogger _logger;
        public TableStorageService(ILogger logger, string connectionString, string tableName)
        {
            _logger = logger;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(tableName);

            // Create the table if it doesn't exist.
            _table.CreateIfNotExistsAsync().Wait();
        }

        public IEnumerable<T> GetAllEntries()
        {
            try
            {
                TableContinuationToken token = null;
                var entities = new List<T>();
                do
                {
                    var queryResult = _table.ExecuteQuerySegmentedAsync(new TableQuery<T>(), token).Result;
                    entities.AddRange(queryResult.Results);
                    token = queryResult.ContinuationToken;
                } while (token != null);

                return entities.AsEnumerable();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Exception occured when getting all entities");
                return null;
            }
            
        }

        public IEnumerable<T> GetByPartitionId(string partitionKey)
        {
            try
            {
                TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
                var result = new List<T>();
                TableContinuationToken continuationToken = null;
                do
                {
                    var page = _table.ExecuteQuerySegmentedAsync(query, continuationToken).Result;
                    continuationToken = page.ContinuationToken;
                    result.AddRange(page.Results);
                }
                while (continuationToken != null);

                return result.AsEnumerable();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Exception occured when getting all entities by the PartitionId");
                return null;
            }
        }

        public IEnumerable<T> GetByRowKeyId(string rowKey)
        {
            try
            {
                TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));
                var result = new List<T>();
                TableContinuationToken continuationToken = null;
                do
                {
                    var page = _table.ExecuteQuerySegmentedAsync(query, continuationToken).Result;
                    continuationToken = page.ContinuationToken;
                    result.AddRange(page.Results);
                }
                while (continuationToken != null);

                return result.AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Exception occured when getting all entities by the RowKey");
                return null;
            }
        }

        public IEnumerable<T> GetByPartitionAndRowKeys(string partitionKey, string rowKey)
        {
            try
            {
                TableQuery<T> query = new TableQuery<T>()
                    .Where(TableQuery.CombineFilters(
                        TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey),
                        TableOperators.And,
                        TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey)));
                var result = new List<T>();
                TableContinuationToken continuationToken = null;
                do
                {
                    var page = _table.ExecuteQuerySegmentedAsync(query, continuationToken).Result;
                    continuationToken = page.ContinuationToken;
                    result.AddRange(page.Results);
                }
                while (continuationToken != null);

                return result.AsEnumerable();
            }     
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Exception occured when getting all entities by the RowKey and PartitionKey");
                return null;
            }
        }

        public bool InsertOrUpdateEntry(T entry)
        {
            try
            {
                var insertOrReplaceOperation = TableOperation.InsertOrReplace(entry);
                var result = _table.ExecuteAsync(insertOrReplaceOperation).Result;
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An Exception occured when inserting or updating an entry");
                return false;
            }            
        }

        public bool DeleteEntry(Guid partitionKey, Guid rowKey)
        {
            try
            {
                TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey.ToString(), rowKey.ToString());
                var retrieveResult = _table.ExecuteAsync(retrieveOperation).Result;

                T deleteEntity = (T)retrieveResult.Result;
                if (deleteEntity != null)
                {
                    var deleteOperation = TableOperation.Delete(deleteEntity);
                    var result = _table.ExecuteAsync(deleteOperation).Result;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured when deleting entry.");
                return false;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}