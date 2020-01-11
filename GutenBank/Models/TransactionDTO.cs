using System;
using System.ComponentModel.DataAnnotations;

namespace GutenBank.Models
{
    public class TransactionDTO
    {
        [Required]
        public int AccountNumber { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }
    }
}
