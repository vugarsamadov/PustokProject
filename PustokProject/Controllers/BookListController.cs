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

			model.Authors = await _dbContext.Authors
				.Select(a=>new AuthorsBookListVmItem
				(
					a.Name,a.BookAuthors.Count(),a.Id
				)).ToListAsync();

			model.Categories = await _dbContext.Categories.Include(c => c.SubCategories)
				.Where(c=>c.ParentId == null)
				.Select(c => new CategoriesBookListVmItem(c.Name, c.Books.Count(), c.Id,
				c.SubCategories.Select(s => new CategoriesBookListVmItem(s.Name,s.Books.Count(),s.Id,null))
				)).ToListAsync();
			return View(model);
		}

		public async Task<PartialViewResult> GetBooks(int? categoryId,IEnumerable<int>? authorIds)
		{
			var query = _dbContext.Books.Include(b=>b.BookAuthors).ThenInclude(a=>a.Author).AsQueryable();

			if (authorIds != null && authorIds.Count()>0)
			{
				query = query.Where(b => b.BookAuthors.Any(b=>authorIds.Contains(b.AuthorId)));
			}
			if(categoryId != null)
			{
				query = query.Where(b => b.CategoryId == categoryId);
			}

			var queryExec = await query.ToListAsync();

			var model = queryExec
				.Select(b=>new VM_BookListItem
				{
					Name = b.Name,
					Authors = string.Join(", ",b.BookAuthors.Select(a=>a.Author.Name)),
					Description = b.Description,
					ImageUrl = b.CoverImageUrl,
					Price = b.Price,
					DiscountedPrice = b.DiscountedPrice,
					DiscountRate = b.DiscountPercentage
				}).AsEnumerable();


			return PartialView("_BookListPartial", model);
		}

	}
}
