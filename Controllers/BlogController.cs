using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyProject.Models;
using Slugify;
using X.PagedList;
namespace MyProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly MyDbContext _context;

        public BlogController(MyDbContext context)
        {
            _context = context;
        }

        // GET: Blog
        [Authorize]
        public IActionResult Index(int? page)
        {
            var pagesize = 10;
            var pageNumber = page ?? 1;

            return View(_context.Blog.OrderByDescending(s => s.Tarih).ToPagedList(pageNumber,pagesize));
        }

        // GET: Blog/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .FirstOrDefaultAsync(m => m.slugfield == id);
            blog.görüntülenmeSayisi+=1;
            await _context.SaveChangesAsync();
            if (blog == null)
            {
                return NotFound();
            }
            if ((blog.yayinda==false) && (User.Identity.IsAuthenticated==false))
            {
                return RedirectToAction("index","home");
            }

            return View(blog);
        }

        // GET: Blog/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([Bind("BlogId,Name,Yazi,KeyWords,yayinda")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                SlugHelper helper = new SlugHelper();
                blog.slugfield = helper.GenerateSlug(blog.Name);
                blog.Tarih = DateTime.Now;
                User user = await _context.User.FirstOrDefaultAsync(p=>p.Username == User.Identity.Name.ToString());
                user.BlogSayisi+=1;
                blog.yazar = user.Username;
                _context.Add(blog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }
        
        
        [Authorize]
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog.FirstOrDefaultAsync(p=>p.slugfield == id);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("BlogId,Name,Yazi,yayinda,KeyWords,yazar,Tarih,slugfield,görüntülenmeSayisi")] Blog blog)
        {
            

            if (ModelState.IsValid)
            {
                try
                {

                    _context.Update(blog);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.BlogId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blog/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Blog == null)
            {
                return NotFound();
            }

            var blog = await _context.Blog
                .FirstOrDefaultAsync(m => m.slugfield == id);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blog/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Blog == null)
            {
                return Problem("Entity set 'MyDbContext.Blog'  is null.");
            }
            var blog = await _context.Blog.FirstOrDefaultAsync(p=>p.slugfield==id);
            if (blog != null)
            {
                _context.Blog.Remove(blog);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogExists(int id)
        {
          return _context.Blog.Any(e => e.BlogId == id);
        }
    }
}
