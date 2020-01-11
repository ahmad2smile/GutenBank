using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GutenBank.Domain
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public TransactionType Type { get; set; }

        public TransactionStatus Status { get; set; }

        public int AccountNumber { get; set; }

        [ForeignKey("AccountNumber")]
        public Account Account { get; set; }
    }
}
    