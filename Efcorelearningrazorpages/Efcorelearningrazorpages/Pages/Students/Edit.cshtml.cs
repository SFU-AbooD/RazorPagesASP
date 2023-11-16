using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;

namespace Efcorelearningrazorpages.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;

        public EditModel(Efcorelearningrazorpages.Data.SchoolContext context)
        {
            _context = context;
        }

        public Student Student { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Students == null)
            {
                return NotFound();
            }

            var student =  await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            Student = student;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(int? id)
        {

            var student = await _context.Students.FindAsync(id);
            // i guess by default this should be like - and you can set state to ensure that you changed to make more efficent 

            try
            {
                if (await TryUpdateModelAsync<Student>(student, "student", s => s.firstmidname, s => s.lastname, s => s.EnrollmentDate))
                {
                    _context.Attach(Student).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Index");
                }
                await _context.SaveChangesAsync();
            }catch (Exception ex) { }
            return RedirectToPage("./Index");
        }

    }
}
