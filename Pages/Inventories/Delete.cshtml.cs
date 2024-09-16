using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;

namespace Practice5.Pages_Inventories
{
    public class DeleteModel : PageModel
    {
        private readonly Practice5.Data.AppDbContext _context;

        public DeleteModel(Practice5.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Inventory Inventory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories.FirstOrDefaultAsync(m => m.InventoryID == id);

            if (inventory == null)
            {
                return NotFound();
            }
            else
            {
                Inventory = inventory;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventories.FindAsync(id);
            if (inventory != null)
            {
                Inventory = inventory;
                _context.Inventories.Remove(Inventory);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
