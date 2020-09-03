using Microsoft.CodeAnalysis.CSharp.Syntax;
using RentAccountApi.V1.Boundary.Response;
using RentAccountApi.V1.Domain;
using RentAccountApi.V1.UseCase.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Factories
{
    public static class CRMFactory
    {
        public static string NormalizePostcode(string postCode)
        {
            postCode = postCode.Replace(" ", "");
            string firstPart = postCode.Substring(0, postCode.Length - 3).ToUpper();
            string secondPart = postCode.Substring(postCode.Length - 3).ToUpper();
            return $"{firstPart} {secondPart}";
        }

        public static RentAccountResponse ToResponse(string paymentReference, CrmRentAccountResponse crmResponse, bool privacy)
        {
            var firstname = privacy ? PrivacyFormatting.GetPrivacyString(crmResponse.value[0].contact1_x002e_firstname) : crmResponse.value[0].contact1_x002e_firstname;
            var lastname = privacy ? PrivacyFormatting.GetPrivacyString(crmResponse.value[0].contact1_x002e_lastname) : crmResponse.value[0].contact1_x002e_lastname;
            var fullname = $"{firstname} {lastname}";
            var currentBalance = decimal.Parse(crmResponse.value[0].housing_cur_bal);
            var toPay = decimal.Parse(crmResponse.value[0].housing_rent) - decimal.Parse(crmResponse.value[0].housing_anticipated);
            var rentAccountResponse = new RentAccountResponse
            {
                AccountNumber = paymentReference,
                Name = fullname,
                CurrentBalance = currentBalance > 0 ? currentBalance : currentBalance * -1,
                Rent = decimal.Parse(crmResponse.value[0].housing_rent),
                ToPay = toPay < 0 ? 0 : toPay,
                Benefits = decimal.Parse(crmResponse.value[0].housing_anticipated),
                HasArrears = decimal.Parse(crmResponse.value[0].housing_cur_bal) > 0,
                IsHackneyResponsible = bool.Parse(crmResponse.value[0].contact1_x002e_hackney_responsible),
                NextPayment = GetNextMondayFormatted(),
                Postcode = crmResponse.value[0].contact1_x002e_address1_postalcode,
                TenancyAgreementId = crmResponse.value[0].housing_tag_ref
            };
            return rentAccountResponse;
        }

        public static string GetNextMondayFormatted()
        {
            DateTime nextMonday = GetNextMonday();
            string dayOfWeek = nextMonday.DayOfWeek.ToString();
            string dayOfWeekNumber = nextMonday.Day.ToString();
            string monthString = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(nextMonday.Month);
            return $"{dayOfWeek} {dayOfWeekNumber} {monthString}";
        }

        public static DateTime GetNextMonday()
        {
            DateTime today = DateTime.Today;
            int daysUntilTuesday = ((int) DayOfWeek.Monday - (int) today.DayOfWeek + 7) % 7;
            DateTime nextTuesday = today.AddDays(daysUntilTuesday);
            return nextTuesday;
        }
    }
}
