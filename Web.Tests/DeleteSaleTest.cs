using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;
using Practice5.Pages_Sales;
using System.Threading.Tasks;
using Practice5.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Practice5.Tests
{
    [TestFixture]
    public class DeleteSaleTests
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
        public async Task OnPostAsync_SaleIsDeleted_ReturnsRedirectToPageResult()
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

            var testSale = new Sale
            {
                SaleID = 1,
                ProductID = testProduct.ProductID,
                SaleDate = DateTime.Now,
                QuantitySold = 5,
                SalePrice = 250.0m,
                Product = testProduct
            };
            _context.Sale.Add(testSale);
            await _context.SaveChangesAsync();

            // Act
            var result = await _deleteModel.OnPostAsync(testSale.SaleID);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var deletedSale = await _context.Sale.FindAsync(testSale.SaleID);
            Assert.That(deletedSale, Is.Null);
        }

        [Test]
        public async Task OnPostAsync_SaleDoesNotExist_ReturnsNotFoundResult()
        {
            // Act
            var result = await _deleteModel.OnPostAsync(999); // Non-existent SaleID

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task OnGetAsync_SaleExists_ReturnsPageResult()
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

            var testSale = new Sale
            {
                SaleID = 1,
                ProductID = testProduct.ProductID,
                SaleDate = DateTime.Now,
                QuantitySold = 5,
                SalePrice = 250.0m,
                Product = testProduct
            };
            _context.Sale.Add(testSale);
            await _context.SaveChangesAsync();

            // Act
            var result = await _deleteModel.OnGetAsync(testSale.SaleID);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_deleteModel.Sale, Is.Not.Null);
            Assert.That(_deleteModel.Sale.SaleID, Is.EqualTo(testSale.SaleID));
        }

        [Test]
        public async Task OnGetAsync_SaleDoesNotExist_ReturnsNotFoundResult()
        {
            // Act
            var result = await _deleteModel.OnGetAsync(999); // Non-existent SaleID

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
