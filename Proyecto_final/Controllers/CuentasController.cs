using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Proyecto_final.Models;

namespace Proyecto_final.Controllers
{
    public class CuentasController : Controller
    {
        private readonly ProyectoFinalDbContext _context;

        public CuentasController(ProyectoFinalDbContext context)
        {
            _context = context;
        }

        // GET: Cuentas
        public async Task<IActionResult> Index()
        {
            if ((string)HttpContext.Session.GetString("admin") == "True")
            {
                // Si el usuario es "admin", obtén todas las cuentas
                return View(await _context.Cuentas.Include(c => c.User).ToListAsync());
            }
            else
            {
                // Si el usuario no es "admin", obtén solo las cuentas del usuario logueado
                int loggedUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                var cuentasDelUsuario = await _context.Cuentas.Include(c => c.User)
                                                               .Where(c => c.UserId == loggedUserId)
                                                               .ToListAsync();
                return View(cuentasDelUsuario);
            }
        }


        // GET: Cuentas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.Cuentas
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CuentaId == id);
            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // GET: Cuentas/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Usuarios, "UserId", "UserId");
            return View();
        }

        // POST: Cuentas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CuentaId,UserId,NombreCuenta,SaldoInicial,FechaCreacion")] Cuenta cuenta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cuenta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Usuarios, "UserId", "UserId", cuenta.UserId);
            return View(cuenta);
        }

        // GET: Cuentas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.Cuentas.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Usuarios, "UserId", "UserId", cuenta.UserId);
            return View(cuenta);
        }

        // POST: Cuentas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CuentaId,UserId,NombreCuenta,SaldoInicial,FechaCreacion")] Cuenta cuenta)
        {
            if (id != cuenta.CuentaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cuenta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CuentaExists(cuenta.CuentaId))
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
            ViewData["UserId"] = new SelectList(_context.Usuarios, "UserId", "UserId", cuenta.UserId);
            return View(cuenta);
        }

        // GET: Cuentas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cuenta = await _context.Cuentas
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CuentaId == id);
            if (cuenta == null)
            {
                return NotFound();
            }

            return View(cuenta);
        }

        // POST: Cuentas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cuenta = await _context.Cuentas.FindAsync(id);
            if (cuenta != null)
            {
                _context.Cuentas.Remove(cuenta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CuentaExists(int id)
        {
            return _context.Cuentas.Any(e => e.CuentaId == id);
        }
    }
}


/*
            if ((string)HttpContext.Session.GetString("admin") == "True")
            {

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
 */