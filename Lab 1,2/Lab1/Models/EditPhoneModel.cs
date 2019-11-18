using System;
using System.Collections.Generic;

namespace Lab1.Models
{
    public class EditPhoneModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<Guid> ParameterValueIds { get; set; }
    }
}
