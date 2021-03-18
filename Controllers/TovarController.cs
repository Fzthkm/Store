using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewStudio.Models;

namespace NewStudio.Controllers
{
    public class TovarController : Controller
    {
        private readonly context _context;

        public TovarController(context context)
        {
            _context = context;
        }

        // GET: Tovars
        public async Task<IActionResult> AdminPanel()
        {
            return View();
        }
        public async Task<IActionResult> Index(int? id)
        {
            //Фильтрация
            IQueryable<Tovar> Tovar = _context.Tovars;
            if(id == 0)
            {
                return View(Tovar);
            }
            else
            {
                Tovar = Tovar.Where(x => x.category == (category)id);
                return View(Tovar);
            }
        }
        // GET: Tovars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Tovar = await _context.Tovars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Tovar == null)
            {
                return NotFound();
            }
            return View(Tovar);
        }

        // GET: Tovars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tovars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Cost,Name,Image,category,Description")] Tovar Tovar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(Tovar);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            return View(Tovar);
        }

        // GET: Tovars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Tovar = await _context.Tovars.FindAsync(id);
            if (Tovar == null)
            {
                return NotFound();
            }
            return View(Tovar);
        }

        // POST: Tovars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Cost,Name,Image,category,Description")] Tovar Tovar)
        {
            if (id != Tovar.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(Tovar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TovarExists(Tovar.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(Tovar);
        }
        public async Task<IActionResult> Search(int? stuffs, string name)
        {
            //Фильтрация
            IQueryable<Tovar> tovar = _context.Tovars;
            if (!String.IsNullOrEmpty(name))
            {
                tovar = tovar.Where(x => x.Name.Contains(name));
            }
            svm svm = new svm
            {
                Tovars = tovar,
                viewModel = new searchViewModel(_context.Tovars.ToList(), stuffs, name)
            };
            return View(svm);

        }
        // GET: Tovars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
                
            var Tovar = await _context.Tovars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (Tovar == null)
            {
                return NotFound();
            }

            return View(Tovar);
        }

        // POST: Tovars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var Tovar = await _context.Tovars.FindAsync(id);
            _context.Tovars.Remove(Tovar);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index","Home");
        }

        private bool TovarExists(int id)
        {
            return _context.Tovars.Any(e => e.Id == id);
        }
    }
}
