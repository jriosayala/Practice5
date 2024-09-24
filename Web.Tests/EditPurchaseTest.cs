using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;
using Practice5.Pages_Purchases;
using System.Threading.Tasks;
using Practice5.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Practice5.Tests
{
    [TestFixture]
    public class EditPurchaseTest
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
        public async Task OnPostAsync_PurchaseIsUpdated_ReturnsRedirectToPageResult()
        {
            // Arrange
            var testPurchase = new Purchase
            {
                PurchaseID = 1,
                ProductID = 1,
                PurchaseDate = DateTime.Now,
                QuantityPurchased = 10,
                PurchasePrice = 100.0m
            };
            _context.Purchase.Add(testPurchase);
            await _context.SaveChangesAsync();

            // Modify the purchase details
            _editModel.Purchase = new Purchase
            {
                PurchaseID = 1,
                ProductID = 2,
                PurchaseDate = DateTime.Now.AddDays(1),
                QuantityPurchased = 20,
                PurchasePrice = 200.0m
            };

            // Act
            var result = await _editModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var updatedPurchase = await _context.Purchase.FindAsync(testPurchase.PurchaseID);
            Assert.That(updatedPurchase, Is.Not.Null);
            Assert.That(updatedPurchase.ProductID, Is.EqualTo(2));
            Assert.That(updatedPurchase.PurchaseDate, Is.EqualTo(DateTime.Now.AddDays(1)).Within(1).Minutes);
            Assert.That(updatedPurchase.QuantityPurchased, Is.EqualTo(20));
            Assert.That(updatedPurchase.PurchasePrice, Is.EqualTo(200.0m));
        }

        [Test]
        public async Task OnPostAsync_PurchaseDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            _editModel.Purchase = new Purchase
            {
                PurchaseID = 999, // Non-existent PurchaseID
                ProductID = 2,
                PurchaseDate = DateTime.Now.AddDays(1),
                QuantityPurchased = 20,
                PurchasePrice = 200.0m
            };

            // Act
            var result = await _editModel.OnPostAsync();

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
