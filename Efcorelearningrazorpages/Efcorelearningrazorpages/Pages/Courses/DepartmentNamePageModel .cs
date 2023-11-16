using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Efcorelearningrazorpages.Pages.Courses
{
    public class DepartmentNamePageModel : PageModel
    {
        public SelectList DepartmentNameSL { get;set;}
        public void populateselectlist(SchoolContext context, object selectedDepartment = null)
        {
            // create query object for sendting it to the server!
            IQueryable<Department> query = 
                from e in context.Departments orderby e.Name select e;
            DepartmentNameSL = new SelectList(query.AsNoTracking(), nameof(Department.DepartmentID),
                nameof(Department.Name),selectedDepartment);
        }
    }
}
