using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab1.DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lab1.Models
{
    public class PhoneModel
    {
        public Phone Phone { get; set; }

        public List<PhoneParameterValue> PhoneParameterValues { get; set; }
    }
}
