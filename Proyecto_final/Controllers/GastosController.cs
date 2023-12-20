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
    public class GastosController : Controller
    {
       
    private readonly ProyectoFinalDbContext _context;

        public GastosController(ProyectoFinalDbContext context)
        {
            _context = context;
        }

        // GET: Gastos
        public async Task<IActionResult> Index()
        {
            // Verificar si el usuario es "admin" basado en la sesión
            if ((string)HttpContext.Session.GetString("admin") == "True")
            {
                // Si el usuario es "admin", obtén todos los gastos
                return View(await _context.Gastos.Include(i => i.Cuenta).ToListAsync());
            }
            else
            {
                // Si el usuario no es "admin", obtén solo los gastos asociados al usuario logueado
                int loggedUserId = HttpContext.Session.GetInt32("UserId") ?? 0;
                var gastosDelUsuario = await _context.Gastos.Include(i => i.Cuenta)
                                                 .Where(i => i.Cuenta.UserId == loggedUserId)
                                                 .ToListAsync();


                return View(gastosDelUsuario);
            }
        }

        // GET: Gastos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos
                .Include(g => g.Categoria)
                .Include(g => g.Cuenta)
                .FirstOrDefaultAsync(m => m.GastoId == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // GET: Gastos/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "NombreCategoria");
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "NombreCuenta");
            return View();
        }

        // POST: Gastos/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GastoId,CuentaId,CategoriaId,Descripcion,Monto,FechaGasto")] Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gasto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", gasto.CategoriaId);
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "CuentaId", gasto.CuentaId);
            return View(gasto);
        }

        // GET: Gastos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto == null)
            {
                return NotFound();
            }
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "NombreCategoria", gasto.CategoriaId);
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "NombreCuenta", gasto.CuentaId);
            return View(gasto);
        }

        // POST: Gastos/Edit/5
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GastoId,CuentaId,CategoriaId,Descripcion,Monto,FechaGasto")] Gasto gasto)
        {
            if (id != gasto.GastoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gasto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GastoExists(gasto.GastoId))
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "NombreCategoria", gasto.CategoriaId);
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "NombreCuenta", gasto.CuentaId);
            return View(gasto);
        }

        // GET: Gastos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gasto = await _context.Gastos
                .Include(g => g.Categoria)
                .Include(g => g.Cuenta)
                .FirstOrDefaultAsync(m => m.GastoId == id);
            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }

        // POST: Gastos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gasto = await _context.Gastos.FindAsync(id);
            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GastoExists(int id)
        {
            return _context.Gastos.Any(e => e.GastoId == id);
        }
    }
}
