using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;
using System.Data;

namespace Efcorelearningrazorpages.Pages.Departments
{
    public class DeleteModel : PageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public DeleteModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }
        public string ConcurrencyErrorMessage { get; set; }

        [BindProperty]
      public Department Department { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id,bool ? concurrencyError)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.Include(x=>x.Administrator).AsNoTracking().FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (concurrencyError.GetValueOrDefault())
            {
                ConcurrencyErrorMessage = "The record you attempted to delete "
                  + "was modified by another user after you selected delete. "
                  + "The delete operation was canceled and the current values in the "
                  + "database have been displayed. If you still want to delete this "
                  + "record, click the Delete button again.";
            }
            if (department == null)
            {
                return NotFound();
            }
            else 
            {
                Department = department;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }
            var department = await _context.Departments.FindAsync(id);

            if (department != null)
            {
                try
                {
                    // Department = department;
                    if ((await _context.Departments.AnyAsync(x => x.DepartmentID == id)))
                    {
                        _context.Remove(department);
                        await _context.SaveChangesAsync();
                    }
                }
                catch(DBConcurrencyException) {
                    return RedirectToPage("./Delete",
             new { concurrencyError = true, id = id });
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
