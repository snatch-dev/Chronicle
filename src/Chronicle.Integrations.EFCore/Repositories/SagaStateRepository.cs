using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Chronicle.Integrations.EFCore.Persistence;

namespace Chronicle.Integrations.EFCore.Repositories
{
    internal class SagaStateRepository<TContext> : ISagaStateDBRepository where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public SagaStateRepository(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<EFCoreSagaStateData> ReadAsync(SagaId id, Type type)
        {
            return await _dbContext.Set<EFCoreSagaStateData>()
                .FirstOrDefaultAsync(sld => sld.SagaId == id.Id && sld.SagaType == type.FullName);
        }

        public async Task WriteAsync(EFCoreSagaStateData sagaState)
        {
            var entity = await _dbContext
                            .Set<EFCoreSagaStateData>()
                            .FirstOrDefaultAsync(sld => sld.SagaId == sagaState.Id.Id && sld.SagaType == sagaState.SagaType);
            if (entity != null)
            {
                _dbContext.Set<EFCoreSagaStateData>().Remove(entity);
            }

            await _dbContext.Set<EFCoreSagaStateData>().AddAsync(
                new EFCoreSagaStateData(sagaState.Id.Id, sagaState.SagaType, sagaState.State, JsonConvert.SerializeObject(sagaState.Data))
            );
        }
    }
}
