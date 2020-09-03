using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Gateways
{
    public interface ICRMTokenGateway
    {
        Task<string> GetCRMToken();
    }
}
