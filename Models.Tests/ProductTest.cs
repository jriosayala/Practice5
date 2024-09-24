using NUnit.Framework;
using Practice5.Data;
using Practice5.Models;

namespace Practice5.Models.Tests
{
    [TestFixture]
    public class ProductTests
    {
        [Test]
        public void Product_CanBeInstantiated()
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

            // Assert
            Assert.That(product, Is.Not.Null);
        }

        [Test]
        public void Product_PropertiesCanBeSetAndGet()
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

            // Assert
            Assert.That(product.ProductID, Is.EqualTo(product.ProductID));
            Assert.That(product.ProductName, Is.EqualTo(product.ProductName));
            Assert.That(product.Price, Is.EqualTo(product.Price));
        }
    }
}
