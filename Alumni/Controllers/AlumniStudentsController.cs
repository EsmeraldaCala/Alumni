using Alumni.Data;
using Alumni.Models;
using Alumni.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace Alumni.Controllers
{
    public class AlumniStudentsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IScopedAuthentication _auth;


        public AlumniStudentsController(ILogger<HomeController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment webHostEnvironment, IScopedAuthentication auth)
        {

            _logger = logger;
            _context = context;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
            _auth = auth;
        }

        public async Task<IActionResult> Index()
        {
            AlumniFacultyVM alumniFacultyVM = new AlumniFacultyVM()
            {
                Alumnis = await _context.Alumni.Include(a => a.ApplicationUser).ToListAsync(),
                FacultyRepresentatives = await _context.FacultyRepresentatives.Include(a => a.ApplicationUser).ToListAsync()
            };

            //  var alumni = await _context.Alumni.Include(a=>a.ApplicationUser).ToListAsync();
            return View(alumniFacultyVM);

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

            var alumni = await _context.Alumni.Include(a => a.ApplicationUser).FirstOrDefaultAsync(x => x.Id == id);
            if (alumni == null)
            {
                return NotFound();
            }
            return View(new AlumniRegisterViewModel()
            {
                Id = alumni.Id,
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

        public async Task<IActionResult> Delete(int? id, string source)
        {
            if (source == "fr")
            {
                var fr = await _context.FacultyRepresentatives.FindAsync(id);

                if (fr == null)
                {
                    return NotFound();
                }
                _context.FacultyRepresentatives.Remove(fr);
            }
            else
            {
                var al = await _context.Alumni.FindAsync(id);

                if (al == null)
                {
                    return NotFound();
                }
                _context.Alumni.Remove(al);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public IActionResult RegisterFacultyRep()
        {
            return View(new FacultyRepRegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> RegisterFacultyRep(FacultyRepRegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string? uniqueFileName = null;
                    if (model.ProfilePicture != null)
                    {
                        // File upload code
                    }

                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName, LastName = model.LastName, ProfilePicture = uniqueFileName };
                    var result = await _userManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // Role assignment code
                        await _userManager.AddToRoleAsync(user, "Faculty Representative");

                        // Add claims for email and name
                        await _userManager.AddClaimsAsync(user, new[]
                        {
                                new Claim(ClaimTypes.Name, $"{model.FirstName} {model.LastName}"),
                                new Claim(ClaimTypes.Email, model.Email),
                                new Claim(ClaimTypes.Role, "Faculty Representative")
                    });

                        // FacultyRep creation code
                        FacultyRepresentative facultyRepresentative = new()
                        {
                            UserId = user.Id,
                            Faculty = model.Faculty
                        };
                        _context.FacultyRepresentatives.Add(facultyRepresentative);
                        await _context.SaveChangesAsync();


                        return RedirectToAction(nameof(Index));
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception for debugging purposes
                    ModelState.AddModelError(string.Empty, "An error occurred during registration.");
                    // Log the exception (e.g., using a logging framework like Serilog, NLog, etc.)
                    // Log.Error(ex, "Error during registration.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RegisterAlumni()
        {
            return View(new AlumniRegisterViewModel());
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

                    // Add claims for email and name
                    await _userManager.AddClaimsAsync(user, new[]
                    {
                    new Claim(ClaimTypes.Name, $"{model.FirstName} {model.LastName}"),
                    new Claim(ClaimTypes.Email, model.Email),
                    new Claim(ClaimTypes.Role, "User")
                    });

                    var alumni = new Models.Alumni
                    {
                        UserId = user.Id,
                        FieldOfStudy = model.FieldOfStudy,
                        JobPosition = model.JobPosition,
                        GraduationDate = model.GraduationDate,
                        Company = model.Company,
                    };

                    _context.Alumni.Add(alumni);
                    await _context.SaveChangesAsync();


                    return RedirectToAction(nameof(Index));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
    }
    //private bool AlumniExists(int id)
    //{
    //    return _context.Events.Any(e => e.Id == id);
    //}
}
