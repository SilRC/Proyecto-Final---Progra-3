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
    public class IngresosController : Controller
    {
        private readonly ProyectoFinalDbContext _context;

        public IngresosController(ProyectoFinalDbContext context)
        {
            _context = context;
        }

        // GET: Ingresoes
        public async Task<IActionResult> Index()
        {
            // Verificar si el usuario es "admin" basado en la sesión
            if ((string)HttpContext.Session.GetString("admin") == "True")
            {
                // Si el usuario es "admin", obtén todos los ingresos
                return View(await _context.Ingresos.Include(i => i.Cuenta).ToListAsync());
            }
            else
            {
                // Si el usuario no es "admin", obtén solo los ingresos asociados al usuario logueado
                int loggedUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                var ingresosDelUsuario = await _context.Ingresos.Include(i => i.Cuenta)
                                                 .Where(i => i.Cuenta.UserId == loggedUserId)
                                                 .ToListAsync();


                return View(ingresosDelUsuario);
            }
        }

            // GET: Ingresoes/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.Cuenta)
                .FirstOrDefaultAsync(m => m.IngresoId == id);
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // GET: Ingresoes/Create
        public IActionResult Create()
        {
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "CuentaId");
            return View();
        }

        // POST: Ingresoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IngresoId,CuentaId,Descripcion,Monto,FechaIngreso")] Ingreso ingreso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ingreso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "CuentaId", ingreso.CuentaId);
            return View(ingreso);
        }

        // GET: Ingresoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso == null)
            {
                return NotFound();
            }
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "CuentaId", ingreso.CuentaId);
            return View(ingreso);
        }

        // POST: Ingresoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IngresoId,CuentaId,Descripcion,Monto,FechaIngreso")] Ingreso ingreso)
        {
            if (id != ingreso.IngresoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ingreso);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!IngresoExists(ingreso.IngresoId))
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
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "CuentaId", ingreso.CuentaId);
            return View(ingreso);
        }

        // GET: Ingresoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingreso = await _context.Ingresos
                .Include(i => i.Cuenta)
                .FirstOrDefaultAsync(m => m.IngresoId == id);
            if (ingreso == null)
            {
                return NotFound();
            }

            return View(ingreso);
        }

        // POST: Ingresoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingreso = await _context.Ingresos.FindAsync(id);
            if (ingreso != null)
            {
                _context.Ingresos.Remove(ingreso);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool IngresoExists(int id)
        {
            return _context.Ingresos.Any(e => e.IngresoId == id);
        }
    }
}
