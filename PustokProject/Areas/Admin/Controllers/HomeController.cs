using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PustokProject.CoreModels;
using PustokProject.Enums;
using PustokProject.Persistance;
using PustokProject.ViewModels;
using PustokProject.ViewModels.Sliders;
using System.Text.Json.Nodes;

namespace PustokProject.Areas.Home.Controllers
{

    [Area("Admin")]
    public class HomeController : Controller
    {

        public ApplicationDbContext _context { get; }

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index()
        {

            var pagenumber = 1;
            var take = 3;

            var count = await _context.Sliders
                    .CountAsync();

            var sliders = await _context.Sliders
                .Skip((pagenumber - 1) * take)
                    .Take(take)
                    .ToListAsync();

            var page = new VM_PaginatedEntityTable<Slider>();
            page.Items = sliders;
            page.PageCount = (int)Math.Ceiling((decimal)count / take);
            page.HasPrev = pagenumber > 1;
            page.HasNext = pagenumber < page.PageCount;
            page.CurrentPage = pagenumber;

            var routevaldic = new RouteValueDictionary();
            routevaldic["pagenumber"] = pagenumber + 1;
            routevaldic["take"] = take;

            page.NextPage = Url.Action(nameof(PaginatedSliders), routevaldic);
            routevaldic["pagenumber"] = pagenumber - 1;
            page.PreviousPage = Url.Action(nameof(PaginatedSliders), routevaldic);

            return View(page);            
        }

        public async Task<IActionResult> PaginatedSliders(int pagenumber, int take)
        {

            var count = await _context.Sliders
                    .CountAsync();

            var sliders = await _context.Sliders
                .Skip((pagenumber - 1) * take)
                    .Take(take)
                    .ToListAsync();

            var page = new VM_PaginatedEntityTable<Slider>();
            page.Items = sliders;
            page.PageCount = (int)Math.Ceiling((decimal)count / take);
            page.HasPrev = pagenumber > 1;
            page.HasNext = pagenumber < page.PageCount;
            page.CurrentPage = pagenumber;

            var routevaldic = new RouteValueDictionary();
            routevaldic["pagenumber"] = pagenumber + 1;
            routevaldic["take"] = take;

            page.NextPage = Url.Action(nameof(PaginatedSliders), routevaldic);
            routevaldic["pagenumber"] = pagenumber - 1;
            page.PreviousPage = Url.Action(nameof(PaginatedSliders), routevaldic);

            return PartialView("_PaginatedColumnsSliders", page);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(VM_CreateSlider createModel)
        {
            if (ModelState.IsValid)
            {
                var slider = new Slider();
                slider.Title = createModel.Title;
                slider.Description = createModel.Description;
                slider.ThumpnailUrl = createModel.ThumpnailUrl;
                slider.ButtonText = createModel.ButtonText;
                slider.TextPosition = (HeroAreaTextPosition) createModel.TextPosition;
                await _context.Sliders.AddAsync(slider);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        
        
        [HttpGet]
        public async Task<IActionResult> UpdateSlider(int id)
        {
            var vm = new VM_UpdateSlider();
            var slider = await _context.Sliders.FindAsync(id);
            vm.Id = slider.Id;
            vm.Title = slider.Title;
            vm.Description = slider.Description;
            vm.ButtonText = slider.ButtonText;
            vm.TextPosition = (int)slider.TextPosition;
            vm.ThumpnailUrl = slider.ThumpnailUrl;

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSlider(VM_UpdateSlider model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var slider = await _context.Sliders.FindAsync(model.Id);
            slider.Description = model.Description;
            slider.Title = model.Title;
            slider.ButtonText = model.ButtonText;
            slider.ThumpnailUrl = model.ThumpnailUrl;
            slider.TextPosition = (HeroAreaTextPosition)model.TextPosition;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); 
        }
    
        public async Task<IActionResult> DeleteSlider(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider != null)
            {
                slider.Delete();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> RevokeDelete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider != null)
            {
                slider.RevokeDelete();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        
        
        
    }
}
