using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.UseCase.Interfaces;
using RentAccountApi.V1.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using RentAccountApi.V1.Boundary.Response;
using System;
using System.Threading.Tasks;
using Amazon.Lambda.Core;

namespace RentAccountApi.V1.Controllers
{
    [ApiController]
    [Route("api/v1/rentaccountaudit")]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class RentAccountAuditController : BaseController
    {

        private readonly IPostAuditUseCase _postAuditUseCase;
        private readonly IGetAuditByUserUseCase _getAuditByUserUseCase;

        public RentAccountAuditController(IPostAuditUseCase postAuditUseCase, IGetAuditByUserUseCase getAuditByUserUseCase)
        {
            _postAuditUseCase = postAuditUseCase;
            _getAuditByUserUseCase = getAuditByUserUseCase;
        }

        /// <summary>
        /// Records an audit log for admin users accessing rent accounts
        /// </summary>
        /// <response code="204">Audit Log successfully generated</response>
        /// <response code="400">One or more request parameters are invalid or missing</response>
        /// <response code="500">There was a problem recording an audit.</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [HttpPost]
        [Route("admin")]
        public IActionResult GenerateAdminAuditLog([FromBody] CreateAdminAuditRequest auditRequest)
        {
            try
            {
                _postAuditUseCase.CreateAdminAudit(auditRequest);
                return new NoContentResult();
            }
            catch (AuditNotInsertedException ex)
            {
                return StatusCode(500, string.Format("There was a problem inserting the audit data into the database. {0}", ex.Message));
            }
        }

        /// <summary>
        /// Records an audit log for residents accessing rent accounts
        /// </summary>
        /// <response code="204">Audit Log successfully generated</response>
        /// <response code="400">One or more request parameters are invalid or missing</response>
        /// <response code="500">There was a problem recording an audit.</response>
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(NoContentResult), StatusCodes.Status204NoContent)]
        [HttpPost]
        [Route("resident")]
        public async Task<IActionResult> GenerateResidentAuditLog([FromBody] CreateResidentAuditRequest auditRequest)
        {
            try
            {
                var response = await _postAuditUseCase.CreateResidentAudit(auditRequest);
                if (response != null && response.success)
                {
                    return NoContent();
                }
                else
                {
                    throw new AuditNotInsertedException();
                }
            }
            catch (MissingQueryParameterException e)
            {
                LambdaLogger.Log(e.Message);
                return BadRequest(e.Message);
            }
            catch (AuditNotInsertedException ex)
            {
                return StatusCode(500, string.Format("There was a problem inserting the audit data into the database. {0}", ex.Message));
            }
        }

        /// <summary>
        /// Returns a list of audit entries for a given user email address
        /// </summary>
        /// <response code="200">...</response>
        /// <response code="400">One or more request parameters are invalid or missing</response>
        [ProducesResponseType(typeof(GetAllAuditsResponse), StatusCodes.Status200OK)]
        [HttpGet]
        [Route("admin")]
        public async Task<IActionResult> GetAuditByUser([FromQuery] string userEmail)
        {
            try
            {
                if (string.IsNullOrEmpty(userEmail))
                {
                    throw new MissingQueryParameterException("Parameter useremail must be provided.");
                }
                var response = await _getAuditByUserUseCase.GetAuditByUser(userEmail);
                return Ok(response);
            }
            catch (MissingQueryParameterException ex)
            {
                return StatusCode(400, ex.Message);
            }
            catch (Exception ex)
            {
                LambdaLogger.Log(ex.Message);
                return StatusCode(500, "There was a problem retrieving the audit data from the database.");
            }
        }

    }
}
