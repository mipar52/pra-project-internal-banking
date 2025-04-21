using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRA_1.Models;
using System.ComponentModel.DataAnnotations;

namespace PRA_1.DTOs
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScholarshipPaymentCreateDto : ControllerBase
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
