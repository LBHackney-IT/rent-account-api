using System.ComponentModel.DataAnnotations;

namespace RentAccountApi.V1.Boundary.Request
{
    public class CreateAdminAuditRequest
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
        /// <example>
        /// unlink
        /// </example>
        [Required]
        public string AuditAction { get; set; }
    }
}
