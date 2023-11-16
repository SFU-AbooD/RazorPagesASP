using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Efcorelearningrazorpages.Pages
{
    public class AboutModel : PageModel
    {
        private readonly SchoolContext _context;
        public AboutModel(SchoolContext context)
        {
            _context = context;
        }
        public IList<GroupStudent> students { get; set; }   
        public async Task OnGetAsync()
        {
            IQueryable<GroupStudent> query = from entry in _context.Students
                                             group entry by entry.EnrollmentDate into groupdate
                                             select new GroupStudent
                                             {
                                                 EnrollmentDate = groupdate.Key,
                                                 StudentCount = groupdate.Count()
                                             };
            students = await query.ToListAsync();
        }
    }
}
