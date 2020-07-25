using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
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
      
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(User.Identity.Name)) ViewBag.Message = User.Identity.Name;
            else ViewBag.Message = "None";

            var worker = Bd.GetLogin(User.Identity.Name);
            if (worker != null) ViewBag.CurrentCulture = worker.Culture;

            return  View(Bd.GetWorkers_workers());
        }
        
        
        public IActionResult Details(int id)
        {
            if (Bd.GetWorkers(id) != null) return View(Bd.GetWorkers(id));
            _logger.LogWarning("Details can't be opened", id);
            return NotFound();
        }
       

        [HttpGet]
        [Authorize(Policy = "Policy_role")]
        public IActionResult Edit(int id)
        {
            if (Bd.GetWorkers(id) != null) return View(Bd.GetWorkers(id));
            return NotFound();
        }

        [HttpPost]
        [Authorize(Policy = "Policy_role")]
        public IActionResult Edit(WorkersModel workers)
        {
            _logger.LogInformation("Data opened", nameof(workers));
            Bd.Edit(workers);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = "Policy_role")]
        [ActionName("Delete")]
        public IActionResult DeleteWorker(int id)
        {
            if (Bd.GetWorkers(id) != null) return View(Bd.GetWorkers(id));
            return NotFound();
        }

        [HttpPost]
        [Authorize(Policy = "Policy_role")]
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
