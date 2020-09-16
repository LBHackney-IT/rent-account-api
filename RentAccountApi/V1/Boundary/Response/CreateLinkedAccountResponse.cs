using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Boundary.Response
{
    public class CreateLinkedAccountResponse
    {
        /// <example>
        /// b3aa0b17-69c7-ea11-a218-000d3a7fedeb
        /// </example>
        public string LinkedAccountId { get; set; }
    }
}
