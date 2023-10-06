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
        //public async Task<IActionResult> Index()
        //{
        //    var currentUserId = _auth.GetUserId();
        //    //if (_auth.Identity == null)
        //    //    return RedirectToAction("Welcome", "Account");
        //    List<JobOpportunity> jobOpportunities = new List<JobOpportunity>();
        //    if (currentUserId != null)
        //    {
        //        jobOpportunities = await _context.JobOpportunity
        //        .Include(e => e.ApplicationUser).Where(e => e.UserId == currentUserId)
        //        .ToListAsync();
        //    }
        //    else

        //        jobOpportunities = await _context.JobOpportunity
        //        .Include(e => e.ApplicationUser)
        //        .ToListAsync();


        //    return View(jobOpportunities);
        //}

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            if (_auth.Identity == null)
                return RedirectToAction("Welcome", "Account");
            if (id == null)
            {
                return NotFound();
            }

            var eventItem = await _context.Events
                .Include(e => e.ApplicationUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return View(eventItem);
        }

        // GET: Events/Create
        public IActionResult Create()
        {

            //if (_auth.Identity == null)
            //    return RedirectToAction("Welcome", "Account");
            return View(new Events() { UserId = _auth.GetUserId() ?? 0 });
        }

        // POST: Events/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,UserId,EventLocation,Date,TicketPrice, Photo")] Events eventItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(eventItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(eventItem);
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

            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }
            return View(new EventsViewModel()
            {
                Id = eventItem.Id,
                Date = eventItem.Date,
                EventLocation = eventItem.EventLocation,
                Description = eventItem.Description,
                TicketPrice = eventItem.TicketPrice,
                Title = eventItem.Title,
                UserId = eventItem.UserId,
                Photo = eventItem.Photo,
            });
        }

        // POST: Events/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EventsViewModel eventItem)
        {
            if (eventItem.Id != eventItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (eventItem.Picture != null)
                    {
                        string? uniqueFileName = null;
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + eventItem.Picture.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        using var fileStream = new FileStream(filePath, FileMode.Create);
                        await eventItem.Picture.CopyToAsync(fileStream);
                        eventItem.Photo = uniqueFileName;
                    }
                    _context.Update(eventItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(eventItem.Id))
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
            return View(eventItem);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}


