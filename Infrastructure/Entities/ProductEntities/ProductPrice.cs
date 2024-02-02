using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class ProductPrice
{
    public string ArticleNumber { get; set; } = null!;

    public decimal Price { get; set; }

    public virtual Product ArticleNumberNavigation { get; set; } = null!;
}
