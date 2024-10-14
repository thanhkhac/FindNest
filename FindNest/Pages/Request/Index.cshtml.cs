using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FindNest.Data;
using FindNest.Data.Models;

// https://sandbox.vnpayment.vn/vnpaygw-sit-testing/order

namespace FindNest.Pages.Request
{
    public class IndexModel : PageModel
    {
        private readonly FindNest.Data.FindNestDbContext _context;

        public IndexModel(FindNest.Data.FindNestDbContext context)
        {
            _context = context;
        }

        public IList<ReceivedRequest> ReceivedRequest { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ReceivedRequest = await _context.Requests.OrderByDescending(x=>x.Id).ToListAsync();
        }
    }
}
