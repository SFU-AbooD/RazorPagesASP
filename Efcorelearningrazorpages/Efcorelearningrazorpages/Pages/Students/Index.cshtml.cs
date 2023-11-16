using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Efcorelearningrazorpages.Data;
using Efcorelearningrazorpages.Models;

namespace Efcorelearningrazorpages.Pages.Students
{
    public class IndexModel : PageModel
    {
        private readonly Efcorelearningrazorpages.Data.SchoolContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        public IndexModel(Efcorelearningrazorpages.Data.SchoolContext context, ILogger<IndexModel> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;     
        }

        public PaginatedList<Student> Student { get;set; } = default!;
        public int? sort_asc { get; set; }
        public string? searchstring { get; set; }
        public async Task OnGetAsync(string? searchstring,string? current_filter ,int? sort_asc, int pageindex = 1)
        {
            this.sort_asc = sort_asc;
            this.searchstring = searchstring;
            // Iqueryable type works by method chaining not by using functions without assiging values
            IQueryable<Student> Student = from s in _context.Students select s; // init select statment!
            if (_context.Students != null)
            {
                if (!String.IsNullOrEmpty(searchstring))
                {
                    // bad performance since it uses to upper in parameter which could disable the indexing in database Student = Student.Where(x=>x.lastname.ToUpper().Contains(searchstring) || x.firstmidname.Contains(searchstring));
                    // use this instead the below code
                    Student = Student.Where(x => x.lastname.Contains(searchstring) || x.firstmidname.Contains(searchstring));
                    pageindex = 1;
                    current_filter = searchstring;
                }
                else
                {
                    searchstring = current_filter;
                }
                    switch (sort_asc) {
                        case 1:
                        Student= Student.OrderBy(x => x.lastname);
                            break;
                        case 2:
                        Student=Student.OrderBy(x => x.firstmidname);
                            break;
                        case 3:
                        Student =Student.OrderBy(x => x.EnrollmentDate);
                        break;
                    case -1:
                        Student = Student.OrderByDescending(x => x.lastname);
                        break;
                    case -2:
                        Student= Student.OrderByDescending(x => x.firstmidname);
                        break;
                    case -3:
                        Student= Student.OrderByDescending(x => x.EnrollmentDate);
                        break;
                }
                this.Student = await PaginatedList<Student>.CreateAsync(Student,pageindex,_configuration.GetValue("pagesize",4));
            }
        }
    }
}
