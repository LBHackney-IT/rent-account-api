using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Domain
{
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException() { }
        public AccountNotFoundException(string message) : base(message)
        { }
    }
}
