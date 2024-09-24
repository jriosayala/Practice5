using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;
using Practice5.Models;
using System;
using System.Threading.Tasks;

namespace Practice5.Data.Tests
{
    [TestFixture]
    public class AppDbContextTests
    {
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AppDbContext_CanBeInstantiated()
        {
            // Assert
            Assert.That(_context, Is.Not.Null);
        }

        [Test]
        public void AppDbContext_HasDbSetProperties()
        {
            // Assert
            Assert.That(_context.Product, Is.InstanceOf<DbSet<Product>>());
            Assert.That(_context.Sale, Is.InstanceOf<DbSet<Sale>>());
            Assert.That(_context.Purchase, Is.InstanceOf<DbSet<Purchase>>());
            Assert.That(_context.Inventories, Is.InstanceOf<DbSet<Inventory>>());
        }

        [Test]
        public async Task AppDbContext_CanAddAndRetrieveProduct()
        {
            // Arrange
            var product = new Product
            {
                ProductID = 1,
                ProductName = "Test Product",
                Description = "Test Description",
                Price = 50.0m
            };

            // Act
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
            var retrievedProduct = await _context.Product.FindAsync(product.ProductID);

            // Assert
            Assert.That(retrievedProduct, Is.Not.Null);
            Assert.That(retrievedProduct.ProductName, Is.EqualTo(product.ProductName));
            Assert.That(retrievedProduct.Price, Is.EqualTo(product.Price));
        }

        [Test]
        public async Task AppDbContext_CanAddAndRetrieveSale()
        {
            // Arrange
            var product = new Product
            {
                ProductID = 1,
                ProductName = "Test Product",
                Description = "Test Description",
                Price = 50.0m
            };
            var sale = new Sale
            {
                SaleID = 1,
                ProductID = 1,
                QuantitySold = 10,
                SaleDate = DateTime.Now,
                Product = product
            };

            // Act
            _context.Sale.Add(sale);
            await _context.SaveChangesAsync();
            var retrievedSale = await _context.Sale.FindAsync(sale.SaleID);

            // Assert
            Assert.That(retrievedSale, Is.Not.Null);
            Assert.That(retrievedSale.ProductID, Is.EqualTo(sale.ProductID));
            Assert.That(retrievedSale.QuantitySold, Is.EqualTo(sale.QuantitySold));
            Assert.That(retrievedSale.SaleDate, Is.EqualTo(sale.SaleDate));
        }

        [Test]
        public async Task AppDbContext_CanAddAndRetrievePurchase()
        {
            // Arrange
            var purchase = new Purchase
            {
                PurchaseID = 1,
                ProductID = 1,
                QuantityPurchased = 20,
                PurchaseDate = DateTime.Now
            };

            // Act
            _context.Purchase.Add(purchase);
            await _context.SaveChangesAsync();
            var retrievedPurchase = await _context.Purchase.FindAsync(purchase.PurchaseID);

            // Assert
            Assert.That(retrievedPurchase, Is.Not.Null);
            Assert.That(retrievedPurchase.ProductID, Is.EqualTo(purchase.ProductID));
            Assert.That(retrievedPurchase.QuantityPurchased, Is.EqualTo(purchase.QuantityPurchased));
            Assert.That(retrievedPurchase.PurchaseDate, Is.EqualTo(purchase.PurchaseDate));
        }

        [Test]
        public async Task AppDbContext_CanAddAndRetrieveInventory()
        {
            // Arrange
            var inventory = new Inventory
            {
                InventoryID = 1,
                ProductID = 1,
                Quantity = 100,
                LastUpdated = DateTime.Now
            };

            // Act
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
            var retrievedInventory = await _context.Inventories.FindAsync(inventory.InventoryID);

            // Assert
            Assert.That(retrievedInventory, Is.Not.Null);
            Assert.That(retrievedInventory.ProductID, Is.EqualTo(inventory.ProductID));
            Assert.That(retrievedInventory.Quantity, Is.EqualTo(inventory.Quantity));
            Assert.That(retrievedInventory.LastUpdated, Is.EqualTo(inventory.LastUpdated));
        }
    }
}
