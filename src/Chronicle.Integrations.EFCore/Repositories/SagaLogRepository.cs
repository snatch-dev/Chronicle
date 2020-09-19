using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Chronicle.Integrations.EFCore.Persistence;

namespace Chronicle.Integrations.EFCore.Repositories
{
    internal class SagaLogRepository<TContext> : ISagaLogRepository where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public SagaLogRepository(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IReadOnlyCollection<EFCoreSagaLogData>> ReadAsync(SagaId id, Type type)
        {
            return await _dbContext.Set<EFCoreSagaLogData>()
                .Where(sld => sld.SagaId == id.Id && sld.SagaType == type.FullName)
                .ToArrayAsync();
        }

        public async Task WriteAsync(EFCoreSagaLogData message)
        {
            if (null == message)
                throw new ArgumentNullException(nameof(message));
            await _dbContext.Set<EFCoreSagaLogData>().AddAsync(message);
        }
    }
}
