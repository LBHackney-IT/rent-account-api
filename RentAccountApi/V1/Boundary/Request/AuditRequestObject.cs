using System.ComponentModel.DataAnnotations;

namespace RentAccountApi.V1.Boundary.Request
{
    public class AuditRequestObject
    {
        /// <example>
        /// john.smith@test.com
        /// </example>
        [Required]
        public string User { get; set; }
        /// <example>
        /// 12345678
        /// </example>
        [Required]
        public string RentAccountNumber { get; set; }
        /// <example>
        /// true
        /// </example>
        [Required]
        public bool CSSOLogin { get; set; }
    }
}
