using Alumni.Data;
using Alumni.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Alumni.Controllers
{
    public class AlumniStudentsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<ApplicationUser> _userManager;


        public AlumniStudentsController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment)
        {

            _logger = logger;
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var alumni = await _context.Alumni.ToListAsync();
            return View(alumni);

        }


        public IActionResult GetAlumniImage(int id)
        {
            var @alumni = _context.Alumni.FirstOrDefault(e => e.Id == id);
            if (@alumni != null)
            {

                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", @alumni.ApplicationUser.ProfilePicture);


                if (System.IO.File.Exists(imagePath))
                {
                    var image = System.IO.File.OpenRead(imagePath);
                    return File(image, "images/jpeg");
                }
            }

            return NotFound();
        }

    }
}
