 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NewStudio.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace NewStudio.Views
{
    public class OrdersController : Controller
    {
        private readonly context _context;

        public OrdersController(context context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var context = _context.orders.Include(o => o.User).Include(o => o.Tovar);
            return View(await context.ToListAsync());
        }

        public async Task<IActionResult> Oplata(int id)
        {
            IQueryable<Order> orders = _context.orders;
            orders = orders.Where(x => x.User._Username == User.Identity.Name);
            orders = orders.Where(x => x.status == status.Open);
            foreach (Order order in orders)
            {
                order.status = status.Close;
                if (id == 1)
                {
                    order.type = global::type.Cash;
                }
                else
                {
                    order.type = global::type.Card;
                }
                _context.Update(order);
            }
            _context.SaveChanges();
            return RedirectToAction("Cart");
        }
        
        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.orders
                .Include(o => o.User)
                .Include(o => o.Tovar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["TovarId"] = new SelectList(_context.Tovars, "Id", "Id");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,_Count,type,status,TovarId,UserId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            ViewData["TovarId"] = new SelectList(_context.Tovars, "Id", "Id", order.TovarId);
            return View(order);
        }

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            ViewData["TovarId"] = new SelectList(_context.Tovars, "Id", "Id", order.TovarId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,_Count,type,status,TovarId,UserId")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", order.UserId);
            ViewData["TovarId"] = new SelectList(_context.Tovars, "Id", "Id", order.TovarId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.orders
                .Include(o => o.User)
                .Include(o => o.Tovar)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.orders.FindAsync(id);
            _context.orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.orders.Any(e => e.Id == id);
        }
        public async Task<IActionResult> Cart()
        {
            User currentUser = await _context.Users.FirstOrDefaultAsync(x => x._Username == User.Identity.Name);
            IQueryable<Order> orders = _context.orders
                .Include(x => x.Tovar)
                .Include(x => x.User).Where(x => x.UserId == currentUser.Id);
            orders = orders.Where(x => x.status == status.Open);
            return View(orders);
        }
        [Authorize]
        public async Task<IActionResult> addToCart(int? id)
        {
            User currentUser = await _context.Users.FirstOrDefaultAsync(x => x._Username == User.Identity.Name);
            foreach (Order orders in _context.orders)
            {
                if (orders.UserId == currentUser.Id && orders.TovarId == id)
                {
                    orders._Count++;
                    id = null;
                    _context.orders.Update(orders);
                }

            }
            if (id != null)
            {
                Order order = new Order { TovarId = (int)id, UserId = currentUser.Id, _Count = 1, type = type.Card, status = status.Open };
                _context.orders.Add(order);
            }
            _context.SaveChanges();
            return RedirectToAction("Index","Home");
        }
    }
}
