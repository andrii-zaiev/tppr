using System;
using System.ComponentModel.DataAnnotations;

namespace Lab1.DataAccess.Models
{
    public class Parameter
    {
        [Key]
        public Guid Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public CriterionType Type { get; set; }

        public Optimality Optimality { get; set; }

        [MaxLength(20)]
        public string Unit { get; set; }

        public Scale Scale { get; set; }
    }
}
