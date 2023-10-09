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
            var alumni = await _context.Alumni.Include(a=>a.ApplicationUser).ToListAsync();
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

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {

            //if (_auth.Identity == null)
            //    return RedirectToAction("Welcome", "Account");
            if (id == null)
            {
                return NotFound();
            }

            var alumni = await _context.Alumni.Include(a=>a.ApplicationUser).FirstOrDefaultAsync(x=>x.Id == id);
            if (alumni == null)
            {
                return NotFound();
            }
            return View(new AlumniRegisterViewModel()
            {
                Id =alumni.Id,
                UserId = alumni.ApplicationUser.Id,
                FirstName = alumni.ApplicationUser?.FirstName,
                LastName = alumni.ApplicationUser?.LastName,
                Email = alumni.ApplicationUser?.Email,
                GraduationDate = alumni.GraduationDate,
                FieldOfStudy = alumni.FieldOfStudy,
                Company = alumni.Company,
                JobPosition = alumni.JobPosition,
            });

        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AlumniRegisterViewModel alumni)
        {

            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");
            ModelState.Remove("ProfilePicture");
            if (ModelState.IsValid)
            {
                try
                {
                    if (alumni.ProfilePicture != null)
                    {
                        string? uniqueFileName = null;
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + alumni.ProfilePicture.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        await alumni.ProfilePicture.CopyToAsync(fileStream);
                        //alumni.ProfilePicture = uniqueFileName;
                    }
                    var user = _userManager.Users.FirstOrDefault(x => x.Id == alumni.UserId);
                    if (user == null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    user.FirstName = alumni.FirstName;
                    user.LastName = alumni.LastName;
                    user.Email = alumni.Email;
                    await _userManager.UpdateAsync(user);
                    var alumniRecord = await _context.Alumni.FirstOrDefaultAsync(x => x.Id == alumni.Id);
                    if (alumniRecord == null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    alumniRecord.FieldOfStudy = alumni.FieldOfStudy;
                    alumniRecord.JobPosition = alumni.JobPosition;
                    alumniRecord.Company = alumni.Company;
                    alumniRecord.GraduationDate = alumni.GraduationDate;
                    _context.Alumni.Update(alumniRecord);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    //if (!AlumniExists(alumni.Id))
                    //{
                    //    return NotFound();
                    //}
                    //else
                    //{
                    //    throw;
                    //}
                }
                return RedirectToAction(nameof(Index));
            }
            return View(alumni);
        }

    }
    //private bool AlumniExists(int id)
    //{
    //    return _context.Events.Any(e => e.Id == id);
    //}
}
