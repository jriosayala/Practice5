using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Practice5.Data;

namespace Practice5.Pages_Sales
{
    public class IndexModel : PageModel
    {
        private readonly Practice5.Data.AppDbContext _context;

        public IndexModel(Practice5.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Sale> Sale { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Sale = await _context.Sale
                .Include(s => s.Product).ToListAsync();
        }
    }
}
