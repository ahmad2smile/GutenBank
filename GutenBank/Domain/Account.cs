using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GutenBank.Domain
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountNumber { get; set; }

        [ConcurrencyCheck]
        [Timestamp]
        public byte[] Version { get; set; }

        [Required]
        public int Balance { get; set; }

        [Required]
        public Currency Currency { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
