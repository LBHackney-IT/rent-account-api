using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Boundary.Response
{
    public class RentAccountResponse
    {
        public string AccountNumber { get; set; }
        public string Name { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal Rent { get; set; }
        public decimal ToPay { get; set; }
        public decimal Benefits { get; set; }
        public bool HasArrears { get; set; }
        public bool IsHackneyResponsible { get; set; }
        public string NextPayment { get; set; }
        public string Postcode { get; set; }
        public string TenancyAgreementId { get; set; }
    }
}
