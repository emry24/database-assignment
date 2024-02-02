using System;
using System.Collections.Generic;

namespace Infrastructure.Entities.ProductEntities;

public partial class ProductInformation
{
    public string ArticleNumber { get; set; } = null!;

    public string ProductTitle { get; set; } = null!;

    public string? Ingress { get; set; }

    public string? Description { get; set; }

    public string? Specification { get; set; }

    public virtual Product ArticleNumberNavigation { get; set; } = null!;
}
