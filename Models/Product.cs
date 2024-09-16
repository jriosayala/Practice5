public class Product
{
    public int ProductID { get; set; }
    public required string ProductName { get; set; }
    public required string Description { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public DateTime ModifiedDate { get; set; }
}