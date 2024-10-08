using System.ComponentModel.DataAnnotations;

namespace Practice5.Models
{
    public class Purchase
    {
        public int PurchaseID { get; set; }
        public int ProductID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int QuantityPurchased { get; set; }
        public decimal PurchasePrice { get; set; }
    }
}