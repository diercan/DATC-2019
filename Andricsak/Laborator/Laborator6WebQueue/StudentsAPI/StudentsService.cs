using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace Services
{
    public class StudentsService : IDisposable
    {
        private CloudTable studentsTable;

        public StudentsService()
        {
            string storageConnectionString = "DefaultEndpointsProtocol=https;"
                            + "AccountName="
                            + ";AccountKey="
                            + ";EndpointSuffix=core.windows.net";

            var account = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = account.CreateCloudTableClient();

            studentsTable = tableClient.GetTableReference("StudentiAA");
        }

        public async Task Initialize()
        {
            await studentsTable.CreateIfNotExistsAsync();
        }

        public async Task<List<StudentEntity>> GetStudents()
        {
            if (studentsTable == null)
            {
                throw new Exception();
            }

            var students = new List<StudentEntity>();
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>(); //.Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "Smith"));

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;
                students.AddRange(resultSegment.Results);
            } while (token != null);

            return students;
        }

        public async Task<TableResult> AddStudent(StudentEntity student)
        {
            if (studentsTable == null)
            {
                throw new Exception();
            }

            var insertOperation = TableOperation.Insert(student);
            return await studentsTable.ExecuteAsync(insertOperation);
        }

        public void UpdateStudent(StudentEntity student)
        {
            // Must be implemented
        }

        public void DeleteStudent(StudentEntity student)
        {
            // Must be implemented
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
