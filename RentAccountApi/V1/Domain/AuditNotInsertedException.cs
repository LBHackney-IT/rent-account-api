using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Domain
{
    public class AuditNotInsertedException : Exception
    {
        public AuditNotInsertedException()
        {

        }

        public AuditNotInsertedException(string message) : base(message)
        {

        }
    }
}
