using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Job
{
    public static class DatabaseSyncFunction
    {
        private const string Cron = "0 */2 * * * *";
        private static HttpClient _Client = new HttpClient();

        [FunctionName("DatabaseSyncFunction")]
        public static async Task RunAsync([TimerTrigger(Cron)]TimerInfo myTimer,
                                          [Table("JobExecution", Connection = "AzureTableStorage")] CloudTable cloudTable,
                                          ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");

            var lastExecution = await GetLastSucessfulyExecution(cloudTable);

            var result = await InvokeSyncJob(lastExecution);

            await InsertExecution(cloudTable, result);

            log.LogInformation($"C# Timer trigger function finished at: {DateTime.UtcNow}");
        }

        private static async Task<bool> InvokeSyncJob(DateTime lastExecution)
        {
            var url = Environment.GetEnvironmentVariable("DatabaseSyncUrl");
            var result = await _Client.PostAsJsonAsync(url, lastExecution);

            return result.IsSuccessStatusCode;
        }

        private static async Task<DateTime> GetLastSucessfulyExecution(CloudTable cloudTable)
        {
            var query = new TableQuery<JobExecution>().Where(
                            TableQuery.GenerateFilterConditionForBool("Sucessfull", QueryComparisons.Equal, true))
                            .Take(1);

            var result = await cloudTable.ExecuteQuerySegmentedAsync(query, new TableContinuationToken());

            var data = result.Results.FirstOrDefault()?.Timestamp ?? DateTime.Now.AddHours(-2);

            return data.UtcDateTime;

        }

        private static async Task InsertExecution(CloudTable cloudTable, bool sucess)
        {
            var job = new JobExecution(sucess);

            var operation = TableOperation.Insert(job);

            await cloudTable.ExecuteAsync(operation);
        }

    }
}
