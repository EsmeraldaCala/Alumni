using Alumni.Data;
using Alumni.Models;
using Alumni.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Alumni.Controllers
{
    using Alumni = Models.Alumni;
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public IActionResult RegisterAlumni()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAlumni(AlumniRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                string? uniqueFileName = null;
                if (model.ProfilePicture != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePicture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.ProfilePicture.CopyToAsync(fileStream);
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, ProfilePicture = uniqueFileName };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Alumni");

                    var alumni = new Alumni
                    {
                        UserId = user.Id,
                        FieldOfStudy = model.FieldOfStudy,
                        JobPosition = model.JobPosition,
                        GraduationDate = model.GraduationDate,
                        Company = model.Company,
                    };

                    _dbContext.Alumni.Add(alumni);
                    await _dbContext.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterFacultyRep()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegisterFacultyRep(FacultyRepRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string? uniqueFileName = null;
                if (model.ProfilePicture != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ProfilePicture.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using var fileStream = new FileStream(filePath, FileMode.Create);
                    await model.ProfilePicture.CopyToAsync(fileStream);
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, ProfilePicture = uniqueFileName };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Faculty Representative");

                    var facultyRep = new FacultyRepresentative
                    {
                        UserId = user.Id,
                        Faculty = model.Faculty,
                        FieldOfStudy = model.FieldOfStudy
                    };

                    _dbContext.FacultyRepresentatives.Add(facultyRep);
                    await _dbContext.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
    }

}
