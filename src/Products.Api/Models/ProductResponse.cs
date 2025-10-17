namespace Products.Api.Models;

// V1 Response - Simple product information
public sealed record ProductResponseV1
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
}

// V2 Response - Enhanced product information with structured data
public sealed record ProductResponseV2
{
    public Guid Id { get; init; }
    public ProductInfoV2 Product { get; init; } = new ProductInfoV2();
    public InventoryInfoV2 Inventory { get; init; } = new InventoryInfoV2();
}

public sealed record ProductInfoV2
{
    public string Name { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public PricingInfoV2 Pricing { get; init; } = new PricingInfoV2();
}

public sealed record PricingInfoV2
{
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public bool Discounted { get; init; }
}

public sealed record InventoryInfoV2
{
    public bool InStock { get; init; }
    public int Quantity { get; init; }
}

// V3 Response - Advanced product information with analytics and categorization
public sealed record ProductResponseV3
{
    public Guid Id { get; init; }
    public ProductInfoV3 Product { get; init; } = new ProductInfoV3();
    public InventoryInfoV3 Inventory { get; init; } = new InventoryInfoV3();
    public AnalyticsInfoV3 Analytics { get; init; } = new AnalyticsInfoV3();
    public CategoryInfoV3 Category { get; init; } = new CategoryInfoV3();
}

public sealed record ProductInfoV3
{
    public string Name { get; init; } = string.Empty;
    public string DisplayName { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<string> Tags { get; init; } = new List<string>();
    public PricingInfoV3 Pricing { get; init; } = new PricingInfoV3();
    public List<VariantInfoV3> Variants { get; init; } = new List<VariantInfoV3>();
}

public sealed record PricingInfoV3
{
    public decimal Amount { get; init; }
    public string Currency { get; init; } = string.Empty;
    public bool Discounted { get; init; }
    public decimal? DiscountedAmount { get; init; }
    public DateTime? DiscountEndDate { get; init; }
    public List<PriceHistoryEntryV3> PriceHistory { get; init; } = new List<PriceHistoryEntryV3>();
}

public sealed record PriceHistoryEntryV3
{
    public DateTime Date { get; init; }
    public decimal Price { get; init; }
}

public sealed record InventoryInfoV3
{
    public bool InStock { get; init; }
    public int Quantity { get; init; }
    public int ReservedQuantity { get; init; }
    public WarehouseInfoV3 Warehouse { get; init; } = new WarehouseInfoV3();
    public List<InventoryHistoryEntryV3> InventoryHistory { get; init; } = new List<InventoryHistoryEntryV3>();
}

public sealed record WarehouseInfoV3
{
    public string Location { get; init; } = string.Empty;
    public string Code { get; init; } = string.Empty;
}

public sealed record InventoryHistoryEntryV3
{
    public DateTime Date { get; init; }
    public int Quantity { get; init; }
    public string Reason { get; init; } = string.Empty;
}

public sealed record AnalyticsInfoV3
{
    public int Views { get; init; }
    public int Purchases { get; init; }
    public double Rating { get; init; }
    public int ReviewsCount { get; init; }
    public List<ReviewInfoV3> TopReviews { get; init; } = new List<ReviewInfoV3>();
}

public sealed record ReviewInfoV3
{
    public string Reviewer { get; init; } = string.Empty;
    public int Rating { get; init; }
    public string Comment { get; init; } = string.Empty;
    public DateTime Date { get; init; }
}

public sealed record CategoryInfoV3
{
    public string PrimaryCategory { get; init; } = string.Empty;
    public List<string> SubCategories { get; init; } = new List<string>();
    public string Department { get; init; } = string.Empty;
}

public sealed record VariantInfoV3
{
    public string Name { get; init; } = string.Empty;
    public string Sku { get; init; } = string.Empty;
    public Dictionary<string, string> Attributes { get; init; } = new Dictionary<string, string>();
    public PricingInfoV3 Pricing { get; init; } = new PricingInfoV3();
    public InventoryInfoV3 Inventory { get; init; } = new InventoryInfoV3();
}
