using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PustokProject.CoreModels;
using PustokProject.Persistance;
using PustokProject.ViewModels;
using PustokProject.ViewModels.Blogs;
using System.Security.Policy;

namespace PustokProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BlogsController : Controller
    {

        public BlogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public ApplicationDbContext _context { get; }


        public async Task<IActionResult> Index()
        {
            var pagenumber = 1;
            var take = 3;

            var count = await _context.Blogs
                    .CountAsync();

            var blogs = (await _context.Blogs
                .Include(c => c.Author)
                .Include(c => c.Tags)
                .Skip((pagenumber - 1) * take)
                    .Take(take)
                    .ToListAsync())
                    .Select(b => new VM_BlogIndex
                    {
                        Title = b.Title,
                        Description = b.Description,
                        Content = b.Content,
                        Id = b.Id,
                        AuthorName = b.Author.Name,
                        IsDeleted = b.IsDeleted,
                        Tags = string.Join(", ", b.Tags.Select(t=>t.Title).ToList())
                    });

            var page = new VM_PaginatedEntityTable<VM_BlogIndex>();
            page.Items = blogs;
            page.PageCount = (int)Math.Ceiling((decimal)count / take);
            page.HasPrev = pagenumber > 1;
            page.HasNext = pagenumber < page.PageCount;
            page.CurrentPage = pagenumber;

            var routevaldic = new RouteValueDictionary();
            routevaldic["pagenumber"] = pagenumber + 1;
            routevaldic["take"] = take;

            page.NextPage = Url.Action(nameof(PaginatedBlogs), routevaldic);
            routevaldic["pagenumber"] = pagenumber - 1;
            page.PreviousPage = Url.Action(nameof(PaginatedBlogs), routevaldic);

            return View(page);
        }


        public async Task<IActionResult> PaginatedBlogs(int pagenumber,int take)
        {
            var count = await _context.Blogs
                    .CountAsync();

            var blogs = (await _context.Blogs
                .Include(c => c.Author)
                .Include(c => c.Tags)
                .Skip((pagenumber - 1) * take)
                    .Take(take)
                    .ToListAsync())
                    .Select(b => new VM_BlogIndex
                    {
                        Title = b.Title,
                        Description = b.Description,
                        Content = b.Content,
                        Id = b.Id,
                        AuthorName = b.Author.Name,
                        IsDeleted = b.IsDeleted,
                        Tags = string.Join(", ", b.Tags.Select(t => t.Title).ToList())
                    }); 

            var page = new VM_PaginatedEntityTable<VM_BlogIndex>();
            page.Items = blogs;
            page.PageCount = (int)Math.Ceiling((decimal)count / take);
            page.HasPrev = pagenumber > 1;
            page.HasNext = pagenumber < page.PageCount;
            page.CurrentPage = pagenumber;

            var routevaldic = new RouteValueDictionary();
            routevaldic["pagenumber"] = pagenumber + 1;
            routevaldic["take"] = take;

            page.NextPage = Url.Action(nameof(PaginatedBlogs), routevaldic);
            routevaldic["pagenumber"] = pagenumber - 1;
            page.PreviousPage = Url.Action(nameof(PaginatedBlogs), routevaldic);

            return PartialView("_PaginatedColumnsBlogs",page);
        }

        public async Task<IActionResult> CreateBlog()
        {
            ViewBag.Tags = new SelectList(await _context.Tags.ToListAsync(),"Id","Title");
            ViewBag.Authors = new SelectList( await _context.Authors.ToListAsync(), "Id", "Name"); ;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateBlog(VM_BlogCreate model)
        {
            var blog = new Blog()
            {
                Title = model.Title,
                Description = model.Description,
                Tags = await _context.Tags.Where(t => model.TagIds.Contains(t.Id)).ToListAsync(),
                AuthorId = model.AuthorId,
                Content = model.Content
            };

            await _context.Blogs.AddAsync(blog);    
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> UpdateBlog(int id)
        {
            ViewBag.Tags = new SelectList(await _context.Tags.ToListAsync(), "Id", "Title");
            ViewBag.Authors = new SelectList(await _context.Authors.ToListAsync(), "Id", "Name"); ;

            var blog = await _context.Blogs.Include(b=>b.Tags).FirstOrDefaultAsync(b=>b.Id == id);
            var model = new VM_BlogCreate();
            
            if(blog != null)
            {
                model.Id = blog.Id;
                model.Title = blog.Title;
                model.Description = blog.Description;
                model.TagIds = blog.Tags.Select(t=> t.Id).ToList();
                model.AuthorId = blog.AuthorId;
                model.Content = blog.Content;
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBlog(int id, VM_BlogCreate updateModel)
        {
            ViewBag.Tags = new SelectList(await _context.Tags.ToListAsync(), "Id", "Title");
            ViewBag.Authors = new SelectList(await _context.Authors.ToListAsync(), "Id", "Name");

            var blog = await _context.Blogs.Include(b => b.Tags).FirstOrDefaultAsync(b => b.Id == id);
            
            if (blog != null)
            {
                blog.Title = updateModel.Title;
                blog.Description = updateModel.Description;
                blog.Tags = await _context.Tags.Where(t => updateModel.TagIds.Contains(t.Id)).ToListAsync();
                blog.AuthorId = updateModel.AuthorId;
                blog.Content = updateModel.Content;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }





        public async Task<IActionResult> DeleteBlog(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if(blog!=null)
            {
                blog.Delete();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> RevokeDelete(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            if (blog != null)
            {
                blog.RevokeDelete();
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }



    }
}
