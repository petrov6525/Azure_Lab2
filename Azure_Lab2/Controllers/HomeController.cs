using Azure_Lab2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Azure_Lab2.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationContext context { get; set; }
        
        public HomeController(ApplicationContext applicationContext)
        {
            context = applicationContext;
            
        }

        public async Task<IActionResult> Index()
        {
            await AzureService.TryCreateBlobContainer("home");

            var images=context.Pictures.AsNoTracking().ToList();

            ViewBag.images = images;


            return View();
        }

        public async Task<IActionResult> UploadImageAsync(IFormFile? image)
        {
            string? extension = Path.GetExtension(image?.FileName);

            string[] exs = { ".jpg", ".bmp", ".jpeg", ".jfif", ".webp" };

            if(Array.IndexOf(exs, extension) == -1)
            {
                return View("Index");
            }

            var currentImage = context.Pictures.FirstOrDefault(i => i.Name.Equals(image.FileName));

            if(currentImage != null)
            {
                return RedirectToAction("Index");                    
            }

            await AzureService.UploadFile(image!);

            await context.AddImage(image!);

            await AzureService.DownloadFile(image!.FileName);

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        
    }
}