using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RentAccountApi.V1.Boundary.Request;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.UseCase.Interfaces;

namespace RentAccountApi.V1.Controllers
{
    [Route("api/v1/rentaccount")]
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    public class RentAccountCRMController : BaseController
    {
        private readonly ICheckRentAccountExistsUseCase _checkRentAccountExistsUseCase;

        public RentAccountCRMController(ICheckRentAccountExistsUseCase checkRentAccountExistsUseCase)
        {
            _checkRentAccountExistsUseCase = checkRentAccountExistsUseCase;
        }

        /// <summary>
        /// Checks a rent account exists
        /// </summary>
        /// <response code="200">Rent account exists</response>
        /// <response code="404">Rent account does not exist</response>
        /// <response code="500">There was a problem retrieving the rent account</response>
        [HttpGet]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(typeof(CheckAccountExistsResponse), StatusCodes.Status200OK)]
        [Route("/paymentref/{paymentReference}/postcode/{postcode}")]
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
                return BadRequest(e.Message);
            }
        }

        /*GetAccount GET - IN paymentref & postcode - OUT boolean and tag_ref
        GetAccountDetails GET - IN paymentref AND privacy (deals with hiding values if not logged in)- OUT
                accountNumber,
                name,
                currentBalance: parseFloat(
                  currentBalance > 0 ? currentBalance : currentBalance * -1
                ).toFixed(2),
                rent: data.value[0].housing_rent,
                toPay: (toPay < 0 ? 0 : toPay).toFixed(2),
                benefits: parseFloat(data.value[0].housing_anticipated).toFixed(2),
                hasArrears: data.value[0].housing_cur_bal > 0,
                isHackneyResponsible: data.value[0].contact1_x002e_hackney_responsible,
                nextPayment: getNextMonday(),
                postcode: data.value[0].contact1_x002e_address1_postalcode,
                tenancyAgreementId: data.value[0].housing_tag_ref,

        #####
        maybe move to a different controller?
        GetLinkedAccount GET - IN CSSOId - OUT -
                "hackney_csso_linked_rent_accountid": "7defda2c-c96b-ea11-a811-000d3a86b410",
                "csso_id": "1be7fd002a5764168846de6b9e0bcbe48f7f483c90e18a59d2c68c2d84fc4ace",
                "rent_account_number": "9138123314"
        LinkAccount POST
        UnlinkAccount DELETE*/


    }
}
