using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Chronicle.Integrations.EFCore.Persistence;

namespace Chronicle.Integrations.EFCore.Repositories
{
    internal class SagaStateRepository : ISagaStateDBRepository
    {
        private readonly SagaDbContext _dbContext;

        public SagaStateRepository(SagaDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }


        public async Task<EFCoreSagaStateData> GetByIdAsync(SagaId sagaId)
        {
            return await _dbContext.SagaState
                .FirstOrDefaultAsync(sld => sld.SagaId == sagaId.Id);
        }

        public async Task<EFCoreSagaStateData> ReadAsync(SagaId id, Type type)
        {
            return await _dbContext.SagaState
                .FirstOrDefaultAsync(sld => sld.SagaId == id.Id && sld.SagaType == type.FullName);
        }

        public async Task WriteAsync(EFCoreSagaStateData sagaState)
        {
            var entity = await _dbContext
                            .SagaState
                            .FirstOrDefaultAsync(sld => sld.SagaId == sagaState.Id.Id && sld.SagaType == sagaState.SagaType);
            if (entity != null)
            {
                _dbContext.SagaState.Remove(entity);
            }

            await _dbContext.SagaState.AddAsync(
                new EFCoreSagaStateData(sagaState.Id.Id, sagaState.SagaType, sagaState.State, JsonConvert.SerializeObject(sagaState.Data))
            );
            await _dbContext.SaveChangesAsync();
        }
    }
}
