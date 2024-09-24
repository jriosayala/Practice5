using NUnit.Framework;
using Practice5.Data;
using Practice5.Models;
using System;


namespace Practice5.Models.Tests
{
    [TestFixture]
    public class SaleTests
    {
        [Test]
        public void Sale_CanBeInstantiated()
        {
            // Arrange & Act
            var product = new Product
            {
                ProductID = 1,
                ProductName = "Test Product",
                Description = "Test Description",
                Price = 10.0m,
                StockQuantity = 100,
                ModifiedDate = DateTime.Now,
                RowVersion = new byte[8] // Initialize with a default value
            };

            var sale = new Sale
            {
                SaleID = 1,
                ProductID = 2,
                QuantitySold = 3,
                SaleDate = DateTime.Now,
                Product = product
            };

            // Assert
            Assert.That(sale, Is.Not.Null);
        }

        [Test]
        public void Sale_PropertiesCanBeSetAndGet()
        {
            // Arrange && Act   
            var product = new Product
            {
                ProductID = 1,
                ProductName = "Test Product",
                Description = "Test Description",
                Price = 10.0m,
                StockQuantity = 100,
                ModifiedDate = DateTime.Now,
                RowVersion = new byte[8] // Initialize with a default value
            };
            var sale = new Sale
            {
                SaleID = 1,
                ProductID = 2,
                QuantitySold = 3,
                SaleDate = DateTime.Now,
                Product = product
            };

            // Assert
            Assert.That(sale.SaleID, Is.EqualTo(sale.SaleID));
            Assert.That(sale.ProductID, Is.EqualTo(sale.ProductID));
            Assert.That(sale.QuantitySold, Is.EqualTo(sale.QuantitySold));
            Assert.That(sale.SaleDate, Is.EqualTo(sale.SaleDate));
        }
    }
}
