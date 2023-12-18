using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokProject.Models;
using PustokProject.Persistance;
using System.Diagnostics;
using PustokProject.CoreModels;
using PustokProject.ViewModels.Books.Non_Admin;

namespace PustokProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ApplicationDbContext dbContext { get; }

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext DbContext)
        {
            _logger = logger;
            dbContext = DbContext;
        }

        public class BasketItem
        {
            public int Count { get; set; }
            public int Id { get; set; }
        }

        public void AddToBasket(int id)
        {
            var basketItems = HttpContext.Request.Cookies["basket"] ?? "[]";
            
            var basketItemsList = System.Text.Json.JsonSerializer.Deserialize<ICollection<BasketItem>>(basketItems);
            var item = basketItemsList.FirstOrDefault(bi => bi.Id == id);
            if (item != null)
            {
                item.Count++;
            }
            else
            {
                var bi = new BasketItem() {Id = id,Count = 1 };
                basketItemsList.Add(bi);
            }
            var jsonBasket = System.Text.Json.JsonSerializer.Serialize(basketItemsList);
            HttpContext.Response.Cookies.Append("basket", jsonBasket);
        }
        public void RemoveFromBasket(int id)
        {
            var basketItems = HttpContext.Request.Cookies["basket"];
            if (basketItems != null)
            { 
            
            var basketItemsList = System.Text.Json.JsonSerializer.Deserialize<ICollection<BasketItem>>(basketItems);
            var item = basketItemsList.FirstOrDefault(bi => bi.Id == id);
            if (item != null)
            {
                    basketItemsList.Remove(item);
            }
            
            var jsonBasket = System.Text.Json.JsonSerializer.Serialize(basketItemsList);
            HttpContext.Response.Cookies.Append("basket", jsonBasket);
            }
        }


        public string GetBasket()
        {
            var basketItems = HttpContext.Request.Cookies["basket"] ?? "[]";
            return basketItems;
        }

        public record BasketItemVM(int Id,string Name,string ImageUrl,decimal Price,int Count);

        public async Task<IActionResult> Index()
        {
            var model = new VM_Home();
            model.Sliders = await dbContext.Sliders.Where(b=>!b.IsDeleted).ToListAsync();
            model.Books = await dbContext.Books.Where(b=>!b.IsDeleted).ToListAsync();

            var basketItems = HttpContext.Request.Cookies["basket"] ?? "[]";
            var basketItemsList = System.Text.Json.JsonSerializer.Deserialize<ICollection<BasketItem>>(basketItems);

            var basketVMItems = basketItemsList
                .Join(
                await dbContext
                .Books
                .Where(b => basketItemsList.Select(a => a.Id).Contains(b.Id)).ToListAsync(),a=>a.Id,a=>a.Id,
                (x,y)=> new BasketItemVM(y.Id,y.Name,y.CoverImageUrl,y.Price,x.Count)).ToList();
            ViewBag.BasketItems = basketVMItems;

            var BooksAbove20Perc = await dbContext.Books.Where(b => !b.IsDeleted && b.DiscountPercentage > 20).Skip(0).Take(4).ToListAsync();
            var count = await dbContext.Books.Where(b => !b.IsDeleted && b.DiscountPercentage > 20).CountAsync();

            var hasNext = (count >= 4);

            model.PagedBookVM = new PagedBooksVm<IEnumerable<Book>>(hasNext, 0,8, BooksAbove20Perc);


           // model.BooksAbove20Perc = await dbContext.Books.Where(b=>!b.IsDeleted && b.DiscountPercentage > 20).ToListAsync();
            model.BooksChildren = await dbContext.Books
                .Include(b=>b.Category)
                .Where(b=>!b.IsDeleted && b.Category.Name == "Children")
                .ToListAsync();
            return View(model);
        }

        [HttpGet]

        public async Task<IActionResult> BookPagination(int skip=0,int take = 4)
        {

            var BooksAbove20Perc = await dbContext.Books.Where(b => !b.IsDeleted && b.DiscountPercentage > 20).Skip(skip).Take(take).ToListAsync();
            var count = await dbContext.Books.Where(b => !b.IsDeleted && b.DiscountPercentage > 20).CountAsync();

            var hasNext = (count >= skip + take);

            var model = new PagedBooksVm<IEnumerable<Book>>(hasNext,0,take+4,BooksAbove20Perc);


            return PartialView("_BooksPagedViewPartial", model);
        }


        public IActionResult Privacy()
        {
            throw new NotImplementedException("s");
        }

        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }





    }
}