using NPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RefactorThis.Repositories.Entities
{
    [TableName("ProductOptions")]
    [PrimaryKey("Id", AutoIncrement = false)]
    public class ProductOptionEntity
    {
        public string Id { get; set; }

        public string ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}
