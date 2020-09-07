using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Domain
{
    public class CrmLinkedAccountResponse
    {
        public List<CRMLinkedAccount> value { get; set; }
    }

    public class CRMLinkedAccount
    {
        public string hackney_csso_linked_rent_accountid { get; set; }
        public string csso_id { get; set; }
        public string rent_account_number { get; set; }
    }
}
