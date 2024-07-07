using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlhamraMall.Domains.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<Product> Products { get; set; }
        public bool IsDeleted { get; set; } = false;
        [JsonIgnore]
        public List<CommercialStore> CommercialStores { get; set; }






        public Category()
        {
             Id = Guid.NewGuid();
            Products = new List<Product>();
            CommercialStores = new List<CommercialStore>();
        }
    }
}
