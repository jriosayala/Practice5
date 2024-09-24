using NUnit.Framework;
using Practice5.Data;
using System;

namespace Practice5.Models.Tests
{
    [TestFixture]
    public class PurchaseTests
    {
        [Test]
        public void Purchase_CanBeInstantiated()
        {
            // Arrange & Act
            var purchase = new Purchase();

            // Assert
            Assert.That(purchase, Is.Not.Null);
        }

        [Test]
        public void Purchase_PropertiesCanBeSetAndGet()
        {
            // Arrange && Act
            var purchase = new Purchase {
                PurchaseID = 1,
                ProductID = 2,
                QuantityPurchased = 3,
                PurchaseDate = DateTime.Now
            };

            // Assert
            Assert.That(purchase.PurchaseID, Is.EqualTo(purchase.PurchaseID));
            Assert.That(purchase.ProductID, Is.EqualTo(purchase.ProductID));
            Assert.That(purchase.QuantityPurchased, Is.EqualTo(purchase.QuantityPurchased));
            Assert.That(purchase.PurchaseDate, Is.EqualTo(purchase.PurchaseDate));
        }
    }
}
