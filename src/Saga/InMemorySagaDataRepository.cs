using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga
{
    public class InMemorySagaDataRepository<TData> : ISagaDataRepository<TData> where TData : class, ISagaData
    {
        private readonly List<TData> _storage;

        public InMemorySagaDataRepository()
        {
            _storage = new List<TData>();
        }

        public async Task<TData> ReadAsync(Guid id)
            => await Task.FromResult(_storage.FirstOrDefault(d => d.Id == id));

        public async Task WriteAsync(TData data)
        {
            var storedData = await ReadAsync(data.Id);

            if(storedData is null)
            {
                _storage.Add(data);
            }
            else
            {
                _storage.Remove(storedData);
                _storage.Add(data);
            }
        }
    }
}
