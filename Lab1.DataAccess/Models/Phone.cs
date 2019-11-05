using System;
using System.ComponentModel.DataAnnotations;

namespace Lab1.DataAccess.Models
{
    public class Phone
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }
    }
}
