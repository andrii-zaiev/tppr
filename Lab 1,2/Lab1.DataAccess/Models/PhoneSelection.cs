using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab1.DataAccess.Models
{
    public class PhoneSelection
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(Phone))]
        public Guid PhoneId { get; set; }

        public virtual User User { get; set; }

        public virtual Phone Phone { get; set; }
    }
}
