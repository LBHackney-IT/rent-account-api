using System.Collections.Generic;
using RentAccountApi.V1.Domain;

namespace RentAccountApi.V1.Gateways
{
    public interface IExampleGateway
    {
        Entity GetEntityById(int id);

        List<Entity> GetAll();
    }
}
