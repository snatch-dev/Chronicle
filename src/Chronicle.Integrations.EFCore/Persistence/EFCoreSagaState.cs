using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Chronicle.Integrations.EFCore.Persistence
{
    internal class EFCoreSagaState<TContext> : ISagaStateRepository where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public EFCoreSagaState(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ISagaState> ReadAsync(SagaId id, Type type)
            => await _dbContext.Set<EFCoreSagaStateData>()
                .FirstOrDefaultAsync(sld => sld.SagaId == id.Id && sld.SagaType == type.FullName);

        public async Task WriteAsync(ISagaState state)
        {
            var entity = await _dbContext
                .Set<EFCoreSagaStateData>()
                .FirstOrDefaultAsync(sld => sld.SagaId == state.Id.Id && sld.SagaType == state.Type.ToString());
            if (entity is {})
            {
                _dbContext.Set<EFCoreSagaStateData>().Remove(entity);
            }

            await _dbContext.Set<EFCoreSagaStateData>().AddAsync(
                new EFCoreSagaStateData(state.Id.Id, state.Type.ToString(), state.State, JsonConvert.SerializeObject(state.Data))
            );
            await _dbContext.SaveChangesAsync();
        }
    }
}
