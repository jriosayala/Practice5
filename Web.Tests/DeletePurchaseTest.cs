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
    public class DeletePurchaseTests
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
        public async Task OnPostAsync_PurchaseIsDeleted_ReturnsRedirectToPageResult()
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

            // Act
            var result = await _deleteModel.OnPostAsync(testPurchase.PurchaseID);

            // Assert
            Assert.That(result, Is.InstanceOf<RedirectToPageResult>());
            var deletedPurchase = await _context.Purchase.FindAsync(testPurchase.PurchaseID);
            Assert.That(deletedPurchase, Is.Null);
        }

        [Test]
        public async Task OnPostAsync_PurchaseDoesNotExist_ReturnsNotFoundResult()
        {
            // Act
            var result = await _deleteModel.OnPostAsync(999); // Non-existent PurchaseID

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public async Task OnGetAsync_PurchaseExists_ReturnsPageResult()
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

            // Act
            var result = await _deleteModel.OnGetAsync(testPurchase.PurchaseID);

            // Assert
            Assert.That(result, Is.InstanceOf<PageResult>());
            Assert.That(_deleteModel.Purchase, Is.Not.Null);
            Assert.That(_deleteModel.Purchase.PurchaseID, Is.EqualTo(testPurchase.PurchaseID));
        }

        [Test]
        public async Task OnGetAsync_PurchaseDoesNotExist_ReturnsNotFoundResult()
        {
            // Act
            var result = await _deleteModel.OnGetAsync(999); // Non-existent PurchaseID

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
