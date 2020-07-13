using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Workers.Models;

namespace Workers.Controllers
{
    public class WorkersModelsController : Controller
    {
        private readonly WorkersContext _context;

        public WorkersModelsController(WorkersContext context)
        {
            _context = context;
        }

        // GET: WorkersModels
       /* public async Task<IActionResult> Index()
        {
            return View(await _context.WorkersTable.ToListAsync());
        }
       */
        public async Task<IActionResult> Index(string workersPlace, string searchString)
        {
            IQueryable<string> genreQuery = from m in _context.WorkersTable
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

            return View(workersPlaceVM);
        }
        // GET: WorkersModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workersModel = await _context.WorkersTable
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workersModel == null)
            {
                return NotFound();
            }

            return View(workersModel);
        }

        // GET: WorkersModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkersModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Birthday,Email,Login,Password, Place")] WorkersModel workersModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workersModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workersModel);
        }

        // GET: WorkersModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workersModel = await _context.WorkersTable.FindAsync(id);
            if (workersModel == null)
            {
                return NotFound();
            }
            return View(workersModel);
        }

        // POST: WorkersModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Birthday,Email,Login,Password, Place")] WorkersModel workersModel)
        {
            if (id != workersModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workersModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkersModelExists(workersModel.Id))
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
            return View(workersModel);
        }

        // GET: WorkersModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workersModel = await _context.WorkersTable
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workersModel == null)
            {
                return NotFound();
            }

            return View(workersModel);
        }

        // POST: WorkersModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workersModel = await _context.WorkersTable.FindAsync(id);
            _context.WorkersTable.Remove(workersModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkersModelExists(int id)
        {
            return _context.WorkersTable.Any(e => e.Id == id);
        }
    }
}
