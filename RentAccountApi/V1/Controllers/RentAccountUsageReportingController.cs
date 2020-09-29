using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.UseCase.Interfaces;

namespace RentAccountApi.V1.Controllers
{
    [Route("api/v1/rentaccounts")]
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class RentAccountUsageReportingController : BaseController
    {
        private readonly IUsageReportingsUseCase _usageReportingUseCase;
       
        public RentAccountUsageReportingController(IUsageReportingsUseCase usageReportingUseCase)
        {
            _usageReportingUseCase = usageReportingUseCase;
        }

        /// <summary>
        /// Run CRM reports
        /// </summary>
        /// <response code="200">Returned report response</response>
        /// <response code="500">There was a problem producing the report</response>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(UsageReportResponse), StatusCodes.Status200OK)]
        [Route("report")]
        public async Task<IActionResult> RunCRMReport([FromQuery] UsageReportRequest usageReportRequest)
        {
            try
            {
                var response = await _usageReportingUseCase.Execute(usageReportRequest);
                return Ok(response);                
            }
            catch (UsageReportRequestException ex)
            {
                LambdaLogger.Log(ex.Message);
                return BadRequest(ex.Message);
            }
            catch (MissingQueryParameterException e)
            {
                LambdaLogger.Log(e.Message);
                return BadRequest(e.Message);
            }
            catch (Exception ex)
            {
                LambdaLogger.Log(ex.Message);
                return StatusCode(500, "An error has occured");
            }
        }
    }
}
