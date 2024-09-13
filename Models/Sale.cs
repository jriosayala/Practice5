public class Sale
{
    public int SaleID { get; set; }
    public int ProductID { get; set; }
    public DateTime SaleDate { get; set; }
    public int QuantitySold { get; set; }
    public decimal SalePrice { get; set; }
    public Product Product { get; set; }
}