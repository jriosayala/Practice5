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
    public class CreateInventoryTests
    {
        private AppDbContext _context;
        private CreateModel _createModel;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);
            _createModel = new CreateModel(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task OnPostAsync_InventoryIsCreated_ReturnsRedirectToPageResult()
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

            _createModel.Inventory = new Inventory
            {
                InventoryID = 1,
                ProductID = testProduct.ProductID,
                Quantity = 100,
                LastUpdated = DateTime.Now
            };

            // Act
            var result = await _createModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var createdInventory = await _context.Inventories.FindAsync(_createModel.Inventory.InventoryID);
            Assert.That(createdInventory, Is.Not.Null);
            Assert.That(createdInventory.ProductID, Is.EqualTo(testProduct.ProductID));
            Assert.That(createdInventory.Quantity, Is.EqualTo(100));
            Assert.That(createdInventory.LastUpdated, Is.EqualTo(_createModel.Inventory.LastUpdated));
        }

        [Test]
        public async Task OnPostAsync_InvalidModelState_ReturnsPageResult()
        {
            // Arrange
            _createModel.ModelState.AddModelError("ProductID", "ProductID is required.");

            // Act
            var result = await _createModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_createModel.ModelState.IsValid, Is.False);
        }

        [Test]
        public void OnGet_ReturnsPageResult()
        {
            // Act
            var result = _createModel.OnGet();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
        }
    }
}
