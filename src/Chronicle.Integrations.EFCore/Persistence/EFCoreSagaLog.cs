using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Chronicle.Integrations.EFCore.Persistence
{
    internal class EFCoreSagaLog<TContext> : ISagaLog where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public EFCoreSagaLog(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<ISagaLogData>> ReadAsync(SagaId id, Type type)
           => await _dbContext.Set<EFCoreSagaLogData>()
                .Where(sld => sld.SagaId == id.Id && sld.SagaType == type.FullName)
                .ToArrayAsync();

        public async Task WriteAsync(ISagaLogData message)
        {
            if (null == message)
                throw new ArgumentNullException(nameof(message));
            await _dbContext.Set<EFCoreSagaLogData>().AddAsync(new EFCoreSagaLogData(message.Id.Id, message.Type.ToString(), message.CreatedAt, JsonConvert.SerializeObject(message.Message)));
            await _dbContext.SaveChangesAsync();
        }
    }

}
