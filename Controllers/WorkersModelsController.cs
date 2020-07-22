using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Auth.AccessControlPolicy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Workers.Models;
using Workers.Service;

namespace Workers.Controllers
{
    public class WorkersModelsController : Controller
    {

        // private readonly WorkersContext _context;
        private readonly ILogger<WorkersModelsController> _logger;
       
      
        public BdBrain Bd;
     
        public WorkersModelsController(BdBrain bd, ILogger<WorkersModelsController> logger )
        {
            Bd = bd;
            _logger = logger;
            _logger.LogDebug("WorkersModelControlller");

        }
        //public WorkersModelsController(WorkersContext context)
        // {
        //     _context = context;
        //}

        // GET: WorkersModels


        public IActionResult Index() => View(Bd.GetWorkers());
        
            /* IQueryable<string> genreQuery = from m in bd.WorkersTable
                                             orderby m.Place
                                             select m.Place;
             var workers = from m in _context.WorkersTable
                           select m;

             if (!String.IsNullOrEmpty(searchString))
             {
                 workers = workers.Where(s => s.Name.Contains(searchString));
             }

             if (!string.IsNullOrEmpty(workersPlace))
             {
                 workers = workers.Where(x => x.Place == workersPlace);
             }

             var workersPlaceVM = new PlaceViewModel
             {
                 Place = new SelectList(await genreQuery.Distinct().ToListAsync()),
                 workers = await workers.ToListAsync()
             };

             return View(workersPlaceVM);*/

           // return View();
        
    
        // GET: WorkersModels/Details/5
        
        public IActionResult Details(int id)
        {
            if (Bd.GetWorkers(id) != null) return View(Bd.GetWorkers(id));
            _logger.LogWarning("Details can't be opened", id);
            return NotFound();
        }
        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(WorkersModel workers)
        {
            _logger.LogInformation("Data recieved", nameof(workers));
            if (workers == null) throw new ArgumentNullException("Data cannot be empty", nameof(workers));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Not all is fieled");
                return View();
            }
            Bd.Create(workers);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {

            if (Bd.GetWorkers(id) != null) return View(Bd.GetWorkers(id));
            
            return NotFound();
        }

        [HttpPost]
        [Authorize(Policy = "RolePolicy")]
        public IActionResult Edit(WorkersModel workers)
        {
            _logger.LogInformation("Data opened", nameof(workers));
            Bd.Edit(workers);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = "RolePolicy")]
        [ActionName("Delete")]
        public IActionResult DeleteWorker(int id)
        {
            if (Bd.GetWorkers(id) != null) return View(Bd.GetWorkers(id));
            return NotFound();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
             if (Bd.GetWorkers(id) != null) Bd.Remove(Bd.GetWorkers(id));

             _logger.LogInformation("Worker deleted", id);

             return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            var worker = Bd.GetLogin(User.Identity.Name);
            if (worker != null)
            {
                worker.Culture = culture;
                Bd.Edit(worker);
            }
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
      
    }
}
