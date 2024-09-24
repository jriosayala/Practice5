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
    public class EditInventoryTests
    {
        private AppDbContext _context;
        private EditModel _editModel;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _context = new AppDbContext(options);
            _editModel = new EditModel(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task OnPostAsync_InventoryIsUpdated_ReturnsRedirectToPageResult()
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

            // Retrieve the existing inventory from the context
            var inventoryToUpdate = await _context.Inventories.FindAsync(testInventory.InventoryID);

            _editModel.Inventory = new Inventory
            {
                InventoryID = inventoryToUpdate.InventoryID,
                ProductID = inventoryToUpdate.ProductID,
                Quantity = 200, // Update Quantity
                LastUpdated = DateTime.Now.AddDays(1) // Update LastUpdated
            };

            // Act
            var result = await _editModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var updatedInventory = await _context.Inventories.FindAsync(testInventory.InventoryID);
            Assert.That(updatedInventory, Is.Not.Null);
            Assert.That(updatedInventory.Quantity, Is.EqualTo(_editModel.Inventory.Quantity));
            Assert.That(updatedInventory.LastUpdated, Is.EqualTo(_editModel.Inventory.LastUpdated));
        }

        [Test]
        public async Task OnPostAsync_InvalidModelState_ReturnsPageResult()
        {
            // Arrange
            _editModel.ModelState.AddModelError("ProductID", "ProductID is required.");

            // Act
            var result = await _editModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_editModel.ModelState.IsValid, Is.False);
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
            var result = await _editModel.OnGetAsync(testInventory.InventoryID);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_editModel.Inventory, Is.Not.Null);
            Assert.That(_editModel.Inventory.InventoryID, Is.EqualTo(testInventory.InventoryID));
        }

        [Test]
        public async Task OnGetAsync_InventoryDoesNotExist_ReturnsNotFoundResult()
        {
            // Act
            var result = await _editModel.OnGetAsync(999); // Non-existent InventoryID

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
