using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentAccountApi.V1.UseCase.Helpers
{
    public static class PrivacyFormatting
    {
        public static string GetPrivacyString(string textToChange)
        {
            string padding = new string('x', textToChange.Length - 1);
            string returnString = textToChange.Substring(0, 1) + padding;
            return returnString;
        }
    }
}
