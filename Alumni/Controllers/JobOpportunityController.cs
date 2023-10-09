using Alumni.Data;
using Alumni.Models;
using Alumni.Models.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Humanizer.In;

namespace Alumni.Controllers
{

    public class JobOpportunityController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IScopedAuthentication _auth;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public JobOpportunityController(ApplicationDbContext context, IScopedAuthentication auth, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _auth = auth;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            var currentUserId = _auth.GetUserId();
            //if (_auth.Identity == null)
            //    return RedirectToAction("Welcome", "Account");
            List<JobOpportunity> JobOpportunity = new List<JobOpportunity>();
            if (currentUserId != null)
            {
                JobOpportunity = await _context.JobOpportunity
                .Include(e => e.ApplicationUser).Where(e => e.UserId == currentUserId)
                .ToListAsync();
            }
            else

                JobOpportunity = await _context.JobOpportunity
                .Include(e => e.ApplicationUser)
                .ToListAsync();


            return View(JobOpportunity);
        }

      

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (_auth.Identity == null)
                return RedirectToAction("Welcome", "Account");
            if (id == null)
            {
                return NotFound();
            }

            var JobOppItem = await _context.JobOpportunity
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (JobOppItem == null)
            {
                return NotFound();
            }

            return View(JobOppItem);
        }

        // GET: Events/Create
        public IActionResult Create()
        {

            //if (_auth.Identity == null)
            //    return RedirectToAction("Welcome", "Account");
            return View(new JobOpportunity() { UserId = _auth.GetUserId() ?? 0 });
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Summary,UserId,Details,Company,Deadline, Experience, Salary, Email ")] JobOpportunity JobOppItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(JobOppItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(JobOppItem);
        }

        // GET: Events/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{

        //    //if (_auth.Identity == null)
        //    //    return RedirectToAction("Welcome", "Account");
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var JobOppItem = await _context.JobOpportunities.FindAsync(id);
        //    if (JobOppItem == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(new JobOpportunityViewModel()
        //    {
        //        Id = JobOppItem.Id,
        //        Title = JobOppItem.Title,
        //        Summary = JobOppItem.Summary,
        //        Details = JobOppItem.Details,
        //        Company = JobOppItem.Company,
        //        Deadline = JobOppItem.Deadline,
        //        UserId = JobOppItem.UserId,
        //        Experience = JobOppItem.Experience,
        //        Salary = JobOppItem.Salary,
        //        Email = JobOppItem.Email,


        //    });
        //}

        //// POST: Events/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(EventsViewModel eventItem)
        //{
        //    if (eventItem.Id != eventItem.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (eventItem.Picture != null)
        //            {
        //                string? uniqueFileName = null;
        //                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        //                uniqueFileName = Guid.NewGuid().ToString() + "_" + eventItem.Picture.FileName;
        //                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
        //                using var fileStream = new FileStream(filePath, FileMode.Create);
        //                await eventItem.Picture.CopyToAsync(fileStream);
        //                eventItem.Photo = uniqueFileName;
        //            }
        //            _context.Update(eventItem);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EventExists(eventItem.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(eventItem);
        //}

        //public async Task<IActionResult> Delete(int id)
        //{
        //    var eventItem = await _context.Events.FindAsync(id);
        //    _context.Events.Remove(eventItem);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool EventExists(int id)
        //{
        //    return _context.Events.Any(e => e.Id == id);
        //}
    }
}


