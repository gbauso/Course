using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace Job
{
    public class JobExecution : TableEntity
    {
        public JobExecution() : base()
        {

        }
        public JobExecution(bool sucessfull)
        {
            var date = DateTime.UtcNow;
            Timestamp = date;
            RowKey = Guid.NewGuid().ToString();
            PartitionKey = (DateTime.MaxValue.Ticks - date.Ticks).ToString();
            Sucessfull = sucessfull;
        }

        public bool Sucessfull { get; set; }
    }
}
