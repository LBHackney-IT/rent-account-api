using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.Factories
{
    public static class CRMFactory
    {
        public static string NormalizePostcode(string postCode)
        {
            postCode = postCode.Replace(" ", "");
            string firstPart = postCode.Substring(0, postCode.Length - 3).ToUpper();
            string secondPart = postCode.Substring(postCode.Length - 3).ToUpper();
            return $"{firstPart} {secondPart}";
        }
    }
}
