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
    public class UsuariosController : Controller
    {
        private readonly ProyectoFinalDbContext _context;

        public UsuariosController(ProyectoFinalDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            if ((string)HttpContext.Session.GetString("admin") == "True")
            {
                return View(await _context.Usuarios.ToListAsync());
            }
            else
            {
                int? userId = HttpContext.Session.GetInt32("UserId");
                if (!userId.HasValue)
                {
                    // Si no hay un UserId en la sesión, redirige a una página de error o maneja el caso adecuadamente.
                    return RedirectToAction("Error", "Home");
                }

                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.UserId == userId.Value);
                if (usuario == null)
                {
                    // Si el usuario no se encuentra en la base de datos, maneja el caso adecuadamente.
                    return RedirectToAction("Error", "Home");
                }

                return View(new List<Usuario> { usuario });
            }
        }


        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            if ((string)HttpContext.Session.GetString("admin") == "True")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,NombreUsuario,Email,Contrasena")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);


        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,NombreUsuario,Email,Contrasena")] Usuario usuario)
        {
            if (id != usuario.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UserId))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            if ((string)HttpContext.Session.GetString("admin") == "True")
            {
                var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(m => m.UserId == id);
                if (usuario == null)
                {
                    return NotFound();
                }

                return View(usuario);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UserId == id);
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