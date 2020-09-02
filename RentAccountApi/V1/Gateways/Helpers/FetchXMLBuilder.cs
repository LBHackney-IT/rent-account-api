using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Gateways.Helpers
{
    public static class FetchXMLBuilder
    {
        public static string BuildGetTenancyRefFetchXML(string paymentReference, string postcode)
        {
            var fetchXML = $@"<fetch top='1'>
            <entity name = 'account'> 
             <attribute name = 'housing_u_saff_rentacc'/>  
              <filter type = 'and'>   
                 <condition attribute = 'housing_u_saff_rentacc' operator= 'eq' value = '{paymentReference}'/>       
                   </filter>       
                   <link-entity name = 'contact' from = 'parentcustomerid' to = 'accountid' link-type = 'inner'>                
                    <attribute name = 'address1_postalcode'/>                 
                    <attribute name = 'address1_line1'/>                  
                    <attribute name = 'address1_line2'/>                   
                    <attribute name = 'address1_line3'/>                    
                        <filter type = 'and'>                     
                            <condition attribute = 'address1_postalcode' operator= 'eq' value = '{postcode}'/>
                        </filter>
                    </link-entity>
             </entity>
            </fetch>";

            var queryDictionary = new Dictionary<string, string>();

            if (!string.IsNullOrEmpty(fetchXML)) queryDictionary.Add("fetchXml", fetchXML);
            var rqpString = new FormUrlEncodedContent(queryDictionary).ReadAsStringAsync().Result;
            return rqpString; 
        }
    }
}
