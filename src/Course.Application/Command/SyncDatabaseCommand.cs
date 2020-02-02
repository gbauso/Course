using MediatR;
using System;
using System.Threading.Tasks;

namespace Application.Command
{
    public class SyncDatabaseCommand : IRequest<bool>
    {
        public SyncDatabaseCommand(DateTime lastExecution)
        {
            LastExecution = lastExecution;
        }

        public DateTime LastExecution { get; set; }
    }
}
