using Alumni.Data;
using Alumni.Models;
using Alumni.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Alumni.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<ApplicationUser> _userManager; 
      

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
         
        }

        public async Task<IActionResult> Index()
        {
            var alumniList = await _context.Alumni.Include(a=> a.ApplicationUser).ToListAsync();

           
            var eventList = await _context.Events.ToListAsync(); // Assuming you have an Events DbSet in your ApplicationDbContext

            var viewModel = new HomeIndexViewModel
            {
                AlumniList = alumniList,
                EventList = eventList
            };

            return View(viewModel);
        }


        public IActionResult GetUserImage(int id)
        {
            var user = _userManager.Users.FirstOrDefault(u => u.Id == id);

            if (user != null)
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", user.ProfilePicture);
                if (System.IO.File.Exists(imagePath))
                {
                    var image = System.IO.File.OpenRead(imagePath);
                    return File(image, "image/jpeg");
                }
            }

            return NotFound();
        }


        public IActionResult GetEventPhoto(int id)
        {
            var events = _context.Events.FirstOrDefault(u => u.Id == id);

            if (events != null && events.Photo != null)
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", events.Photo);
                if (System.IO.File.Exists(imagePath))
                {
                    var image = System.IO.File.OpenRead(imagePath);
                    return File(image, "image/jpeg");
                }
                
            }

            return NotFound();
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
}