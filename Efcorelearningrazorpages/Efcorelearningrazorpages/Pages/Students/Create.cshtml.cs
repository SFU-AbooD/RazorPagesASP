using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Efcorelearningrazorpages.Pages.Students
{
    public class CreateModel : PageModel
    {
        // uses the Pagemodel value provider
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public CreateModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        [BindProperty]
        public StudenVM? Student { get; set; }
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (Student == null)
                return NotFound();
            Student emp = new Student();
            if(await TryUpdateModelAsync<Student>(emp,"student",s => s.firstmidname, s => s.lastname, s => s.EnrollmentDate)){
                var entry = _context.Add(new Student());
                entry.Property(u => u.secret).CurrentValue = "this is something hidden from insertion";
                entry.CurrentValues.SetValues(Student); // here you can pass the dto 
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
                return Page();
        }
    }
}
