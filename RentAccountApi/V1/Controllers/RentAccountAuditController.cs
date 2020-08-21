using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.UseCase.Interfaces;
using RentAccountApi.V1.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace RentAccountApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/rentaccountaudit")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class RentAccountAuditController : BaseController
    {

        private readonly IPostAuditUseCase _postAuditUseCase;

        public RentAccountAuditController(IPostAuditUseCase postAuditUseCase)
        {
            _postAuditUseCase = postAuditUseCase;
        }

        /// <summary>
        /// Records an audit log for users accessing rent accounts
        /// </summary>
        /// <response code="204">Audit Log successfully generated</response>
        /// <response code="400">One or more request parameters are invalid or missing</response>
        /// <response code="500">There was a problem recording an audit.</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [HttpPost]
        public IActionResult GenerateToken([FromBody] AuditRequestObject auditRequest)
        {
            try
            {
                _postAuditUseCase.Execute(auditRequest);
                return new NoContentResult();
            }
            catch (AuditNotInsertedException ex)
            {
                return StatusCode(500, string.Format("There was a problem inserting the audit data into the database.{0}", ex.Message));
            }
        }
    }
}
