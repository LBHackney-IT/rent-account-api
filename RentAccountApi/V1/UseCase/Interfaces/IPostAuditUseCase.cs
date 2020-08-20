using RentAccountApi.V1.Boundary.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.UseCase.Interfaces
{
    public interface IPostAuditUseCase
    {
        void Execute(AuditRequestObject auditRequest);
    }
}