using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Models
{
    public class CreateUpdateOptionRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
