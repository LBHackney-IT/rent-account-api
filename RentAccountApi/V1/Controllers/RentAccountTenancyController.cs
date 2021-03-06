using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class RentAccountTenancyController : BaseController
    {
        private readonly IGetRentBreakdownUseCase _getRentBreakdownUseCase;
        private readonly IGetTransactionsUseCase _getTransactionsUseCase;

        public RentAccountTenancyController(IGetRentBreakdownUseCase getRentBreakdownUseCase, IGetTransactionsUseCase getTransactionsUseCase)
        {
            _getRentBreakdownUseCase = getRentBreakdownUseCase;
            _getTransactionsUseCase = getTransactionsUseCase;
        }

        /// <summary>
        /// Gets tenancy rent break down
        /// </summary>
        /// <response code="200">Rent breakdown returned for tenancy reference</response>
        /// <response code="404">Rent breakdown for provided tenancy reference could not be found</response>
        /// <response code="500">There was a problem retrieving the rent breakdown</response>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<RentBreakdown>), StatusCodes.Status200OK)]
        [Route("rentbreakdown")]
        public async Task<IActionResult> GetRentBreakdown([FromQuery(Name = "tag-ref"), BindRequired] string tagRef)
        {
            try
            {
                var response = await _getRentBreakdownUseCase.Execute(tagRef);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(404, "Rent breakdown not found");
                }
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

        /// <summary>
        /// Gets transactions
        /// </summary>
        /// <response code="200">List of Transactions returned or empty list</response>
        /// <response code="500">There was a problem retrieving the transactions</response>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(TransactionsResponse), StatusCodes.Status200OK)]
        [Route("transactions/payment-ref/{accountNumber}/post-code/{postCode}")]
        public async Task<IActionResult> GetTransactions(string accountNumber, string postCode)
        {
            try
            {
                var response = await _getTransactionsUseCase.Execute(accountNumber, postCode);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {//TODO: returning empty object rather than a 404 here
                    return StatusCode(404, "Transactions not found");
                }
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
