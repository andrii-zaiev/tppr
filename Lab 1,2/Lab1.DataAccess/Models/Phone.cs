using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab1.DataAccess.Models
{
    public class Phone
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [InverseProperty(nameof(PhoneParameterValue.Phone))]
        public virtual List<PhoneParameterValue> PhoneParameterValues { get; set; }
    }
}
