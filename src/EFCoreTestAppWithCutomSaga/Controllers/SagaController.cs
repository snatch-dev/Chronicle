using System;
using System.Threading.Tasks;
using Chronicle;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EFCoreTestApp.Commands;
using EFCoreTestApp.Events;
using EFCoreTestApp.Persistence;
using EFCoreTestApp.DTO;

namespace EFCoreTestApp.Controllers
{
    /*
        In PRODUCTION environment, this whole controller should be protected and only authorized users should
        be able to access it.
     */
    [ApiController]
    [Route("api/[controller]")]
    public class SagaController : ControllerBase
    {
        private readonly ISagaStateDBRepository _sagaStateDBRepository;
        private readonly ISagaLogRepository _sagaLogRepository;

        public SagaController(ISagaStateDBRepository sagaStateDBRepository, ISagaLogRepository sagaLogRepository)
        {
            _sagaStateDBRepository = sagaStateDBRepository;
            _sagaLogRepository = sagaLogRepository;
        }

        [HttpGet, Route("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSagaById([FromRoute] Guid id)
        {
            // [FromRoute] Guid id
            var sagaState = await _sagaStateDBRepository.GetByIdAsync((SagaId)id.ToString());
            return Ok(sagaState);
        }

        [HttpGet, Route("{id:guid}/logs")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSagaLogsById([FromRoute] Guid id)
        {
            // [FromRoute] Guid id
            var sagaState = await _sagaLogRepository.ReadByIdAsync((SagaId)id.ToString());
            return Ok(sagaState);
        }

    }
}
