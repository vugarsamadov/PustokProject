using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokProject.Persistance;
using static PustokProject.Controllers.HomeController;

namespace PustokProject.Views.Shared.Component.Basket
{
	public class BasketComponent : ViewComponent
	{

        public BasketComponent(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public ApplicationDbContext DbContext { get; }

        public async Task<IViewComponentResult> InvokeAsync()
		{

            var basketItems = HttpContext.Request.Cookies["basket"] ?? "[]";
            var basketItemsList = System.Text.Json.JsonSerializer.Deserialize<ICollection<BasketItem>>(basketItems);

            var basketVMItems = basketItemsList
                .Join(
                await DbContext
                .Books
                .Where(b => basketItemsList.Select(a => a.Id).Contains(b.Id)).ToListAsync(), a => a.Id, a => a.Id,
                (x, y) => new BasketItemVM(y.Id, y.Name, y.CoverImageUrl, y.Price, x.Count)).ToList();
            ViewBag.BasketItems = basketVMItems;

            return View();
		}

	}
}
