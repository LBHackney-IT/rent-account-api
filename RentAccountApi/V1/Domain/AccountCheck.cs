using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Domain
{
    public class CrmResponse
    {
        public List<CrmAccount> value { get; set; }
    }

    public class CrmAccount
    {
        public string housing_u_saff_rentacc { get; set; }
        public string accountid { get; set; }
        public string contact1_x002e_address1_postalcode { get; set; }
        public string contact1_x002e_address1_line1 { get; set; }
        public string contact1_x002e_address1_line3 { get; set; }
    }
}
