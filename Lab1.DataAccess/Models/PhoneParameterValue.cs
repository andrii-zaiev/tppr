using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab1.DataAccess.Models
{
    public class PhoneParameterValue
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Phone))]
        public Guid PhoneId { get; set; }

        [ForeignKey(nameof(ParameterValue))]
        public Guid ParameterValueId { get; set; }

        public virtual Phone Phone { get; set; }

        public virtual ParameterValue ParameterValue { get; set; }
    }
}
