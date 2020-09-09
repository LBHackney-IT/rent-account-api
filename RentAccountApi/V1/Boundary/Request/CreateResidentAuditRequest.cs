using System.ComponentModel.DataAnnotations;

namespace RentAccountApi.V1.Boundary.Request
{
    public class CreateResidentAuditRequest
    {
        /// <example>
        /// 12345678
        /// </example>
        [Required]
        public string RentAccountNumber { get; set; }
        /// <example>
        /// E8 1DY
        /// </example>
        [Required]
        public string PostCode { get; set; }
        /// <example>
        /// 12345/01
        /// </example>
        [Required]
        public string TenancyReference { get; set; }
    }
}
