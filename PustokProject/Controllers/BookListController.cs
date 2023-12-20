using System.Configuration;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokProject.Persistance;
using PustokProject.ViewModels.BookList;

namespace PustokProject.Controllers
{
	public class BookListController : Controller
	{
        public ApplicationDbContext _dbContext { get; set; }
        public BookListController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
		{
			var model = new VM_BookList();

			var result = await _dbContext.Authors.FromSqlRaw("select Id,Count(Id) as count FROM [Authors] A group by A.Id").ToListAsync();
			//model.Categories = _dbContext
			//	.Categories
			//	.Where(c=>c.ParentId == null)
			//	.Join(_dbContext.Categories.Where(c=>c.ParentId != null),c=>c.Id,c=>c.ParentId)	

			var parentCategories = _dbContext.Categories.Where(c => c.ParentId == null);
				
			





			return View();
		}
	}
}
