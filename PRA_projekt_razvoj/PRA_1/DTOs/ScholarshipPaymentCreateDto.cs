using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    public class ScholarshipPaymentCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public int StudyProgramId { get; set; }

        [Required]
        public string? PaymentPlan { get; set; }

        [Required]
        public int? InstallmentNumber { get; set; }

        [Required]
        public int? TotalInstallments { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
