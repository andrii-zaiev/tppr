using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab1.DataAccess.Models
{
    public class ParameterValue
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(Parameter))]
        [DisplayName("Parameter")]
        public Guid ParameterId { get; set; }

        public double Value { get; set; }

        [MaxLength(100)]
        [DisplayName("Value Text")]
        public string ValueText { get; set; }

        public virtual Parameter Parameter { get; set; }
    }
}
