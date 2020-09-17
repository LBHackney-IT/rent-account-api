using System.ComponentModel.DataAnnotations;

namespace RentAccountApi.V1.Boundary.Request
{
    public class CreateLinkedAccountRequest
    {
        /// <example>
        /// 12345678
        /// </example>
        [Required]
        public string AccountNumber { get; set; }
        /// <example>
        /// 123456bbdc9f04c50fd5f96208569736bbdfe44d6401614480542d7d42a2d123
        /// </example>
        [Required]
        public string CssoId { get; set; }
    }
}
