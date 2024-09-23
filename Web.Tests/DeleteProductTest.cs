using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;
using Practice5.Pages_Products;
using System.Threading.Tasks;
using Practice5.Models;

namespace Practice5.Tests
{
    [TestFixture]
    public class DeleteProductTests
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
        public async Task OnPostAsync_ProductIsDeleted_ReturnsRedirectToPageResult()
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

            // Act
            var result = await _deleteModel.OnPostAsync(testProduct.ProductID);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var deletedProduct = await _context.Product.FindAsync(testProduct.ProductID);
            Assert.That(deletedProduct, Is.Null);
        }
    }
}
