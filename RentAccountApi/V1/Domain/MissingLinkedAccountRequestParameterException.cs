using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Domain
{
    public class MissingLinkedAccountRequestParameterException : Exception
    {
        public MissingLinkedAccountRequestParameterException(string message)
            : base(message)
        { }
    }
}
