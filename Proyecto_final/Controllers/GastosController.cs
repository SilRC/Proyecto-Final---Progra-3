﻿using System;
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

        // GET: Gastoes
        public async Task<IActionResult> Index()
        {
            var proyectoFinalDbContext = _context.Gastos.Include(g => g.Categoria).Include(g => g.Cuenta);
            return View(await proyectoFinalDbContext.ToListAsync());
        }

        // GET: Gastoes/Details/5
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

        // GET: Gastoes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId");
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "CuentaId");
            return View();
        }

        // POST: Gastoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Gastoes/Edit/5
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", gasto.CategoriaId);
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "CuentaId", gasto.CuentaId);
            return View(gasto);
        }

        // POST: Gastoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["CategoriaId"] = new SelectList(_context.Categorias, "CategoriaId", "CategoriaId", gasto.CategoriaId);
            ViewData["CuentaId"] = new SelectList(_context.Cuentas, "CuentaId", "CuentaId", gasto.CuentaId);
            return View(gasto);
        }

        // GET: Gastoes/Delete/5
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

        // POST: Gastoes/Delete/5
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
