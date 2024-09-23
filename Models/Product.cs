using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Practice5.Models
{
    public class Product
    {
        public int ProductID { get; set; }
        [Required(ErrorMessage = "The ProductName field is required.")]
        public required string ProductName { get; set; }
        [Required(ErrorMessage = "The Description field is required.")]
        public required string Description { get; set; }

        [Range(0, 1000, ErrorMessage = "The field Price must be between 0 and 1000.")]
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public DateTime ModifiedDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = new byte[8];
    }
}