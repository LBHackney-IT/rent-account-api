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
    public class RentAccountCRMController : BaseController
    {
        private readonly ICheckRentAccountExistsUseCase _checkRentAccountExistsUseCase;
        private readonly IGetRentAccountUseCase _getRentAccountUseCase;
        private readonly IGetLinkedAccountUseCase _getLinkedAccountUseCase;
        private readonly IDeleteLinkedAccountUseCase _deleteLinkedAccountUseCase;
        private readonly ICreateLinkedAccountUseCase _createLinkedAccountUseCase;

        public RentAccountCRMController(ICheckRentAccountExistsUseCase checkRentAccountExistsUseCase, IGetRentAccountUseCase getRentAccountUseCase, IGetLinkedAccountUseCase getLinkedAccountUseCase, IDeleteLinkedAccountUseCase deleteLinkedAccountUseCase, ICreateLinkedAccountUseCase createLinkedAccountUseCase)
        {
            _checkRentAccountExistsUseCase = checkRentAccountExistsUseCase;
            _getRentAccountUseCase = getRentAccountUseCase;
            _getLinkedAccountUseCase = getLinkedAccountUseCase;
            _deleteLinkedAccountUseCase = deleteLinkedAccountUseCase;
            _createLinkedAccountUseCase = createLinkedAccountUseCase;
        }

        /// <summary>
        /// Checks a rent account exists
        /// </summary>
        /// <response code="200">Rent account and payment reference matches</response>
        /// <response code="404">Rent account and payment reference does not match</response>
        /// <response code="500">There was a problem checking the rent account</response>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CheckAccountExistsResponse), StatusCodes.Status200OK)]
        [Route("checkrentaccount/paymentref/{paymentReference}/postcode/{postcode}")]
        public async Task<IActionResult> CheckRentAccountExists(string paymentReference, string postcode)
        {
            try
            {
                var response = await _checkRentAccountExistsUseCase.Execute(paymentReference, postcode);
                if (response.Exists)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(404, "Account not found");
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
        /// Retrieves rent account details
        /// </summary>
        /// <response code="200">Rent account returned</response>
        /// <response code="404">Rent account does not match</response>
        /// <response code="500">There was a problem retrieving the rent account</response>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(RentAccountResponse), StatusCodes.Status200OK)]
        [Route("paymentref/{paymentReference}/privacy/{privacy}")]
        public async Task<IActionResult> GetRentAccount(string paymentReference, bool privacy)
        {
            try
            {
                var response = await _getRentAccountUseCase.Execute(paymentReference, privacy);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(404, "Account not found");
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
        /// Create linked account
        /// </summary>
        /// <response code="200">Linked account created</response>
        /// <response code="500">There was a problem creating the linked account</response>
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CreateLinkedAccountResponse), StatusCodes.Status200OK)]
        [Route("linkedaccount")]
        public async Task<IActionResult> CreateLinkedAccount(CreateLinkedAccountRequest createLinkedAccountRequest)
        {
            try
            {
                var response = await _createLinkedAccountUseCase.Execute(createLinkedAccountRequest);
                if (response != null)
                {
                    return Ok(response);
                }
                else
                {
                    return StatusCode(404, "Linked account could not be created, please check values");
                }
            }
            catch (MissingLinkedAccountRequestParameterException e)
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
        /// Retrieves linked account
        /// </summary>
        /// <response code="200">Linked account returned or empty list returned</response>
        /// <response code="500">There was a problem retrieving the linked account</response>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(List<LinkedAccountResponse>), StatusCodes.Status200OK)]
        [Route("linkedaccount/{cssoId}")]
        public async Task<IActionResult> GetLinkedAccount(string cssoId)
        {
            try
            {
                var response = await _getLinkedAccountUseCase.Execute(cssoId);
                if (response != null)
                {
                    var responseList = new List<LinkedAccountResponse>()
                    {
                        response
                    };
                    return Ok(responseList);
                }
                else
                {
                    return Ok(new List<LinkedAccountResponse>());
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
        /// Deletes linked account
        /// </summary>
        /// <response code="204">Linked account deleted</response>
        /// <response code="404">Linked account does not exist</response>
        /// <response code="500">There was a problem deleting the linked account</response>
        [HttpDelete]
        [ProducesResponseType(typeof(string), StatusCodes.Status204NoContent)]
        [Route("linkedaccount/{cssoId}")]
        public async Task<IActionResult> UnlinkAccount(string cssoId)
        {
            try
            {
                var response = await _deleteLinkedAccountUseCase.Execute(cssoId);
                if (response != null && response.success)
                {
                    return NoContent();
                }
                else
                {
                    return StatusCode(404, "Linked account not found");
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
