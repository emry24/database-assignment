using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class Product
{
    public string ArticleNumber { get; set; } = null!;

    public int CategoryId { get; set; }

    public int ManufactureId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Manufacture Manufacture { get; set; } = null!;

    public virtual ProductInformation? ProductInformation { get; set; }

    public virtual ProductPrice? ProductPrice { get; set; }
}
