using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;
using Practice5.Pages_Purchases;
using System;
using System.Threading.Tasks;
using Practice5.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Practice5.Tests
{
    [TestFixture]
    public class CreatePurchaseTests
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
        public async Task OnPostAsync_PurchaseIsCreated_ReturnsRedirectToPageResult()
        {
            // Arrange
            _createModel.Purchase = new Purchase
            {
                PurchaseID = 1,
                ProductID = 1,
                PurchaseDate = DateTime.Now,
                QuantityPurchased = 10,
                PurchasePrice = 100.0m
            };

            // Act
            var result = await _createModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var createdPurchase = await _context.Purchase.FindAsync(_createModel.Purchase.PurchaseID);
            Assert.That(createdPurchase, Is.Not.Null);
            Assert.That(createdPurchase.ProductID, Is.EqualTo(1));
            Assert.That(createdPurchase.QuantityPurchased, Is.EqualTo(10));
            Assert.That(createdPurchase.PurchasePrice, Is.EqualTo(100.0m));
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
