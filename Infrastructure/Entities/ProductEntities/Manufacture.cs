using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class Manufacture
{
    public int Id { get; set; }

    public string ManufactureName { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
