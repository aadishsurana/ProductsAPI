using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Models
{
    public class ProductOption
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
