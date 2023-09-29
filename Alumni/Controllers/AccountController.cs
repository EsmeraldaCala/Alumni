using Alumni.Data;
using Alumni.Models;
using Alumni.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Alumni.Controllers
{
    using Alumni = Models.Alumni;
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IScopedAuthentication _auth;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment, IScopedAuthentication auth)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _auth = auth;
        }
        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Welcome()
        {
            return View();
        }
        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                var claim = User.Claims;
                if (result.Succeeded)
                {

                    var user = await _userManager.FindByEmailAsync(model.Email);
                    var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    await _signInManager.RefreshSignInAsync(user);
                    _auth.Identity = userPrincipal.Identity;
                    return RedirectToAction(nameof(HomeController.Index), "Home");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _auth.Identity = null;
            return RedirectToAction("Index", "Home");
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
                    var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    await _signInManager.RefreshSignInAsync(user);
                    _auth.Identity = userPrincipal.Identity;
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
            return View(new FacultyRepRegisterViewModel());
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
                      
                    };

                    _dbContext.FacultyRepresentatives.Add(facultyRep);
                    await _dbContext.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var userPrincipal = await _signInManager.CreateUserPrincipalAsync(user);
                    await _signInManager.RefreshSignInAsync(user);
                    _auth.Identity = userPrincipal.Identity;
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
