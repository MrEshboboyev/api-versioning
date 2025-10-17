public record UpdateProductRequestV3
{
    public string? Name { get; init; }
    public string? DisplayName { get; init; }
    public string? Description { get; init; }
    public decimal? Price { get; init; }
    public string? Currency { get; init; }
    public bool? IsDiscounted { get; init; }
    public decimal? DiscountedPrice { get; init; }
    public bool? InStock { get; init; }
    public int? Quantity { get; init; }
    public string? Category { get; init; }
    public string? Department { get; init; }
    public List<string>? Tags { get; init; }
}
