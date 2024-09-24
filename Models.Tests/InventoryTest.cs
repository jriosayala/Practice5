using NUnit.Framework;
using Practice5.Data;
using System;

namespace Practice5.Models.Tests
{
    [TestFixture]
    public class InventoryTests
    {
        [Test]
        public void Inventory_CanBeInstantiated()
        {
            // Arrange & Act
            var inventory = new Inventory();

            // Assert
            Assert.That(inventory, Is.Not.Null);
        }

        [Test]
        public void Inventory_PropertiesCanBeSetAndGet()
        {
            // Arrange
            var inventory = new Inventory();
            var inventoryID = 1;
            var productID = 2;
            var quantity = 100;
            var lastUpdated = DateTime.Now;

            // Act
            inventory.InventoryID = inventoryID;
            inventory.ProductID = productID;
            inventory.Quantity = quantity;
            inventory.LastUpdated = lastUpdated;

            // Assert
            Assert.That(inventory.InventoryID, Is.EqualTo(inventoryID));
            Assert.That(inventory.ProductID, Is.EqualTo(productID));
            Assert.That(inventory.Quantity, Is.EqualTo(quantity));
            Assert.That(inventory.LastUpdated, Is.EqualTo(lastUpdated));
        }
    }
}
