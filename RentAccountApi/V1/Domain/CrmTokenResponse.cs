using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Domain
{
    public class CrmTokenResponse
    {
        public string accessToken { get; set; }
        public string expiresAt { get; set; }
    }
}
