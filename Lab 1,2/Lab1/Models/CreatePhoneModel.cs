using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab1.Models
{
    public class CreatePhoneModel
    {
        public string Name { get; set; }

        public List<Guid> ParameterValueIds { get; set; }
    }
}
