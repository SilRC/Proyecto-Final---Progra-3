using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_final.Models;
using System.Diagnostics;

namespace Proyecto_final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProyectoFinalDbContext _context;

        private readonly ILogger<HomeController> _logger;

       
        public HomeController(ProyectoFinalDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                // Buscar el usuario en la base de datos por nombre de usuario y contraseña
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.NombreUsuario == model.NombreUsuario && u.Contrasena == model.Contrasena);

                if (usuario != null)
                {
                    // Usuario autenticado, redirige a la página de inicio
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Usuario no encontrado, agrega un error al modelo
                    ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
                }
            }

            // Si el modelo no es válido o la autenticación falló, vuelve a la vista de inicio de sesión con errores
            return View();
        }

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Aquí deberías guardar el usuario en la base de datos
                // Puedes utilizar Entity Framework Core o cualquier otro ORM

                // Por ejemplo, si estás utilizando EF Core:
                // dbContext.Usuarios.Add(usuario);
                // dbContext.SaveChanges();

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                // Redirige a la página de inicio de sesión o cualquier otra página
                return RedirectToAction("Login", "Home");
            }

            // Si el modelo no es válido, vuelve a la vista de registro con errores
            return View();
        }

        public IActionResult Informes()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
