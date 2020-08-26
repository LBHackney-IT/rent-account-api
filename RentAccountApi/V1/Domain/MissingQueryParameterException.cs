using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Domain
{
    public class MissingQueryParameterException : Exception
    {
        public MissingQueryParameterException(string message)
            : base(message)
        { }
    }
}
