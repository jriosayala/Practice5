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
    public class DetailsModel : PageModel
    {
        private readonly Practice5.Data.AppDbContext _context;

        public DetailsModel(Practice5.Data.AppDbContext context)
        {
            _context = context;
        }

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
    }
}
