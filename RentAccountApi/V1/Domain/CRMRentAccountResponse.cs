using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Domain
{
    public class CrmRentAccountResponse
    {
        public List<CRMRentAccount> value { get; set; }
    }

    public class CRMRentAccount
    {
        public string housing_prop_ref { get; set; }
        public string housing_house_ref { get; set; }
        public string housing_rent { get; set; }
        public string housing_directdebit { get; set; }
        public string housing_tag_ref { get; set; }
        public string housing_cur_bal { get; set; }
        public string housing_anticipated { get; set; }
        public string contact1_x002e_hackney_title { get; set; }
        public string contact1_x002e_hackney_responsible { get; set; }
        public string contact1_x002e_lastname { get; set; }
        public string contact1_x002e_firstname { get; set; }
        public string contact1_x002e_address1_postalcode { get; set; }
    }
}
