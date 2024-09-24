using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;
using Practice5.Pages_Inventories;
using System;
using System.Threading.Tasks;
using Practice5.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Practice5.Tests
{
    [TestFixture]
    public class DeleteInventoryTests
    {
        private AppDbContext _context;
        private DeleteModel _deleteModel;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);
            _deleteModel = new DeleteModel(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task OnPostAsync_InventoryIsDeleted_ReturnsRedirectToPageResult()
        {
            // Arrange
            var testProduct = new Product
            {
                ProductID = 1,
                ProductName = "Test Product",
                Description = "Test Description",
                Price = 50.0m
            };
            _context.Product.Add(testProduct);
            await _context.SaveChangesAsync();

            var testInventory = new Inventory
            {
                InventoryID = 1,
                ProductID = testProduct.ProductID,
                Quantity = 100,
                LastUpdated = DateTime.Now
            };
            _context.Inventories.Add(testInventory);
            await _context.SaveChangesAsync();

            // Act
            var result = await _deleteModel.OnPostAsync(testInventory.InventoryID);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var deletedInventory = await _context.Inventories.FindAsync(testInventory.InventoryID);
            Assert.That(deletedInventory, Is.Null);
        }

        [Test]
        public async Task OnPostAsync_InventoryDoesNotExist_ReturnsRedirectToPageResult()
        {
            // Act
            var result = await _deleteModel.OnPostAsync(999); // Non-existent InventoryID

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
        }

        [Test]
        public async Task OnGetAsync_InventoryExists_ReturnsPageResult()
        {
            // Arrange
            var testProduct = new Product
            {
                ProductID = 1,
                ProductName = "Test Product",
                Description = "Test Description",
                Price = 50.0m
            };
            _context.Product.Add(testProduct);
            await _context.SaveChangesAsync();

            var testInventory = new Inventory
            {
                InventoryID = 1,
                ProductID = testProduct.ProductID,
                Quantity = 100,
                LastUpdated = DateTime.Now
            };
            _context.Inventories.Add(testInventory);
            await _context.SaveChangesAsync();

            // Act
            var result = await _deleteModel.OnGetAsync(testInventory.InventoryID);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_deleteModel.Inventory, Is.Not.Null);
            Assert.That(_deleteModel.Inventory.InventoryID, Is.EqualTo(testInventory.InventoryID));
        }

        [Test]
        public async Task OnGetAsync_InventoryDoesNotExist_ReturnsNotFoundResult()
        {
            // Act
            var result = await _deleteModel.OnGetAsync(999); // Non-existent InventoryID

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}