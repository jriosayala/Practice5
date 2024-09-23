using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;
using Practice5.Pages_Products;
using System.Threading.Tasks;
using Practice5.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Practice5.Tests
{
    [TestFixture]
    public class CreateProductTests
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
        public async Task OnPostAsync_ProductIsCreated_ReturnsRedirectToPageResult()
        {
            // Arrange
            _createModel.Product = new Product
            {
                ProductID = 1,
                ProductName = "New Product",
                Description = "New Description",
                Price = 15.0m,
                StockQuantity = 50,
                ModifiedDate = DateTime.Now,
                RowVersion = new byte[8] // Initialize with a default value
            };

            // Act
            var result = await _createModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var createdProduct = await _context.Product.FindAsync(_createModel.Product.ProductID);
            Assert.That(createdProduct, Is.Not.Null);
            Assert.That(createdProduct.ProductName, Is.EqualTo("New Product"));
            Assert.That(createdProduct.Description, Is.EqualTo("New Description"));
            Assert.That(createdProduct.Price, Is.EqualTo(15.0m));
            Assert.That(createdProduct.StockQuantity, Is.EqualTo(50));
        }

        [Test]
        public async Task OnPostAsync_InvalidModelState_ReturnsPageResult()
        {
            // Arrange
            _createModel.Product = new Product
            {
                ProductID = 1,
                ProductName = "", // Invalid ProductName to trigger validation error
                Description = "New Description",
                Price = 15.0m,
                StockQuantity = 50,
                ModifiedDate = DateTime.Now,
                RowVersion = new byte[8] // Initialize with a default value
            };
            _createModel.ModelState.AddModelError("Product.ProductName", "The ProductName field is required.");

            // Act
            var result = await _createModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_createModel.ModelState.IsValid, Is.False);
            Assert.That(_createModel.ModelState["Product.ProductName"].Errors, Has.One.Items);
            Assert.That(_createModel.ModelState["Product.ProductName"].Errors[0].ErrorMessage, Is.EqualTo("The ProductName field is required."));
        }
    }
}
