using Infrastructure.Database.Query.Model;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Database.Query
{
    public class CosmosDbManager<T> : IReadDatabase<T> where T: IQueryModel
    {
        private readonly Container _Container;

        public CosmosDbManager(IConfiguration configuration)
        {
            var client = new CosmosClient(configuration.GetConnectionString("ReadDatabase"));

            this._Container = client.GetContainer("report", typeof(T).Name);
        }

        public async Task AddItem(T item)
        {
            await _Container.CreateItemAsync(item, new PartitionKey(item.Id));
        }

        public void Dispose()
        {
        }

        public async Task<T> GetItem(string id)
        {
            try
            {
                var response = await _Container.ReadItemAsync<T>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return default;
            }
        }

        public async Task<IEnumerable<T>> GetItems(string[] fields)
        {
            var query = _Container.GetItemQueryIterator<T>(GetQuery(fields));
            var results = new List<T>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task RemoveItem(string id)
        {
            try
            {
                await _Container.DeleteItemAsync<T>(id, new PartitionKey(id));
            }
            catch { }
        }

        private string GetQuery(string[] fields)
        {
            var projection = fields.Length == 0 ? "*" : string.Join(", q.", fields);

            return $"SELECT q.{projection} FROM {typeof(T).Name} q";
        }
    }
}
