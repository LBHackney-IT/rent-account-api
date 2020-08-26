using RentAccountApi.V1.Domain;
using System.Collections.Generic;

namespace RentAccountApi.V1.Boundary.Response
{
    public class AuditResponse
    {
        /// <example>
        /// bob.hoskins@film.com
        /// </example>
        public string User { get; set; }
        /// <example>
        /// 1234567
        /// </example>
        public string RentAccountNumber { get; set; }
        /// <example>
        /// 2020-08-24T15:58:57.8571170
        /// </example>
        public string TimeStamp { get; set; }
        /// <example>
        /// true
        /// </example>
        public bool CSSOLogin { get; set; }
        /// <example>
        /// view
        /// </example>
        public string AuditAction { get; set; }
    }
}
