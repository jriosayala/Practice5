using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;
using Practice5.Pages_Products;
using System.Threading.Tasks;
using Practice5.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Practice5.Web.Tests
{
    [TestFixture]
    public class EditProductTests
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
        public async Task OnPostAsync_ProductNotUpdated_ReturnsPageWithModelError()
        {
            // Arrange
            var testProduct = new Product
            {
                ProductID = 1,
                ProductName = "Test Product",
                Description = "Test Description",
                Price = 10.0m,
                StockQuantity = 100,
                ModifiedDate = DateTime.Now,
                RowVersion = new byte[8] // Initialize with a default value
            };
            _context.Product.Add(testProduct);
            await _context.SaveChangesAsync();

            _editModel.Product = testProduct;

            // Simulate a concurrency conflict by modifying the RowVersion
            var entry = _context.Entry(testProduct);
            entry.Property("RowVersion").OriginalValue = new byte[8]; // Original value
            entry.Property("RowVersion").CurrentValue = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }; // New value to simulate conflict

            // Act
            var result = await _editModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_editModel.ModelState.IsValid, Is.False);
        }

        [Test]
        public async Task OnPostAsync_ProductNameIsEmpty_ReturnsPageWithModelError()
        {
            // Arrange
            var testProduct = new Product
            {
                ProductID = 1,
                ProductName = string.Empty,
                Description = "Test Description",
                Price = 10.0m,
                StockQuantity = 100,
                ModifiedDate = DateTime.Now,
                RowVersion = new byte[8] // Initialize with a default value
            };
            _context.Product.Add(testProduct);
            await _context.SaveChangesAsync();

            _editModel.Product = testProduct;

            // Simulate a concurrency conflict by modifying the RowVersion
            var entry = _context.Entry(testProduct);
            entry.Property("RowVersion").OriginalValue = new byte[8]; // Original value
            entry.Property("RowVersion").CurrentValue = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 }; // New value to simulate conflict

            // Act
            var result = await _editModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_editModel.ModelState.IsValid, Is.False);
        }
        [Test]
        public async Task OnPostAsync_ProductPriceIsInvalid_ReturnsPageWithModelError()
        {
            // Arrange
            var testProduct = new Product
            {
                ProductID = 1,
                ProductName = "Test Product",
                Description = "Test Description",
                Price = -10.0m, // Invalid Price to trigger validation error
                StockQuantity = 100,
                ModifiedDate = DateTime.Now,
                RowVersion = new byte[8] // Initialize with a default value
            };
            _context.Product.Add(testProduct);
            await _context.SaveChangesAsync();

            _editModel.Product = testProduct;
             // Manually trigger validation beacuse this thing nomás no validaba nada y google vino a salvar el dia
            var validationContext = new ValidationContext(_editModel.Product, null, null);
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(_editModel.Product, validationContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                _editModel.ModelState.AddModelError(validationResult.MemberNames.First(), validationResult.ErrorMessage);
            }

            // Act
            var result = await _editModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_editModel.ModelState.IsValid, Is.False);
        }
    }
}