using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using X.PagedList;

namespace MyProject.Controllers;

public class HomeController : Controller
{
    private readonly MyDbContext _context;

    public HomeController(MyDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(int? page)
    {
        var pagesize = 10;
        var pageNumber = page ?? 1;

        return View(_context.Blog.Where(p=>p.yayinda==true).OrderByDescending(s => s.görüntülenmeSayisi).ToPagedList(pageNumber,pagesize));
    }
    public IActionResult Search(string k, int? page)
    {   
        var pagesize = 10;
        var pageNumber = page ?? 1;
        ViewBag.k = k;
        if(!string.IsNullOrEmpty(k))
        {
            List<Blog> blogs = _context.Blog.Where(p =>
            p.KeyWords.ToLower().Contains(k.ToLower()) ||
            p.Name.ToLower().Contains(k.ToLower()) ||
            p.Yazi.ToLower().Contains(k.ToLower())
            ).ToList();
            ViewBag.sayi = blogs.Count();
            return View(blogs.ToPagedList(pageNumber,pagesize));
        }else
        {
        
        return View(NotFound());
        }
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    } 
}
