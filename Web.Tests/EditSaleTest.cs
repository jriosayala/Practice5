using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;
using Practice5.Pages_Sales;
using System;
using System.Threading.Tasks;
using Practice5.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Practice5.Tests
{
    [TestFixture]
    public class EditSaleTests
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
        public async Task OnPostAsync_SaleIsUpdated_ReturnsRedirectToPageResult()
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

            // Retrieve the existing sale from the context
            var saleToUpdate = await _context.Sale.FindAsync(testSale.SaleID);

            _editModel.Sale = new Sale
            {
                SaleID = saleToUpdate.SaleID,
                ProductID = saleToUpdate.ProductID,
                SaleDate = DateTime.Now.AddDays(1), // Update SaleDate
                QuantitySold = 10, // Update QuantitySold
                SalePrice = 500.0m, // Update SalePrice
                Product = saleToUpdate.Product
            };

            // Act
            var result = await _editModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var updatedSale = await _context.Sale.FindAsync(testSale.SaleID);
            Assert.That(updatedSale, Is.Not.Null);
            Assert.That(updatedSale.SaleDate, Is.EqualTo(_editModel.Sale.SaleDate));
            Assert.That(updatedSale.QuantitySold, Is.EqualTo(_editModel.Sale.QuantitySold));
            Assert.That(updatedSale.SalePrice, Is.EqualTo(_editModel.Sale.SalePrice));
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
            var result = await _editModel.OnGetAsync(testSale.SaleID);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_editModel.Sale, Is.Not.Null);
            Assert.That(_editModel.Sale.SaleID, Is.EqualTo(testSale.SaleID));
        }

        [Test]
        public async Task OnGetAsync_SaleDoesNotExist_ReturnsNotFoundResult()
        {
            // Act
            var result = await _editModel.OnGetAsync(999); // Non-existent SaleID

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
